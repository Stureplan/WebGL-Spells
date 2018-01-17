Shader "Custom/Particles/ParticleDistorted"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D)="white"{}
		_Intensity("Intensity", Vector) = (1,1,0,0)
		_Multiplier("Emission Multiplier", Float) = 1
	}
	SubShader
	{
		Tags 
		{
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent" 
		}

		LOD 100

		Pass
		{
			Blend One One
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 vertexColor : COLOR;
			};
			
			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 vertexColor : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float4 _Intensity;
			float  _Multiplier;


			
			VertexOutput vert (VertexInput v)
			{
				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.texcoord1, _NoiseTex);
				o.vertexColor = v.vertexColor;

				return o;
			}
			
			fixed4 frag (VertexOutput i) : SV_Target
			{
				float lt = i.vertexColor.a;
				
				i.uv1 += _Time.yy * _Intensity.zw;

				// Sample noise with offset noise UV's
				float4 noise = tex2D(_NoiseTex, i.uv1);

				//i.uv0 -= ((noise.rg) - 0.5) * 2 * 0.1 * lt;
				i.uv0 += (noise.rg * 2 - 1) * _Intensity.xy;
				i.uv0 -= _Intensity.xy;

				//float2 offset = (noise.rg * 2 - 1) * _Intensity.xy;
				float4 color = tex2D(_MainTex, i.uv0);
				//float3 emissive = lerp(float3(1, 1, 1), color.rgb * i.vertexColor.rgb, color.a);

				noise.rgb = clamp(noise.rgb, float3(0, 0, 0), float3(1, 1, 1)) * color.a*lt;
				float n = noise.r -(1.0001-lt+noise.g);
				clip(n);
				//float a = lerp(0, (lt*color.a), n * 10);
				float3 emissive = (color.rgb * i.vertexColor.rgb);// *(color.a * lt);
				emissive *= _Multiplier;

				return float4(emissive,0);
			}
			ENDCG
		}
	}
	FallBack "Mobile/Particles/Additive"
}

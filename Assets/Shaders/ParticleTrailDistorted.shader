Shader "Custom/Particles/ParticleTrailDistorted"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Frequency("Frequency", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		_ColorStrength("Color Strength", Float) = 1
	}
	SubShader
	{
		Tags 
		{
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"DisableBatching"="True"
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
				float4 vertexColor : COLOR;
			};
			
			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float _Frequency;
			float _Amplitude;
			float _ColorStrength;


			float rand(float3 co)
			{
				return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 8.5453);
			}

			float3 distort(float3 v, float t)
			{
				t *= 5;
				v.x += (_Amplitude * sin(_Time.y*_Frequency + v.z)) * t;

				//v.z += sin(_Time.y*_Frequency + v.z) * t * 0.1;


				return v;
			}

			
			VertexOutput vert (VertexInput v)
			{
				VertexOutput o;
				o.pos = v.vertex;

				o.pos.xyz = distort(v.vertex.xyz, 1 - v.vertexColor.a);

				o.pos = UnityObjectToClipPos(o.pos);

				o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex) + float2(-_Time.y * 1.0, 0);
				o.vertexColor = v.vertexColor;

				return o;
			}
			
			fixed4 frag (VertexOutput i) : SV_Target
			{
				float lt = i.vertexColor.a;
				float4 color = tex2D(_MainTex, i.uv0) * i.vertexColor;

				float4 noise = tex2D(_NoiseTex, i.uv0);

				color.rgb *= lt * noise.r * _ColorStrength;

				return color;
			}
			ENDCG
		}
	}
	FallBack "Mobile/Particles/Additive"
}

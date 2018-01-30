Shader "Custom/Particles/ParticleDistorted_Mult_Color"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white"{}
		_Intensity("Intensity", Vector) = (1,1,0,0)
		_Multiplier("Emission Multiplier", Float) = 1
		_Color("Color", Color) = (1,1,1,1)
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
			//Blend SrcAlpha OneMinusSrcAlpha
			Blend One One
			ColorMask RGB
			Cull Off Lighting Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _Intensity;
			float4 _MainTex_ST;
			float  _Multiplier;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 time;
				time.x = 0;
				time.y = _Time.y;
				fixed4 noise = tex2D(_NoiseTex, 0.5*i.uv+(time*0.25));
				float lt = 1-i.color.a;

				i.uv -= (((noise.rg) - 0.5) * 2 * 0.1 * lt) * _Intensity.xy;
				fixed4 color = tex2D(_MainTex, i.uv);
				
				float n = noise.r - lt*1;
				clip(n);

				float3 emissive = (color.rgb * _Color);
				emissive *= color.a * _Multiplier;

				return float4(emissive, color.a);
			}
			ENDCG
		}
	}
}

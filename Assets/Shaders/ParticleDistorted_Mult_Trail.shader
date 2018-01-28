Shader "Custom/Particles/ParticleDistorted_Mult_Trail"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white"{}
		_Intensity("Intensity", Vector) = (1,1,0,0)
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
			Blend SrcAlpha OneMinusSrcAlpha
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
				float lt = 1 - i.color.a;

				fixed4 noise = tex2D(_NoiseTex, i.uv*0.25);
				float n = noise.r - lt;
				clip(n);
				
				i.uv -= (((noise.rg-lt) - 0.5) * 2 * 0.1 * lt) * _Intensity.xy;
				fixed4 color = tex2D(_MainTex, i.uv);

				color.rgb *= i.color.rgb;
				return color;
			}
			ENDCG
		}
	}
}

Shader "Unlit/HealthGlobe"
{
	Properties
	{
		_MainTex ("BG Texture", 2D) = "white" {}
		_HealthTex("Health Texture", 2D) = "white" {}
		_DetailTex("Detail Texture", 2D) = "white" {}
		_Health ("Health", Range(0, 100)) = 50
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" "DisableBatching"="True"}
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha:fade
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _HealthTex;
			sampler2D _DetailTex;
			float4 _MainTex_ST;
			float _Health;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float liquid_line(float2 uv)
			{
				float y = uv.y + (sin((_Time.y*5) + uv.x * 25)*0.005);

				y += sin(_Time.y*2-uv.x*5) * 0.025;

				return y;
			}
			
			float2 spherical_distortion(float2 uv, float dist)
			{
				uv = uv *2.0 - 1.0;
				float r = uv.x * uv.x + uv.y * uv.y;
				uv *= 1.6 + dist * r + dist * r * r;
				uv = 0.5 * (uv * 0.5 + 1.0);

				uv += _Time.y*0.1;
				return uv;
			}

			fixed4 frag (v2f i) : SV_Target
			{


				_Health *= 0.01;


				float2 dt_uv = i.uv;
				//dt_uv += sin(dt_uv.x * 0.1 + _Time.y)*0.1;
				
				dt_uv = spherical_distortion(dt_uv, 0.5);



				fixed4 bg = tex2D(_MainTex, i.uv);
				fixed4 fg = tex2D(_HealthTex, i.uv); //fixed4(0.8, 0, 0, 1);

				dt_uv.x += fg.r*0.25;
				fixed4 dt = tex2D(_DetailTex, dt_uv);
				fixed4 hl = (fg *10)+(dt*0.5*dt.a);


				float2 uv = i.uv;
				float y = liquid_line(uv);
				

				float wave = sin(10 * _Time.y + uv.x * 10)*0.01;
				float s = smoothstep(_Health - 0.015, _Health + 0.015, y+wave);
				hl.a = 1-s;
				hl.a *= fg.a;

				// Top wave stuff
				fg.a *= 1 - step(_Health, y + wave);
				hl.a *= smoothstep(_Health - 0.03, _Health + 0.05, y);


				// Combine
				fg.rgb = lerp(fg.rgb, dt.rgb, dt.a);
				bg.rgb = lerp(bg.rgb, fg.rgb, fg.a);
				bg.rgb = lerp(bg.rgb, hl.rgb, hl.a);

				
				return bg;
			}
			ENDCG
		}
	}
}

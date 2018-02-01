Shader "Custom/AdditiveCrackingShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_CrackTex("Crackling Texture", 2D) = "white" {}
		_CrackAmount("Crackling Amount", Range(0, 1)) = 0.0
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha One
		ColorMask RGB
		AlphaTest Greater .01
		Cull Off Lighting Off ZWrite Off

		LOD 100

		Pass
	{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash
			#pragma multi_compile_particles
			#include "UnityCG.cginc"
			//#pragma surface surf Lambert vertex:vert noforwardadd alpha:fade


			sampler2D _MainTex;
			sampler2D _CrackTex;
			float _CrackAmount;
			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			float4 _Color;
			float4 _MainTex_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 proj : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 proj : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.proj = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.proj.z);
				o.color = v.color;

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, i.uv);

				float z = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.proj)));
				float zz = i.proj.z;
				float fade = saturate(_InvFade * (z - zz));
				_Color.a *= fade;

				fixed4 crack = tex2D(_CrackTex, (i.uv*1.5));

				crack *= _CrackAmount*0.2;
				fixed4 color = (2 * i.color * c * _Color);

				color.rgb += crack.rgb;
				return color;
			}
			ENDCG
		}
	}
}

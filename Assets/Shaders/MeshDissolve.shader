Shader "Custom/MeshDissolve" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_NoiseMap("Noise Map", 2D) = "white"{}
		_BurnMap("Burn Map", 2D) = "white"{}
		_Dissolve("Dissolve", Range(0,1.5)) = 0
		_Burn("Burn", Range(0,1)) = 0

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _NoiseMap;
		sampler2D _BurnMap;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		half _Dissolve;
		half _Burn;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			o.Smoothness = _Glossiness;

			float2 uv = IN.worldPos.rg * 0.15; //NOTE: Replace with IN.uvMainTex if you want normal UV's
			clip(tex2D(_NoiseMap, uv).rgb - _Dissolve);

			half test = tex2D(_NoiseMap, uv).rgb - _Dissolve;
			if (test < _Burn && _Dissolve > 0 && _Dissolve < 1)
			{
				o.Emission = tex2D(_BurnMap, float2(test*(1 / _Burn), 0)) * 2;
			}
			


			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

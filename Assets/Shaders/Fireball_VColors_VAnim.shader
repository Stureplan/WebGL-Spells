// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Fireball_VColors_VAnim" 
{
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Shakiness("Shakiness", Range(0,1)) = 0.5
		_Speed("Speed", Range(0,10)) = 1
		_Bloom("Bloom", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard noforwardadd vertex:vert alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		half _Glossiness;
		half _Metallic;
		half _Shakiness;
		half _Speed;
		half _Bloom;

		//UNITY_INSTANCING_CBUFFER_START(Props)
		//UNITY_INSTANCING_CBUFFER_END

		float rand(float3 co)
		{
			return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 8.5453);
		}

		void vert(inout appdata_full v)
		{
			// Object space distance
			float localDistance = distance(v.vertex.xyz, fixed3(0, 0, 0));

			float sx = sin(_Time.y * 2.5 + v.vertex.y) * 1;
			float cx = cos(_Time.y * 2.5 + v.vertex.y) * 2;

			float sy = sin(_Time.y * _Speed + v.vertex.y) * 1;
			float cy = cos(_Time.y * _Speed + v.vertex.y) * 2;

			float intensity = (v.color.r + v.color.g + v.color.b)*2;

			v.vertex.x += ((v.normal.x / localDistance)*(v.vertex.x * 2) * sx * cx * 0.05 * (localDistance * 1) + (rand(v.vertex.xyz)*0.01 * localDistance)) / (2-intensity);
			v.vertex.y += ((v.normal.y / localDistance)*(v.vertex.y * 2) * sy * cy * 0.1 * (localDistance * 1)) / (1-intensity);
			
			v.vertex.z += ((v.normal.z / localDistance)*(v.vertex.z * 2) * sy * 0.1 * (localDistance * 1) + (rand(v.vertex.xyz) * 0.05 * localDistance)) / (1-intensity);

			v.vertex.z += (sin(_Time.y * 50 + v.vertex.z * localDistance) *2) * localDistance*0.01;
			
			//Comment this in to get a wave in X
			//v.vertex.x += (sin(_Time.y * 0.1 + v.vertex.z * 1* localDistance) * 0.2) / max(localDistance,2) * v.normal.x;

			//v.vertex.z += (sin(_Time.y * 10 + v.vertex.z * 0.5 * localDistance));

			// Vertex in world space
			float4 ws_v = mul(unity_ObjectToWorld, v.vertex);


			//:::::::::::::::::::::::::::::::
			//do transformations to ws_v here


			//:::::::::::::::::::::::::::::::


			// Vertex back to object space
			v.vertex = mul(unity_WorldToObject, ws_v);
		}

		void surf (Input input, inout SurfaceOutputStandard output) 
		{
			float s = sin(_Time.y * 10);
			float c = cos(_Time.y);

			output.Albedo = input.color;
			output.Metallic = 0;
			output.Smoothness = 0;
		
			float4 col = tex2D(_MainTex, input.uv_MainTex);
			output.Alpha = col.a;

			output.Emission = (input.color.rgb * _Bloom);
		}
		ENDCG
	}
	FallBack "Diffuse"
}

Shader "Custom/Particles/ParticleTrailDistorted"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Intensity("Intensity", Float) = 1
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
			float4 _MainTex_ST;
			float _Intensity;

			float3 distort(float3 v, float t)
			{
				float amplitude = 1;
				float frequency = 1;


				float range = distance(v.xyz, mul(unity_WorldToObject, float3(0,0,0)));


				v.x += ((amplitude * sin(_Time.y*frequency)) * range);

				v.xyz = mul(unity_WorldToObject, v.xyz);

				return v;
			}
			
			VertexOutput vert (VertexInput v)
			{
				VertexOutput o;
				o.pos = v.vertex;

				o.pos.xyz = distort(mul(unity_ObjectToWorld,v.vertex.xyz), 1 - v.vertexColor.a);

				o.pos = UnityObjectToClipPos(o.pos);

				o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex) + float2(_Time.y, 0);
				o.vertexColor = v.vertexColor;

				return o;
			}
			
			fixed4 frag (VertexOutput i) : SV_Target
			{
				float lt = i.vertexColor.a;
				float4 color = tex2D(_MainTex, i.uv0) * i.vertexColor;
				
				return color;
			}
			ENDCG
		}
	}
	FallBack "Mobile/Particles/Additive"
}

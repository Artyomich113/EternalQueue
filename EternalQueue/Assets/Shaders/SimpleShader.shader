Shader "Unlit/SimpleShader"
{
	Properties
	{
	_Color("Color" , Color) = (1,1,1,1)
	_Gloss("Gloss", Float) = 1
	_Falloff("Falloff", Float) = 0
	_Reflect("Reflect", Float) = 1
	_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
	Tags { "RenderType" = "Opaque" }
	LOD 100

	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"

		struct VertexInput
		{
			float4 vertex : POSITION;
			float2 uv0 : TEXCOORD0;
			float3 normal : NORMAL;

		};

		struct VertexOutput
		{
			float4 vertex : SV_POSITION;
			float2 uv0 : TEXCOORD0;
			float3 normal : TEXCOORD1;
			float3 worldPos : TEXCOORD2;
		};


		float4 _Color;
		float _Gloss; 
		float _Falloff;
		float _Reflect;
		float3 _MousePos;
		sampler2D _MainTex;

		float MyLerp(float3 a, float3 b, float t )
		{
			return t * b + (1.0 - t)* a;
		}

		float InvLerp(float a, float b, float value)
		{
			return (value - a)/(b - a);
		}

		float Posterize(float steps, float value)
		{
			return floor(value * steps) / steps;
		}


		VertexOutput vert(VertexInput v)
		{
			VertexOutput o;
			o.uv0 = v.uv0;
			o.normal = v.normal;
			o.worldPos = mul(unity_ObjectToWorld,v.vertex);


			o.vertex = UnityObjectToClipPos(v.vertex);
			return o;
		}

		fixed4 frag(VertexOutput i) : SV_Target
		{
		//float dist = distance (_MousePos, i.worldPos);

		///float glow = saturate(1- dist);

		//return dist;
		float2 uv = i.uv0;
		float3 normal = normalize(i.normal);

	
		//direct light
		float3 lightdir = _WorldSpaceLightPos0.xyz; //float3(1,1,1);
		float3 lightcolor = _LightColor0.rgb;//float3(0.9,0.82,0.7);
		
		float lightFalloff = saturate(dot(lightdir , normal) + _Falloff);
		//lightFalloff = step( 0.6 , lightFalloff);
		float3 directDiffuseLight = lightcolor * lightFalloff;
		
		//ambientlight
		float3 ambientlight = float3(0.1, 0.1, 0.1);

		//direct Specular light
		float3 cameraPos = _WorldSpaceCameraPos;
		
		float3 viewDir = normalize(cameraPos - i.worldPos);

		float3 reflectedVect = reflect(viewDir, normal);
		
		float3 SpectacularFalloff = max(0 ,dot(-reflectedVect, lightdir));
		SpectacularFalloff = pow(SpectacularFalloff,_Gloss);
		
		float3 directSpecular = SpectacularFalloff  * lightcolor;
		
		//return float4(directSpecular,1);


		float3 diffuseLight = ambientlight + directDiffuseLight;
		float3 finaleColor =   diffuseLight *tex2D(_MainTex, i.uv0) * _Color.rgb + directSpecular * _Reflect;

		return float4(finaleColor,0);
		}
		ENDCG
	}
	}
}

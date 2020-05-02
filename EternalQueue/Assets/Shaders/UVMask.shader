Shader "Unlit/UVMask"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color",color) = (1,1,1,1)
		_StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
       _StencilOp("Stencil Operation", Float) = 0
       _StencilWriteMask("Stencil Write Mask", Float) = 255
       _StencilReadMask("Stencil Read Mask", Float) = 255
       _ColorMask("Color Mask", Float) = 15
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
				#pragma multi_compile_fog

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
				float4 _MainTex_ST;
				float4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv) * _Color * i.uv.y;

					return col;
				}
				ENDCG
			}
		}
}

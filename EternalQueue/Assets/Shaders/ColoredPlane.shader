Shader "Unlit/ColoredPlane"
{
    Properties
    {
        _Color("Color" , Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _StrechX ("StrechX", float) = 1
        _StrechY ("StrechY", float) = 1
        _SteppedColor("SteppedColor" , Color) = (1,1,1,1)
        _SteppedTileX("SteppedTileX", int) = 0
        _SteppedTileY("SteppedTileY", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
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

            bool IsBetween(float val, int leftvalue, int rightvalue)
            {
                return (val > leftvalue && val < rightvalue);
            }

            float4 _SteppedColor;
            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _StrechX;
            float _StrechY;
            float _SteppedTileX;
            float _SteppedTileY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = float2(v.uv.x * _StrechX, v.uv.y * _StrechY);
               
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                if(IsBetween(i.uv.x, _SteppedTileX, _SteppedTileX + 1) && IsBetween(i.uv.y, _SteppedTileY, _SteppedTileY + 1))
                {
                    col = tex2D(_MainTex, i.uv) * _SteppedColor;
                }
                else
                {
                col = tex2D(_MainTex, i.uv) * _Color;
                }
                return col;
            }
            ENDCG
        }
    }
}

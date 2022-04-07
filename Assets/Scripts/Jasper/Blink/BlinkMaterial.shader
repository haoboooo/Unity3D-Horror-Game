Shader "Unlit/BlinkMaterial"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _height("y value", Range(0, 1)) = 1
        _alpha("blackness", Range(0, 1)) = 1
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
            uniform float _height;
            uniform float _alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = float2(i.uv.x - 0.5, i.uv.y - 0.5);
                if (_height < 0.001 || (uv.x * uv.x) / 0.275 + (uv.y * uv.y) / (_height * _height) > 1)
                {
                    return lerp(fixed4(0, 0, 0, 1), col, _alpha);
                }
                else
                {
                    return col;
                }
            }
            ENDCG
        }
    }
}

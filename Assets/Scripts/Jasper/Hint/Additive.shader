Shader "Unlit/Additive"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BloomTex("Texture", 2D) = "white" {}
        _HighlightColor("target color", Color) = (0,0,0,0)
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

            sampler2D _MainTex, _BloomTex;
            float4 _MainTex_ST;
            uniform fixed4 _HighlightColor;

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
                fixed4 bloomColor = tex2D(_BloomTex, i.uv);
                fixed4 grayScaleColor = tex2D(_MainTex, i.uv);

                if(abs(grayScaleColor.x - _HighlightColor.x) < 0.01 && abs(grayScaleColor.y - _HighlightColor.y) < 0.01 && abs(grayScaleColor.z - _HighlightColor.z) < 0.01)
                {
                    return grayScaleColor;
                }
                return grayScaleColor + bloomColor;
            }
            ENDCG
        }
    }
}

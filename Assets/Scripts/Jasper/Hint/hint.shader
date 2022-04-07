Shader "Hidden/hint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _bwBlend("Black & White blend", Range(0, 1)) = 0
        _HighlightColor("highlight color", Color) = (0,0,0,0)
        _TargetColor("target color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            uniform float _bwBlend;
            uniform fixed4 _HighlightColor;
            uniform fixed4 _TargetColor;
            uniform fixed4 _HighlightStartColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 finalColor;
                if (abs(col.x - _TargetColor.x) < 0.01 && abs(col.y - _TargetColor.y) < 0.01 && abs(col.z - _TargetColor.z) < 0.01)
                {
                    finalColor = _HighlightColor;
                }
                else
                {
                    float intensity = col.x * 0.3 + col.y * 0.59 + col.z * 0.11;
                    finalColor = fixed4(intensity, intensity, intensity, 1);
                    if (_bwBlend < 1.0)
                    {
                        finalColor.rgb = lerp(col.rgb, finalColor.rgb, _bwBlend);
                    }
                }
                return finalColor;
            }
            ENDCG
        }
    }
}

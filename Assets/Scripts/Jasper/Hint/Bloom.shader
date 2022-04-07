Shader "Unlit/Bloom"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", Range(0,0.2)) = 0.05
		_HighlightColor("target color", Color) = (0,0,0,0)
	}

	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _BlurSize;
		uniform fixed4 _HighlightColor;

		struct VertexData {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct Interpolators {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		Interpolators VertexProgram(VertexData v) {
			Interpolators i;
			i.pos = UnityObjectToClipPos(v.vertex);
			i.uv = v.uv;
			return i;
		}
	ENDCG

	SubShader
    {
		Pass
		{
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram
				fixed4 FragmentProgram(Interpolators i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					if (abs(col.x - _HighlightColor.x) < 0.01 && abs(col.y - _HighlightColor.y) < 0.01 && abs(col.z - _HighlightColor.z) < 0.01)
					{
						return col;
					}
					return fixed4(0, 0, 0, 0);
				}
			ENDCG
		}

		Pass
		{ 
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram
                fixed4 FragmentProgram(Interpolators i) : SV_Target
                {
                    float4 col = 0;
                    for (float index = 0; index < 10; index++) {
                        //get uv coordinate of sample
                        float2 uv = i.uv + float2(0, (index / 9 - 0.5) * _BlurSize);
                        //add color at position to color
                        col += tex2D(_MainTex, uv);
                    }
                    //divide the sum of values by the amount of samples
                    col = col / 10;
                    return col;
                }
			ENDCG
		}

		Pass
		{
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram
				fixed4 FragmentProgram(Interpolators i) : SV_Target
				{
					//calculate aspect ratio
					float invAspect = _ScreenParams.y / _ScreenParams.x;
					//init color variable
					float4 col = 0;
					//iterate over blur samples
					for (float index = 0; index < 10; index++) {
						//get uv coordinate of sample
						float2 uv = i.uv + float2((index / 9 - 0.5) * _BlurSize * invAspect, 0);
						//add color at position to color
						col += tex2D(_MainTex, uv);
					}
					//divide the sum of values by the amount of samples
					col = col / 10;
					return col;
				}
			ENDCG
		}
	}
}
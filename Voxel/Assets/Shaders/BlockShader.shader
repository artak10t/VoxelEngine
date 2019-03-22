Shader "BlockShader" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader
		{
			Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct vertexData
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct fragmentData
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Cutoff;

				fragmentData vert(vertexData v)
				{
					fragmentData o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(fragmentData i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv) * i.color;
					clip(col.a - _Cutoff);
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}

				ENDCG
			}
		}
}
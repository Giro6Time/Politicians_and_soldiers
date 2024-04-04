Shader "Custom/BorderColorShader"
{
	//目标：写一个能够图片控制亮度的
	properties
	{
		_MainTex("MainTex",2d) = ""{}
		_BorderColor("BorderColor",color)=(1,1,1,1)
	}
		SubShader
		{
			pass
			{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "unitycg.cginc"

			sampler2D _MainTex;

			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				//进行MVP变换
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}
			fixed4 frag(v2f IN) :COLOR
			{
				//锁定边框范围
			
				fixed4 color = tex2D(_MainTex,IN.uv);
				return color;
			}
			ENDCG

			}
		}
}

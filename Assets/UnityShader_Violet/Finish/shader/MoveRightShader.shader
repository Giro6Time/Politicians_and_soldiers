Shader "Custom/MoveRightShader"
{
	//目标：写一个能够使得右上角发生移动的
	properties
	{
		_MainTex("MainTex",2d) = ""{}
		_Bright("Bright",range(0,1)) = 0.5
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
			float _Bright;

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
				//获取UV坐标
				o.uv = v.texcoord.xy;
				return o;
			}
			fixed4 frag(v2f IN) :COLOR
			{
				fixed4 color = tex2D(_MainTex,IN.uv);
				color *= _Bright;
				return color;
			}
			ENDCG

			}
		}
}

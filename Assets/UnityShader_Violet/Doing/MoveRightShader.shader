Shader "Custom/MoveRightShader"
{
	//目标：写一个能够使得右上角发生移动的
	properties
	{
		_MainTex("MainTex",2d) = ""{}
		_A("A",range(0,1)) = 0.2
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
			float2 offset;
			float _A;//振幅
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
				//1.锁定范围：uv坐标的：[0-0.5,0.6-1]的
				if (IN.uv.x >= 0.0 && IN.uv.x <= 0.5 && IN.uv.y >= 0.6 && IN.uv.y <= 1.0)
				{
					float sway = (_A * IN.uv.x * _Time.y) % 1;
					offset = float2(0.0, sway);	
				}
				fixed4 color = tex2D(_MainTex,IN.uv+offset);
				return color;
			}
			ENDCG

			}
		}
}

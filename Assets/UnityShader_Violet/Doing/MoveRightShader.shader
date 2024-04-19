Shader "Custom/MoveRightShader"
{
	//Ŀ�꣺дһ���ܹ�ʹ�����ϽǷ����ƶ���
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
			float _A;//���
			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				//����MVP�任
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}
			fixed4 frag(v2f IN) :COLOR
			{ 
				//1.������Χ��uv����ģ�[0-0.5,0.6-1]��
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

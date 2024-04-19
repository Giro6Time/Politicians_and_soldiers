Shader "Custom/MoveRightShader"
{
	//Ŀ�꣺дһ���ܹ�ʹ�����ϽǷ����ƶ���
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
				//����MVP�任
				o.pos = UnityObjectToClipPos(v.vertex);
				//��ȡUV����
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

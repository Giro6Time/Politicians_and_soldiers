Shader "Custom/shader1"
{
    //Ŀ�꣺дһ���ܹ��������ȵ�
	properties
	{
		_MainTex("MainTex",2d) = ""{}
		_F("F",range(1,10)) = 4
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
			float _F;

			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}
			fixed4 frag(v2f IN) :COLOR
			{
				//�ø��͵���������Unity��պ�Ч��
				//���ڿռ�����⣺������εĲ�ͬ��������������ٵ�Ч��
				float2 uv = IN.uv;
				//ͼ���ƶ�ƫ����*����
				float offset_uv = 0.03 * sin(IN.uv * _F + _Time.y);
				uv += offset_uv;

				fixed4 color_1 = tex2D(_MainTex,uv);
				uv = IN.uv;
				uv -= offset_uv;
				fixed4 color_2 = tex2D(_MainTex, uv);

				return (color_1 + color_2) / 2;
			}
			ENDCG

			}
		}
}

//
//Shader "Custom/sv2"
//{
//	SubShader
//	{
//		Pass
//		{
//			CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment flag
//			float Func2(float arr[2]);
//			struct v2f
//			{
//				float4 pos:POSITION;
//				float2 objPos:TEXCOORD0;
//				fixed4 col : COLOR;
//			};
//			struct appdata_base
//			{
//				float2 pos:POSITION;
//				float4 col:COLOR;
//			};
//			//�������ܣ�TANGENT:���� NORMAL������
//			//��Unity�У������������ɫĬ��Ϊ(1,1,1,1)
//			//Unity�����CGԭ�ļ�λ����D:\unity\2022.3.1f1c1\Editor\Data\CGIncludes��Ҳ��UnityCG�ļ���ȡͷ�ļ�ʱ��Ĭ�϶�ȡ·��
//			//unity�����һЩ�������ܽṹ����unityCG.cginc�ļ���
//			v2f vert(appdata_base v)
//			{
//				v2f o;
//				o.pos = float4(v.pos,0,1);
//				o.objPos = float2(1,0);
//				o.col = v.col;
//				return o;
//			}
//			fixed4 flag(v2f IN) :COLOR
//			{
//
//				return IN.col;
//			}
//
//
//
//			float Func2(float arr[2])
//			{
//				float sum = 0;
//				for (int i = 0; i < arr.Length; i++)
//				{
//					sum += arr[i];
//				}
//				return sum;
//			}
//			ENDCG
//		}
//	}
//}

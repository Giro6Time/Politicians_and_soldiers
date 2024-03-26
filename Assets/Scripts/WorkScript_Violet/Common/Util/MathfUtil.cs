using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathfUtil
{
    /// <summary>
    /// ��ȡ������������پ���
    /// </summary>
    /// <param name="beginPos">��ʼλ��</param>
    /// <param name="endPos">��ֹλ��</param>
    /// <param name="isAbs">�Ƿ�ȡ����ֵ</param>
    /// <returns>�����پ���</returns>
    public static float GetManhattanDistance(Vector3 beginPos,Vector3 endPos,bool isAbs=false)
    {
        float distance = endPos.x + endPos.y + endPos.z - beginPos.x - beginPos.y - beginPos.z;
        if (isAbs)
        {
            distance = Mathf.Abs(distance);
        }
        return distance;
    }

    /// <summary>
    /// ��ȡ�������XY�����پ���
    /// </summary>
    /// <param name="beginPos">��ʼλ��</param>
    /// <param name="endPos">��ֹλ��</param>
    /// <param name="isAbs">�Ƿ�ȡ����ֵ</param>
    /// <returns>�����پ���</returns>
    public static float GetManhattanDistance_XY(Vector3 beginPos, Vector3 endPos, bool isAbs = false)
    {
        float distance = endPos.x + endPos.y- beginPos.x - beginPos.y;
        if (isAbs)
        {
            distance = Mathf.Abs(distance);
        }
        return distance;
    }

    /// <summary>
    /// ��ȡ�������XY���ϵķ���
    /// </summary>
    /// <param name="beginPos">���</param>
    /// <param name="endPos">�յ�</param>
    /// <returns></returns>
    public static Vector3 GetDirection_XY(Vector3 beginPos,Vector3 endPos)
    {
        return new Vector3(endPos.x-beginPos.x,endPos.y-beginPos.y,0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathfUtil
{
    /// <summary>
    /// 获取两个点的曼哈顿距离
    /// </summary>
    /// <param name="beginPos">起始位置</param>
    /// <param name="endPos">终止位置</param>
    /// <param name="isAbs">是否取绝对值</param>
    /// <returns>曼哈顿距离</returns>
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
    /// 获取两个点的XY曼哈顿距离
    /// </summary>
    /// <param name="beginPos">起始位置</param>
    /// <param name="endPos">终止位置</param>
    /// <param name="isAbs">是否取绝对值</param>
    /// <returns>曼哈顿距离</returns>
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
    /// 获取两个点的XY轴上的方向
    /// </summary>
    /// <param name="beginPos">起点</param>
    /// <param name="endPos">终点</param>
    /// <returns></returns>
    public static Vector3 GetDirection_XY(Vector3 beginPos,Vector3 endPos)
    {
        return new Vector3(endPos.x-beginPos.x,endPos.y-beginPos.y,0).normalized;
    }
}

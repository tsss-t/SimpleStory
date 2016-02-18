using UnityEngine;
using System.Collections.Generic;
public enum OutDirection
{
    up = 1, right, down, left
}
public enum AngleFix
{
   Angle0, Angle90, Angle180, Angle270
}
[System.Serializable]
public struct AreaOut
{
    //┏━━━━→x    0,0 0,1 0,2 0,3 0,4
    //┃               1,0 1,1 1,2 1,3 1,4
    //┃               2,0 2,1 2,2 2,3 2,4
    //┃
    //↓y  出口信息
    public Vector2 virtualPosition;
    public OutDirection direction;


    /// <summary>
    /// 图形旋转
    /// </summary>
    /// <param name="virtualSize">图形（场地）虚拟大小</param>
    /// <param name="areaOut">图形（场地）出口本地坐标信息</param>
    /// <param name="angle">需要旋转的角度</param>
    /// <returns></returns>
    public static AreaOut[] ChangeDirection(Vector2 virtualSize, AreaOut[] areaOut, AngleFix angle)
    {
        List<AreaOut> newAreaOutList = new List<AreaOut>();
        for (int i = 0; i < areaOut.Length; i++)
        {
            AreaOut newAreaOut;
            switch (angle)
            {
                case AngleFix.Angle90:
                    newAreaOut.direction = (int)areaOut[i].direction + 1 > 4 ? areaOut[i].direction - 3 : areaOut[i].direction + 1;
                    newAreaOut.virtualPosition = new Vector2(
                        Mathf.Abs(areaOut[i].virtualPosition.y - (virtualSize.y - 1)),
                        areaOut[i].virtualPosition.x
                        );
                    newAreaOutList.Add(newAreaOut);
                    break;
                case AngleFix.Angle180:
                    newAreaOut.direction = (int)areaOut[i].direction + 2 > 4 ? areaOut[i].direction - 2 : areaOut[i].direction + 2;
                    newAreaOut.virtualPosition = new Vector2(
                        Mathf.Abs(areaOut[i].virtualPosition.x - (virtualSize.x - 1)),
                        Mathf.Abs(areaOut[i].virtualPosition.y - (virtualSize.y - 1))
                        );
                    newAreaOutList.Add(newAreaOut);
                    break;
                case AngleFix.Angle270:
                    newAreaOut.direction = (int)areaOut[i].direction + 3 > 4 ? areaOut[i].direction - 1 : areaOut[i].direction + 3;
                    newAreaOut.virtualPosition = new Vector2(
                        areaOut[i].virtualPosition.y,
                        Mathf.Abs(areaOut[i].virtualPosition.x - (virtualSize.x - 1))
                        );
                    newAreaOutList.Add(newAreaOut);
                    break;
                default:
                    break;
            }
        }
        return newAreaOutList.ToArray();
    }
}


public class AreaManager : MonoBehaviour
{

    public int height;
    public int width;
    public int angle;
    public Vector3 position;
    public Vector2 virtualPosition;
    public AreaOut[] areaOut;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

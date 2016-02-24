using UnityEngine;
using System.Collections.Generic;
public enum OutDirection
{
    none = -1, up = 1, right, down, left
}
public enum AngleFix
{
    none = -1, Angle0 = 0, Angle90 = 90, Angle180 = 180, Angle270 = 270
}
[System.Serializable]
public struct AreaOut
{
    public Vector3 position;
    public OutDirection direction;
}
[System.Serializable]
public struct AreaOutGO
{
    public GameObject location;
    public OutDirection direction;
}
[System.Serializable]
public class AreaInfo
{
    
    public GameObject area;
    public int height;
    public int width;
    public Vector3 centerPointUp;
    public Vector3 centerPointDown;
    public Vector3 centerPointLeft;
    public Vector3 centerPointRight;

    public AreaOut[] areaOut;
    public AreaInfo()
    {
       
    }
}
public class AreaManager : MonoBehaviour
{
    public AreaOutGO[] AreaOutGOList;
    public AreaInfo AreaAngle0 ;
    public AreaInfo AreaAngle90 ;
    public AreaInfo AreaAngle180 ;
    public AreaInfo AreaAngle270 ;
    //public int height;
    //public int width;
    //public Vector3 centerPointUp;
    //public Vector3 centerPointDown;
    //public Vector3 centerPointLeft;
    //public Vector3 centerPointRight;
    //public AreaOut[] areaOut;
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public AreaInfo GetAreaInfo(AngleFix angle)
    {
        switch (angle)
        {
            case AngleFix.none:
                return AreaAngle0;
            case AngleFix.Angle0:
                return AreaAngle0;
            case AngleFix.Angle90:
                return AreaAngle90;
            case AngleFix.Angle180:
                return AreaAngle180;
            case AngleFix.Angle270:
                return AreaAngle270;
        }
        return AreaAngle0;
    }
}

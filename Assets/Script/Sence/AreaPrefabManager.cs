using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct AreaPrefabOut
{
    public Vector3 position;
    public OutDirection direction;
}
[System.Serializable]
public struct AreaPrefabOutGO
{
    public GameObject location;
    public OutDirection direction;
}
[System.Serializable]
public class AreaPrefabInfo
{

    public GameObject area;
    public int height;
    public int width;
    public Vector3 centerPointUp;
    public Vector3 centerPointDown;
    public Vector3 centerPointLeft;
    public Vector3 centerPointRight;

    public AreaPrefabOut[] areaOut;
    public AreaPrefabInfo()
    {

    }
}

public class AreaPrefabManager : MonoBehaviour
{
    public UnitType type;
    public WeightPoint basePoint;
    public AreaPrefabOutGO[] AreaOutGOList;
    public AreaPrefabInfo AreaAngle0;
    public AreaPrefabInfo AreaAngle90;
    public AreaPrefabInfo AreaAngle180;
    public AreaPrefabInfo AreaAngle270;



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
    public AreaPrefabInfo GetAreaPrefabInfo(AngleFix angle)
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

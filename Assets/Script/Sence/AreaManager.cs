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
    public GameObject position;
    public OutDirection direction;

    //
}

public class AreaManager : MonoBehaviour
{

    public int height;
    public int width;
    public Vector3 centerPointUp;
    public Vector3 centerPointDown;
    public Vector3 centerPointLeft;
    public Vector3 centerPointRight;


    public Vector3 centerPointUpRoted;
    public Vector3 centerPointDownRoted;
    public Vector3 centerPointLeftRoted;
    public Vector3 centerPointRightRoted;
    public AreaOut[] areaOut;
    public int[] test;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfomation : MonoBehaviour
{
    GameObject areaContainer;
    public int floorNum = -1000;
    public static SceneInfomation _instance;

    void Awake()
    {
        _instance = this;
        if (floorNum == -1000 || floorNum == -100)
        {
            remakeScene();
        }
    }

    // Use this for initialization
    void Start()
    {
        GameController._instance.SetGoingToFloor(floorNum);
    }
    List<AreaData> areaDataList;
    void remakeScene()
    {
        if (floorNum != -100)
        {
            floorNum = GameController._instance.GetGoingToFloor();
        }
        int nextFloorNum = floorNum + 1;


        areaDataList = GameController._instance.GetAreaDataList(nextFloorNum);

        if (areaDataList == null)
        {
            SceneMaker._instance.CreateDataStart(nextFloorNum);
        }
        areaDataList = GameController._instance.GetAreaDataList(floorNum);

        if (areaDataList != null)
        {
            areaContainer = new GameObject("Environment");
            for (int i = 0; i < areaDataList.Count; i++)
            {
                GameObject gameObject = Instantiate(Resources.Load(areaDataList[i].areaName), areaDataList[i].areaPosition, areaDataList[i].areaAngle) as GameObject;
                gameObject.transform.parent = areaContainer.transform;
            }
        }
        else
        {
            if (floorNum != -100)
            {


                SceneMaker._instance.CreateDataStart(floorNum);
                areaDataList = GameController._instance.GetAreaDataList(floorNum);
                areaContainer = new GameObject("Environment");
                for (int i = 0; i < areaDataList.Count; i++)
                {
                    GameObject gameObject = Instantiate(Resources.Load(areaDataList[i].areaName), areaDataList[i].areaPosition, areaDataList[i].areaAngle) as GameObject;
                    gameObject.transform.parent = areaContainer.transform;
                }
            }
        }

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfomation : MonoBehaviour
{
    GameObject areaContainer;
    public SceneFloorInfo floorNumber;
    public static SceneInfomation _instance;

    private int floorNum = -1000;
    public int FloorNumber
    {
        get
        {
            return floorNum;
        }
    }
    void Awake()
    {
        _instance = this;
        floorNum = (int)floorNumber;

        remakeScene();

    }

    // Use this for initialization
    void Start()
    {
        GameController._instance.SetGoingToFloor(floorNum);
    }
    List<AreaData> areaDataList;
    void remakeScene()
    {
        if (floorNumber == SceneFloorInfo.RandomMapFloor)
        {
            floorNum = GameController._instance.GetGoingToFloor();
        }
        int nextFloorNum = floorNum + 1;

        if (nextFloorNum != (int)SceneFloorInfo.Town &&
            nextFloorNum != (int)SceneFloorInfo.FirstFloor &&
            nextFloorNum != (int)SceneFloorInfo.LastFloor &&
            nextFloorNum != (int)SceneFloorInfo.BossFloor &&
            nextFloorNum != (int)SceneFloorInfo.ShopFloor &&
            nextFloorNum < 0
            )
        {
            //次の階段のマップデータを生成
            areaDataList = GameController._instance.GetAreaDataList(nextFloorNum);

            if (areaDataList == null)
            {
                SceneMaker._instance.CreateDataStart(nextFloorNum);
            }


            //現段階のマップ構成
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
            //TEST用！！
            else
            {
                if (floorNum != (int)SceneFloorInfo.Town &&
                floorNum != (int)SceneFloorInfo.FirstFloor &&
                floorNum != (int)SceneFloorInfo.LastFloor &&
                floorNum != (int)SceneFloorInfo.BossFloor &&
                floorNum != (int)SceneFloorInfo.ShopFloor &&
                floorNum < 0
                )
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
}

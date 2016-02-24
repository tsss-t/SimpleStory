using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum UnitType
{
    Up, Down, Portal, Road, Room, End
}
public enum AreaShape
{
    OnePoint, TowStrike, ThreeStrike, FourStrike, ForSquare
}


public class SceneMaker : MonoBehaviour
{
    GameObject areaContainer;
    //bool connect = false;
    public GameObject temp;
    public int mapwidth = 0;//地图大小 mapwidth x mapheigth
    public int mapheigth = 0;
    public GameObject[] UpPrefab;
    public GameObject[] DownPrefab;
    public GameObject[] PortalPrefab;
    public GameObject[] RoadPrefab;
    public GameObject[] RoomPrefab;
    public GameObject[] EndPrefab;
    public GameObject[] cornerPrefab;
    #region Editor
    public GameObject[] normalPrefab;
    #endregion
    //#region onePoint
    //public GameObject[] OnePointEndPrefab;
    //public GameObject[] OnePointRoadPrefab;
    //public GameObject[] OnePointCornerPrefab;
    //public GameObject[] OnePointUpPrefab;
    //public GameObject[] OnePointDownPrefab;
    //public GameObject[] OnePointPortalPrefab;
    //#endregion
    //#region twoPoint
    //public GameObject[] TwoPointRoadPrefab;
    //public GameObject[] TwoPointUpPrefab;
    //public GameObject[] TwoPointDownPrefab;
    //public GameObject[] TwoPointPortalPrefab;
    //#endregion
    //#region threePoint
    //public GameObject[] ThreePointRoadPrefab;
    //public GameObject[] ThreePointUpPrefab;
    //public GameObject[] ThreePointDownPrefab;
    //public GameObject[] ThreePointPortalPrefab;
    //#endregion
    //#region fourPoint
    //public GameObject[] FourPointDownPrefab;
    //#endregion
    Dictionary<UnitType, GameObject[]> areaPrefabDictionary;

    Dictionary<Vector2, GameObject> mapDictionary;

    #region 初期化
    // Use this for initialization
    void Start()
    {
        //Debug.Log("start!");
        mapDictionary = new Dictionary<Vector2, GameObject>();
        InitDictionary();
    }
    void InitDictionary()
    {
        areaPrefabDictionary = new Dictionary<UnitType, GameObject[]>();
        areaPrefabDictionary.Add(UnitType.Up, UpPrefab);
        areaPrefabDictionary.Add(UnitType.Down, DownPrefab);
        areaPrefabDictionary.Add(UnitType.Portal, PortalPrefab);
        areaPrefabDictionary.Add(UnitType.Road, RoadPrefab);
        areaPrefabDictionary.Add(UnitType.Room, RoomPrefab);
        areaPrefabDictionary.Add(UnitType.End, EndPrefab);
    }
    #endregion
    #region Random Method
    /// <summary>
    /// 取地图上随机点
    /// </summary>
    /// <returns>生成的随机点坐标</returns>
    Vector2 RandomMapPoiot()
    {
        int x = Random.Range(0, mapwidth);
        int y = Random.Range(0, mapheigth);
        return new Vector2(x, y);
    }
    /// <summary>
    /// 取0到indexMax的随机值
    /// </summary>
    /// <param name="indexMax">随机值上限</param>
    /// <returns>生成的随机值</returns>
    int RandomIndex(int indexMax)
    {
        int x = Random.Range(0, indexMax);
        return x;
    }
    /// <summary>
    /// 从ROAD,ROOM,POTAL,END等场景中随机抽选场景
    /// </summary>
    /// <returns>随机普通场景</returns>
    GameObject RandomNormalArea()
    {
        return normalPrefab[Random.Range(0, normalPrefab.Length)];
    }
    /// <summary>
    /// 在规定范围内产生随机方向
    /// </summary>
    /// <param name="position">生成的位置</param>
    /// <param name="areaInfo">需要生成的区域信息</param>
    /// <returns>方向的Int值（）</returns>
    AngleFix RandomMapRotation(Vector2 position, AreaManager areaInfo)
    {
        List<AngleFix> canSetDirection = new List<AngleFix>();

        if (checkSize(position, areaInfo.AreaAngle0.width, areaInfo.AreaAngle0.height))
        {
            canSetDirection.Add(AngleFix.Angle0);
            canSetDirection.Add(AngleFix.Angle180);
        }
        if (checkSize(position, areaInfo.AreaAngle90.width, areaInfo.AreaAngle90.height))
        {
            canSetDirection.Add(AngleFix.Angle90);
            canSetDirection.Add(AngleFix.Angle270);
        }
        return canSetDirection.Count == 0 ? AngleFix.none : canSetDirection[RandomIndex(canSetDirection.Count)];
        //AreaOut[] tempAreaOut;1

        //tempAreaOut = areaOut;
        //List<AngleFix> canSetDirection = new List<AngleFix>();

        //if (checkRotation(position, virtualSize, tempAreaOut))
        //{
        //    canSetDirection.Add(AngleFix.Angle0);
        //}
        //tempAreaOut = AreaOut.ChangeDirection(virtualSize, areaOut, AngleFix.Angle90);
        //if (checkRotation(position, virtualSize, tempAreaOut))
        //{
        //    canSetDirection.Add(AngleFix.Angle90);
        //}
        //tempAreaOut = AreaOut.ChangeDirection(virtualSize, areaOut, AngleFix.Angle180);
        //if (checkRotation(position, virtualSize, tempAreaOut))
        //{
        //    canSetDirection.Add(AngleFix.Angle180);
        //}
        //tempAreaOut = AreaOut.ChangeDirection(virtualSize, areaOut, AngleFix.Angle270);
        //if (checkRotation(position, virtualSize, tempAreaOut))
        //{
        //    canSetDirection.Add(AngleFix.Angle270);
        //}
        //return canSetDirection.Count == 0 ? -1 : (int)canSetDirection[Random.Range(0, canSetDirection.Count - 1)];
    }

    /// <summary>
    /// 打乱数组(用于随机完全遍历)
    /// </summary>
    /// <param name="areaOutList"></param>
    /// <returns></returns>
    AreaOut[] RandomAreaOut(AreaOut[] areaOutList)
    {
        AreaOut[] tempAreaOutList = new AreaOut[areaOutList.Length];
        int x;
        for (int i = 0; i < tempAreaOutList.Length; i++)
        {
            x = Random.Range(0, tempAreaOutList.Length);
            tempAreaOutList[i] = areaOutList[x];
            tempAreaOutList[x] = areaOutList[i];
        }
        return tempAreaOutList;
    }

    #endregion
    #region makeScene
    public void MakeUpPoint()
    {
        Vector2 upPoint;
        GameObject area;
        AreaManager areaManager;
        AngleFix angle;
        do
        {
            upPoint = RandomMapPoiot();
            area = UpPrefab[RandomIndex(UpPrefab.Length)];
            areaManager = area.GetComponent<AreaManager>();
            angle = RandomMapRotation(upPoint, areaManager);
        }
        while (angle == AngleFix.none);


        CreateArea(area, upPoint, angle);

        for (int i = 0; i < areaManager.AreaOutGOList.Length; i++)
        {

            MakeNormalArea(areaManager.GetAreaInfo(angle).areaOut[i].position + new Vector3(upPoint.x, 0, upPoint.y), areaManager.GetAreaInfo(angle).areaOut[i].direction.TureBack());
        }
    }


    /// <summary>
    /// 生成下一个地区
    /// </summary>
    /// <param name="entryPoint">生成的入口点</param>
    /// <param name="needDirection">入口点的方向</param>
    void MakeNormalArea(Vector3 entryPoint, OutDirection needDirection)
    {
        GameObject areaPrefab;
        //GameObject changedPrefab;
        AreaManager areaManager;
        AreaOut[] areaOutList;
        Vector3 areaPostion = Vector3.zero;

        AngleFix angle = AngleFix.none;

        bool checkRes = false;
        do
        {
            areaPrefab = RandomNormalArea();

            areaManager = areaPrefab.GetComponent<AreaManager>();
            areaOutList = RandomAreaOut(areaManager.AreaAngle0.areaOut);
            for (int i = 0; i < areaOutList.Length; i++)
            {

                angle = areaOutList[i].direction.getAngleFromTargetDirection(needDirection);

                areaPostion = entryPoint - areaOutList[i].Rot(angle).position;

                if (angle == AngleFix.Angle90 || angle == AngleFix.Angle270)
                {
                    checkRes = CheckArea(areaPostion, areaManager, angle);
                }
                else
                {
                    checkRes = CheckArea(areaPostion, areaManager, angle);
                }
                if (checkRes)
                {
                    break;
                }
                //changedPrefab = areaPrefab.Rot(entryPoint, angle);
            }
        } while (checkRes);

        CreateArea(areaPrefab, new Vector2(areaPostion.x, areaPostion.z), angle);

        //TODO:生成方法
    }
    void CreateArea(GameObject go, Vector2 position, AngleFix angle)
    {
        if (areaContainer == null)
        {
            areaContainer = new GameObject("Environment");
        }
        GameObject gameObject = PrefabUtility.InstantiatePrefab(go) as GameObject;
        gameObject.transform.parent = areaContainer.transform;
        gameObject.transform.position = new Vector3(position.x, 0, position.y);
        gameObject.transform.rotation = Quaternion.Euler(0, (int)angle, 0);
    }
    #endregion
    #region check
    /// <summary>
    /// 检验是否生成的区域在地图规定大小区域范围内
    /// </summary>
    /// <param name="position">生成点坐标</param>
    /// <param name="width">区域的长度</param>
    /// <param name="height">区域的宽度</param>
    /// <returns>是否超出地图设置范围： true=通过检测，不超出</returns>
    bool checkSize(Vector2 position, int width, int height)
    {
        if (position.x + width * 0.5 > mapwidth ||
            position.x - width * 0.5 < 0 ||
            position.y + height * 0.5 > mapheigth ||
            position.y - height * 0.5 < 0)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 用投影的方式确认该地区是否与其他地区重合
    /// </summary>
    /// <param name="position">区域地点</param>
    /// <param name="areaManager">区域信息</param>
    /// <param name="roted">是否旋转</param>
    /// <returns></returns>
    bool CheckArea(Vector3 position, AreaManager areaManager, AngleFix angle)
    {
        AreaInfo info = areaManager.GetAreaInfo(angle);
        //left to right
        return 
            !Physics.CapsuleCast(
          position + new Vector3(0, 100, info.centerPointLeft.z + 2),
          position + new Vector3(0, 100, info.centerPointRight.z - 2),
            info.centerPointUp.magnitude / 2,
            Vector3.down)

            &&

            !Physics.CapsuleCast(
          position + new Vector3(info.centerPointUp.x + 2, 100, 0),
          position + new Vector3(info.centerPointDown.x - 2, 100, 0),
            info.centerPointUp.magnitude / 2,
            Vector3.down)

            &&

            checkSize(position, info.width, info.height);

    }
    //bool checkRotation(Vector2 position, Vector2 virtualSize, AreaOut[] areaOut)
    //{

    //    if (position.x + virtualSize.x - 1 >= length || position.y + virtualSize.y - 1 >= length)
    //    {
    //        return false;
    //    }

    //    GameObject tempInfo;
    //    Vector2 closeAreaPosition;
    //    OutDirection needDirection;

    //    for (int i = 0; i < areaOut.Length; i++)
    //    {
    //        switch (areaOut[i].direction)
    //        {
    //            case OutDirection.up:
    //                {
    //                    //与该路口的相邻点虚拟（地图）世界坐标
    //                    closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x, position.y + areaOut[i].virtualPosition.y - 1);
    //                    needDirection = OutDirection.down;
    //                    break;
    //                }
    //            case OutDirection.down:
    //                {
    //                    //与该路口的相邻点虚拟（地图）世界坐标
    //                    closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x, position.y + areaOut[i].virtualPosition.y + 1);
    //                    needDirection = OutDirection.up;
    //                    break;
    //                }
    //            case OutDirection.left:
    //                {
    //                    closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x - 1, position.y + areaOut[i].virtualPosition.y);
    //                    needDirection = OutDirection.right;
    //                    break;
    //                }
    //            case OutDirection.right:
    //                {
    //                    closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x - 1, position.y + areaOut[i].virtualPosition.y);
    //                    needDirection = OutDirection.left;
    //                    break;
    //                }
    //            default:
    //                closeAreaPosition = new Vector2(-1, -1);
    //                needDirection = OutDirection.up;
    //                break;
    //        }
    //        mapDictionary.TryGetValue(closeAreaPosition, out tempInfo);

    //        //如果该相邻点安置了任何区域，并且该区域该点的路口设置不能通过连接验证（该点没有路口设置或者路口方向不正确）
    //        if (tempInfo != null && !checkRoadCross(closeAreaPosition, tempInfo.GetComponent<AreaManager>(), needDirection))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    ///// <summary>
    ///// 验证地图上一个点是否为地图可以接通的方向
    ///// </summary>
    ///// <param name="worldVirtualPosition">验证点的虚拟（地图）世界坐标</param>
    ///// <param name="targetArea">对象区域的信息对象</param>
    ///// <param name="targetNeedDirection">需求该相邻区域的方向</param>
    ///// <returns></returns>
    //bool checkRoadCross(Vector2 worldVirtualPosition, AreaManager targetArea, OutDirection targetNeedDirection)
    //{
    //    Vector2 localAreaPosition = worldVirtualPosition - targetArea.centerPosition;
    //    for (int i = 0; i < targetArea.areaOut.Length; i++)
    //    {
    //        //路口位置坐标找到并且该位置上于所需求的方向一致，则通过验证
    //        if (localAreaPosition == targetArea.areaOut[i].virtualPosition && targetArea.areaOut[i].direction == targetNeedDirection)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    #endregion


    //void MakeDownPoint()
    //{

    //}
    //void MakeMap()
    //{

    //}
    //// Update is called once per frame
    //void Update()
    //{

    //}



}

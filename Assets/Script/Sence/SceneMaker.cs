using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


#region data
[System.Serializable]
public struct sceneData
{
    string areaPrefabName;//使用的prefab名
    Vector3 position;//区域中心点坐标
    int height;//区域大小
    int weidth;//区域大小
}
[System.Serializable]
public struct entryData
{
    Vector3 position;//地点坐标
    OutDirection direction;//方向
    bool isCrossed;//是否连通
}
#endregion
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
    public GameObject[] CornerPrefab;
    public GameObject[] WallPrefab;

    #region Editor
    public sceneData[] senceDataList;
    public entryData[] entryDataList;
    public WeightPoint nowWeightPoint;
    public UnitType makingAreaType;
    public int makeingCombo;
    public UnitType[] normalTypeArray = { UnitType.Road, UnitType.Room, UnitType.Corner, UnitType.End };
    #endregion


    #region 初期化
    // Use this for initialization
    void Start()
    {
        //Debug.Log("start!");
        //mapDictionary = new Dictionary<Vector2, GameObject>();
        InitDictionary();
    }
    void InitDictionary()
    {
        //areaPrefabDictionary = new Dictionary<UnitType, GameObject[]>();
        //areaPrefabDictionary.Add(UnitType.Up, UpPrefab);
        //areaPrefabDictionary.Add(UnitType.Down, DownPrefab);
        //areaPrefabDictionary.Add(UnitType.Portal, PortalPrefab);
        //areaPrefabDictionary.Add(UnitType.Road, RoadPrefab);
        //areaPrefabDictionary.Add(UnitType.Room, RoomPrefab);
        //areaPrefabDictionary.Add(UnitType.End, EndPrefab);
    }
    #endregion
    #region API
    GameObject[] getPrefabArray(UnitType type)
    {
        switch (type)
        {
            case UnitType.Up:
                return UpPrefab;
            case UnitType.Down:
                return DownPrefab;
            case UnitType.Portal:
                return PortalPrefab;
            case UnitType.Road:
                return RoadPrefab;
            case UnitType.Room:
                return RoomPrefab;
            case UnitType.End:
                return EndPrefab;
            case UnitType.Corner:
                return CornerPrefab;
            case UnitType.Wall:
                return WallPrefab;
            default:
                return null;
        }
    }
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
    /// <returns>返回是否通过检测 TRUE=通过</returns>
    bool CheckArea(Vector3 position, AreaManager areaManager, AngleFix angle)
    {
        AreaInfo info = areaManager.GetAreaInfo(angle);
        bool physicsCheck = false;
        //left to right
        if (info.width > info.height)
        {
            //宽大于长，从左到右
            physicsCheck = Physics.CapsuleCast(
          position + new Vector3(0, 100, info.centerPointLeft.z + 1 + info.centerPointUp.magnitude > 0 ? 0 : info.centerPointLeft.z + 1 + info.centerPointUp.magnitude),
          position + new Vector3(0, 100, info.centerPointRight.z - 1 - info.centerPointUp.magnitude < 0 ? 0 : info.centerPointRight.z - 1 - info.centerPointUp.magnitude),
            info.centerPointUp.magnitude - 1f,
            Vector3.down);
        }
        else
        {
            //长大于宽，从上到下
            physicsCheck = Physics.CapsuleCast(
          position + new Vector3(info.centerPointUp.x + 1 + info.centerPointLeft.magnitude > 0 ? 0 : info.centerPointUp.x + 1 + info.centerPointLeft.magnitude, 100, 0),
          position + new Vector3(info.centerPointDown.x - 1 - info.centerPointLeft.magnitude < 0 ? 0 : info.centerPointDown.x - 1 - info.centerPointLeft.magnitude, 100, 0),
            info.centerPointLeft.magnitude - 1f,
            Vector3.down);
        }

        bool sizeCheck = checkSize(new Vector2(position.x, position.z), info.width, info.height);
        //Debug.Log(physicsCheck);
        return
            !physicsCheck


            &&

            sizeCheck;
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
    #endregion
    #region makeScene follow
    public void CreateStart()
    {
        MakeUpPoint();
    }

    public void MakeUpPoint()
    {
        makingAreaType = UnitType.Wall;
        Random.seed = System.DateTime.Now.Millisecond;
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


        TrimWeightPoint(areaManager, 0);
        CreateArea(area, upPoint, angle);
        makingAreaType = UnitType.Up;
        for (int i = 0; i < areaManager.AreaOutGOList.Length; i++)
        {
            MakeNormalArea(areaManager.GetAreaInfo(angle).areaOut[i].position + new Vector3(upPoint.x, 0, upPoint.y), areaManager.GetAreaInfo(angle).areaOut[i].direction.TureBack(), 0);
            TrimWeightPoint(areaManager, 0);
        }
    }

    /// <summary>
    /// 生成下一个地区
    /// </summary>
    /// <param name="entryPoint">生成的入口点</param>
    /// <param name="needDirection">入口点的方向</param>
    void MakeNormalArea(Vector3 entryPoint, OutDirection needDirection, int upFloorNumber)
    {
        int thisFloorNumber = upFloorNumber + 1;
        GameObject areaPrefab = null;
        GameObject[] randomNormalPrefabList = null;


        //GameObject changedPrefab;
        AreaManager areaManager = null;
        AreaOut[] areaOutList = null;
        Vector3 areaPostion = Vector3.zero;
        int entryIndex = 0;
        int areaTypeIndex = 0;
        AngleFix angle = AngleFix.none;

        bool checkRes = false;
        //Debug.Log("----------------------------------------------------------------");
        //Debug.Log(nowWeightPoint.roadPoint);
        //Debug.Log(nowWeightPoint.roomPoint);
        //Debug.Log(nowWeightPoint.cornerPoint);
        //Debug.Log(nowWeightPoint.endPoint);

        normalTypeArray = nowWeightPoint.GetWeightRandomTypeOrder(normalTypeArray);


        for (areaTypeIndex = 0; areaTypeIndex < normalTypeArray.Length && !checkRes; areaTypeIndex++)
        {
            if (areaTypeIndex != 0)
            {
                Debug.Log("lenth:"+normalTypeArray.Length);
                Debug.Log(normalTypeArray[areaTypeIndex]);
            }
            switch (normalTypeArray[areaTypeIndex])
            {
                case UnitType.Road:
                    {
                        randomNormalPrefabList = RoadPrefab.getRandomArray();
                        break;
                    }
                case UnitType.Room:
                    {
                        randomNormalPrefabList = RoomPrefab.getRandomArray();
                        break;
                    }
                case UnitType.End:
                    {
                        randomNormalPrefabList = EndPrefab.getRandomArray();
                        break;
                    }
                case UnitType.Corner:
                    {
                        randomNormalPrefabList = CornerPrefab.getRandomArray();
                        break;
                    }
                case UnitType.Wall:
                    {
                        randomNormalPrefabList = null;
                        CreateWall(entryPoint, needDirection);
                        return;
                    }
            }
            for (int i = 0; i < randomNormalPrefabList.Length && !checkRes; i++)
            {
                areaPrefab = randomNormalPrefabList[i];
                //Debug.Log(areaPrefab.name);
                areaManager = areaPrefab.GetComponent<AreaManager>();
                areaOutList = areaManager.AreaAngle0.areaOut.getRandomArray();
                for (entryIndex = 0; entryIndex < areaOutList.Length; entryIndex++)
                {
                    angle = areaOutList[entryIndex].direction.getAngleFromTargetDirection(needDirection);
                    areaPostion = entryPoint - areaOutList[entryIndex].Rot(angle).position;

                    if (angle == AngleFix.Angle90 || angle == AngleFix.Angle270)
                    {
                        checkRes = CheckArea(areaPostion, areaManager, angle);
                    }
                    else
                    {
                        checkRes = CheckArea(areaPostion, areaManager, angle);
                    }
                    //Debug.Log(checkRes);
                    if (checkRes)
                    {
                        break;
                    }
                    //changedPrefab = areaPrefab.Rot(entryPoint, angle);
                }
                //Debug.Log(checkRes);
            }
            if (checkRes)
            {
                break;
            }
        }

        //Debug.Log(checkRes);
        //所有地区生成不能的情况下，封堵
        if (!checkRes)
        {
            //Debug.Log("OUT!");
            CreateWall(entryPoint, needDirection);
            return;
        }


        //调整权重
        TrimWeightPoint(areaManager, thisFloorNumber);

        //生成区域
        CreateArea(areaPrefab, new Vector2(areaPostion.x, areaPostion.z), angle);
        makingAreaType = normalTypeArray[areaTypeIndex];


        for (int i = 0; i < areaOutList.Length; i++)
        {
            if (i != entryIndex)
            {
                MakeNormalArea(areaOutList[i].Rot(angle).position + areaPostion, areaOutList[i].Rot(angle).direction.TureBack(), thisFloorNumber);
                TrimWeightPoint(areaManager, thisFloorNumber);
            }
        }
        //TODO:生成方法
    }
    /// <summary>
    /// 调整权重算法
    /// </summary>
    /// <param name="type">正在构造的地区的信息</param>
    void TrimWeightPoint(AreaManager makingAreaPrefabInfo, int thisFloorNumber)
    {
        makeingCombo = makingAreaPrefabInfo.type != makingAreaType ? 0 : makeingCombo + 1;
        if (makeingCombo > 0)
        {
            if (makingAreaPrefabInfo.type == UnitType.Road)
            {
                nowWeightPoint.CutWeight(UnitType.Road, 2);
            }
            else if (makingAreaPrefabInfo.type == UnitType.Corner)
            {
                nowWeightPoint.CutWeight(UnitType.Corner, 5);
            }
        }
        else
        {

            nowWeightPoint.cornerPoint = makingAreaPrefabInfo.basePoint.cornerPoint;
            nowWeightPoint.roadPoint = makingAreaPrefabInfo.basePoint.roadPoint;
            nowWeightPoint.roomPoint = makingAreaPrefabInfo.basePoint.roomPoint;
            nowWeightPoint.endPoint = makingAreaPrefabInfo.basePoint.endPoint;

            nowWeightPoint.AddWeight(UnitType.Corner, (10 - thisFloorNumber) >= 0 ? (10 - thisFloorNumber) * 2 : 0);
            nowWeightPoint.CutWeight(UnitType.Road, (10 - thisFloorNumber) >= 0 ? (10 - thisFloorNumber) : 0);
            nowWeightPoint.CutWeight(UnitType.Room, (10 - thisFloorNumber) >= 0 ? (10 - thisFloorNumber) : 0);
            nowWeightPoint.CutWeight(UnitType.End, (10 - thisFloorNumber) >= 0 ? (10 - thisFloorNumber) * 2 : 0);

        }
    }
    void CreateWall(Vector3 postion, OutDirection needDirection)
    {
        makingAreaType = UnitType.Wall;
        if (makingAreaType == UnitType.Road)
        {
            CreateArea(WallPrefab[0], new Vector2(postion.x, postion.z), WallPrefab[0].GetComponent<AreaManager>().AreaAngle0.areaOut[0].direction.getAngleFromTargetDirection(needDirection));
        }
        else
        {
            CreateArea(WallPrefab[1], new Vector2(postion.x, postion.z), WallPrefab[1].GetComponent<AreaManager>().AreaAngle0.areaOut[0].direction.getAngleFromTargetDirection(needDirection));
        }
        //todo
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

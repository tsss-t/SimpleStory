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

    public bool isDownSet = false;
    public bool isPorSet = false;

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
    Vector2 RandomMapPoint()
    {
        int x = Random.Range(0, mapwidth);
        int y = Random.Range(0, mapheigth);
        return new Vector2(x, y);
    }

    Vector2 RandomMapInnerPoint()
    {
        int x = Random.Range((int)(mapwidth * 0.25f), (int)(mapwidth - mapwidth * 0.25f));
        int y = Random.Range((int)(mapheigth * 0.25f), (int)(mapheigth - mapheigth * 0.25f));
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
    bool CheckAreaPhysics(Vector3 position, AreaManager areaManager, AngleFix angle)
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

        //Debug.Log(physicsCheck);
        return !physicsCheck;
    }
    /// <summary>
    /// 验证该地区是否超过地图尺寸
    /// </summary>
    /// <param name="position">区域地点</param>
    /// <param name="areaManager">区域信息</param>
    /// <param name="roted">是否旋转</param>
    /// <returns>返回是否通过检测 TRUE=通过</returns>
    bool checkInMap(Vector3 position, AreaManager areaManager, AngleFix angle)
    {
        AreaInfo info = areaManager.GetAreaInfo(angle);
        bool sizeCheck = checkSize(new Vector2(position.x, position.z), info.width, info.height);
        return sizeCheck;
    }

    #endregion
    #endregion
    #region makeScene follow
    public void CreateStart()
    {
        MakeUpPoint();
    }

    public void MakeUpPoint()
    {
        isDownSet = false;
        isPorSet = false;
        makingAreaType = UnitType.Wall;
        Random.seed = System.DateTime.Now.Millisecond;
        Vector2 upPoint;
        GameObject area;
        AreaManager areaManager;
        AngleFix angle;
        do
        {
            upPoint = RandomMapInnerPoint();
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
        normalTypeArray = nowWeightPoint.GetWeightRandomTypeOrder(normalTypeArray);


        for (areaTypeIndex = 0; areaTypeIndex < normalTypeArray.Length && !checkRes; areaTypeIndex++)
        {
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
                        Debug.Log("!!" + isDownSet);
                        //每当生成终节点时，如果尚未生成向下场景，则生成向下场景/传送阵场景
                        if (!isDownSet)
                        {
                            randomNormalPrefabList = DownPrefab.getRandomArray();
                        }
                        else if (!isPorSet)
                        {
                            randomNormalPrefabList = PortalPrefab.getRandomArray();
                        }
                        else
                        {
                            randomNormalPrefabList = EndPrefab.getRandomArray();
                        }
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


                    checkRes = checkInMap(areaPostion, areaManager, angle);
                    #region 超出地图范围的情况下，允许生成传送阵和出口
                    if (!checkRes && (!isDownSet || !isPorSet))
                    {
                        if (!isDownSet)
                        {
                            areaPrefab = DownPrefab.getRandomOne();
                            normalTypeArray[areaTypeIndex] = UnitType.Down;
                        }
                        else
                        {
                            areaPrefab = PortalPrefab.getRandomOne();
                            normalTypeArray[areaTypeIndex] = UnitType.Portal;
                        }
                        areaManager = areaPrefab.GetComponent<AreaManager>();
                        areaOutList = areaManager.AreaAngle0.areaOut.getRandomArray();

                        angle = areaOutList[0].direction.getAngleFromTargetDirection(needDirection);
                        areaPostion = entryPoint - areaOutList[0].Rot(angle).position;
                    }
                    #endregion
                    checkRes = checkRes && CheckAreaPhysics(areaPostion, areaManager, angle);

                    if (checkRes)
                    {
                        if(areaManager.type == UnitType.Portal)
                        {
                            isPorSet = true;
                        }
                        else if (areaManager.type == UnitType.Down)
                        {
                            isDownSet = true;
                                
                        }
                        break;
                    }
                }
            }
            if (checkRes)
            {
                break;
            }
        }

        //所有地区生成不能的情况下，封堵
        if (!checkRes)
        {
            CreateWall(entryPoint, needDirection);
            return;
        }

        //调整权重
        TrimWeightPoint(areaManager, thisFloorNumber);

        //生成区域
        makingAreaType = normalTypeArray[areaTypeIndex];
        CreateArea(areaPrefab, new Vector2(areaPostion.x, areaPostion.z), angle);

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
            nowWeightPoint.CutWeight(UnitType.End, (10 - thisFloorNumber) >= 0 ? (10 - thisFloorNumber) * 3 : 0);

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



}

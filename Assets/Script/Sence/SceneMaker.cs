using UnityEngine;
using System.Collections.Generic;

public class SceneMaker : MonoBehaviour
{
    #region para
    enum MakeMode
    {
        MakeSceneMode, MakeDataMode
    }

    MakeMode mode;

    public static SceneMaker _instance;

    GameObject areaContainer;
    List<AreaData> areaDataList;
    List<EnemyPositionData> enemyDataList;
    //bool connect = false;
    public GameObject temp;
    public bool isDownSet = false;
    public bool isPorSet = false;

    private int floorNum;
    private WeightPoint nowWeightPoint;
    private UnitType makingAreaType;
    private int makeingCombo;
    private UnitType[] normalTypeArray = { UnitType.Road, UnitType.Room, UnitType.Corner, UnitType.End };
    #region Editor
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

    #endregion
    #endregion

    #region 初期化
    void Awake()
    {
        _instance = this;
    }
    #endregion
    #region 内部方法
    /// <summary>
    /// タイプから、対応のアリアPrefabを貰う
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
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
    ///　マップ上でランダム座標　取地图上随机点
    /// </summary>
    /// <returns>生成的随机点坐标</returns>
    Vector2 RandomMapPoint()
    {
        int x = Random.Range(0, mapwidth);
        int y = Random.Range(0, mapheigth);
        return new Vector2(x, y);
    }
    /// <summary>
    ///　マップ内部（1/2のwidth/heigth）上でランダム座標　取地图上随机点
    /// </summary>
    /// <returns>生成的随机点坐标</returns>
    Vector2 RandomMapInnerPoint()
    {
        int x = Random.Range((int)(mapwidth * 0.25f), (int)(mapwidth - mapwidth * 0.25f));
        int y = Random.Range((int)(mapheigth * 0.25f), (int)(mapheigth - mapheigth * 0.25f));
        return new Vector2(x, y);
    }

    /// <summary>
    /// ０からindexMaxまでランダムで一つを返す　取0到indexMax的随机值
    /// </summary>
    /// <param name="indexMax">ランダム範囲の上限　随机值上限</param>
    /// <returns>ランダム数値を返す　生成的随机值</returns>
    int RandomIndex(int indexMax)
    {
        int x = Random.Range(0, indexMax);
        return x;
    }
    /// <summary>
    /// ランダムで可能な角度の中に一つの角度を貰う　在规定范围内产生随机方向
    /// </summary>
    /// <param name="position">エリアの中心座標　生成的位置</param>
    /// <param name="areaInfo">エリアの情報クラス　需要生成的区域信息</param>
    /// <returns>方向的Int值（）</returns>
    AngleFix RandomMapRotation(Vector2 position, AreaPrefabManager areaInfo)
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
    #region check　エリア生成検証
    /// <summary>
    /// 検証：このエリアの生成場所は指定されたマップサイズに超えたか　验证该地区是否超过地图尺寸
    /// </summary>
    /// <param name="position">エリアの中心座標　区域地点</param>
    /// <param name="width">エリアのwidth　区域的宽度</param>
    /// <param name="height">エリアのheight　区域的长度</param>
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
    /// 検証：CapsuleCastを使用し、他のエリアと被るかどうか　　　　用投影的方式确认该地区是否与其他地区重合
    /// </summary>
    /// <param name="position">エリアの中心座標　区域地点</param>
    /// <param name="areaManager">エリアの情報クラス　区域信息</param>
    /// <param name="angle">エリア回る角度　是否旋转</param>
    /// <returns>この検証を通過できるかどうか　TRUE=通過　返回是否通过检测 TRUE=通过</returns>
    bool CheckAreaPhysics(Vector3 position, AreaPrefabManager areaManager, AngleFix angle)
    {
        AreaPrefabInfo info = areaManager.GetAreaPrefabInfo(angle);
        bool physicsCheck = false;
        //left to right
        if (info.width > info.height)
        {
            //width>height:左から右までのCapsuleCast検証　宽大于长，从左到右
            physicsCheck = Physics.CapsuleCast(
          position + new Vector3(0, 100, info.centerPointLeft.z + 1 + info.centerPointUp.magnitude > 0 ? 0 : info.centerPointLeft.z + 1 + info.centerPointUp.magnitude),
          position + new Vector3(0, 100, info.centerPointRight.z - 1 - info.centerPointUp.magnitude < 0 ? 0 : info.centerPointRight.z - 1 - info.centerPointUp.magnitude),
            info.centerPointUp.magnitude - 1f,
            Vector3.down);
        }
        else
        {
            //height>width:上から下までのCapsuleCast検証　长大于宽，从上到下
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
    /// Simple AABB check
    /// </summary>
    /// <param name="position">エリアの中心座標　区域地点</param>
    /// <param name="areaManager">エリアの情報クラス　区域信息</param>
    /// <param name="angle">エリア回る角度　是否旋转</param>
    /// <returns>この検証を通過できるかどうか　TRUE=通過　返回是否通过检测 TRUE=通过</returns>
    bool CheckAreaOverlay(Vector3 position, AreaPrefabManager areaManager, AngleFix angle)
    {
        AreaPrefabInfo info = areaManager.GetAreaPrefabInfo(angle);
        bool OverlayCheck = true;

        Vector3 relativePosition;

        for (int i = 0; i < areaDataList.Count; i++)
        {
            relativePosition = areaDataList[i].areaPosition - position;
            if (Mathf.Abs(relativePosition.x) < areaDataList[i].heightHalf + info.centerPointUp.magnitude &&
                Mathf.Abs(relativePosition.z) < areaDataList[i].widthHalf + info.centerPointLeft.magnitude
                )
            {
                OverlayCheck = false;
                break;
            }
        }
        return OverlayCheck;
    }

    /// <summary>
    /// 検証：このエリアの生成場所は指定されたマップサイズに超えたか　验证该地区是否超过地图尺寸
    /// </summary>
    /// <param name="position">エリアの中心座標　区域地点</param>
    /// <param name="areaManager">エリアの情報クラス　区域信息</param>
    /// <param name="angle">エリア回る角度　是否旋转</param>
    /// <returns>この検証を通過できるかどうか　TRUE=通過　返回是否通过检测 TRUE=通过</returns>
    bool checkInMap(Vector3 position, AreaPrefabManager areaManager, AngleFix angle)
    {
        AreaPrefabInfo info = areaManager.GetAreaPrefabInfo(angle);
        bool sizeCheck = checkSize(new Vector2(position.x, position.z), info.width, info.height);

        return sizeCheck;
    }

    #endregion
    #endregion
    #region makeScene follow
    public void CreateStart()
    {
        nowWeightPoint = new WeightPoint();
        mode = MakeMode.MakeSceneMode;
        MakeUpPoint();
    }
    public void CreateDataStart(int floorNum)
    {
        this.floorNum = floorNum;
        nowWeightPoint = new WeightPoint();
        mode = MakeMode.MakeDataMode;
        areaDataList = new List<AreaData>();
        enemyDataList = new List<EnemyPositionData>();
        MakeUpPoint();
    }
    /// <summary>
    /// 上に行く階段から生成始めます
    /// </summary>
    public void MakeUpPoint()
    {
        isDownSet = false;
        isPorSet = false;
        makingAreaType = UnitType.Wall;
        Vector2 upPoint;
        GameObject areaPrefab;
        AreaPrefabManager areaManager;
        AngleFix angle;


        for (; isPorSet == false;)
        {


            Random.seed = System.DateTime.Now.Millisecond;
            if (mode == MakeMode.MakeDataMode)
            {
                areaDataList.Clear();
                enemyDataList.Clear();
            }
            else
            {
                DestroyImmediate(areaContainer);
            }
            do
            {
                upPoint = RandomMapInnerPoint();
                areaPrefab = UpPrefab[RandomIndex(UpPrefab.Length)];
                areaManager = areaPrefab.GetComponent<AreaPrefabManager>();
                angle = RandomMapRotation(upPoint, areaManager);
            }
            while (angle == AngleFix.none);

            //Debug.Log(upPoint);


            TrimWeightPoint(areaManager, 0);
            CreateArea(areaPrefab, areaManager, upPoint, angle);
            makingAreaType = UnitType.Up;
            for (int i = 0; i < areaManager.AreaOutGOList.Length; i++)
            {
                MakeNormalArea(areaManager.GetAreaPrefabInfo(angle).areaOut[i].position + new Vector3(upPoint.x, 0, upPoint.y), areaManager.GetAreaPrefabInfo(angle).areaOut[i].direction.TureBack(), 0);
                TrimWeightPoint(areaManager, 0);
            }
            //Debug.Log(isPorSet);
        }
        if (mode == MakeMode.MakeDataMode)
        {
            GameController._instance.SetAreaData(floorNum, areaDataList);
            GameController._instance.SetEnemyPositon(floorNum, enemyDataList);
        }
    }

    /// <summary>
    /// 次のエリアを生成する　　生成下一个地区
    /// </summary>
    /// <param name="entryPoint">生成するエリアの入口の座標　生成的入口点</param>
    /// <param name="needDirection">必要な入口の方向　入口点的方向</param>
    /// <param name="upFloorNumber">生成するエリアの上のエリアの深度</param>
    void MakeNormalArea(Vector3 entryPoint, OutDirection needDirection, int upFloorNumber)
    {
        int thisFloorNumber = upFloorNumber + 1;
        GameObject areaPrefab = null;
        GameObject[] randomNormalPrefabList = null;


        //GameObject changedPrefab;
        AreaPrefabManager areaManager = null;
        AreaPrefabOut[] areaOutList = null;
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
                areaManager = areaPrefab.GetComponent<AreaPrefabManager>();
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
                        areaManager = areaPrefab.GetComponent<AreaPrefabManager>();
                        areaOutList = areaManager.AreaAngle0.areaOut.getRandomArray();

                        angle = areaOutList[0].direction.getAngleFromTargetDirection(needDirection);
                        areaPostion = entryPoint - areaOutList[0].Rot(angle).position;
                    }
                    #endregion
                    if (mode == MakeMode.MakeSceneMode)
                    {
                        checkRes = checkRes && CheckAreaPhysics(areaPostion, areaManager, angle);
                    }
                    else
                    {
                        checkRes = checkRes && CheckAreaOverlay(areaPostion, areaManager, angle);
                    }
                    if (checkRes)
                    {
                        if (areaManager.type == UnitType.Portal)
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

        //全てのエリアのマッチングが出来ない場合、入口を封鎖する　所有地区生成不能的情况下，封堵
        if (!checkRes)
        {
            CreateWall(entryPoint, needDirection);
            return;
        }

        //ウェイトを調整　调整权重
        TrimWeightPoint(areaManager, thisFloorNumber);

        //エリアを生成する　生成区域
        makingAreaType = normalTypeArray[areaTypeIndex];
        CreateArea(areaPrefab, areaManager, new Vector2(areaPostion.x, areaPostion.z), angle);

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
    /// ウェイトを調整　调整权重算法
    /// </summary>
    /// <param name="makingAreaPrefabInfo">正在构造的地区的信息</param>
    /// <param name="thisFloorNumber">現在のエリア深度</param>
    void TrimWeightPoint(AreaPrefabManager makingAreaPrefabInfo, int thisFloorNumber)
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
    /// <summary>
    /// 封鎖の壁を作る
    /// </summary>
    /// <param name="postion">壁生成の位置</param>
    /// <param name="needDirection">壁の方向</param>
    void CreateWall(Vector3 postion, OutDirection needDirection)
    {
        AreaPrefabManager areaManager;

        makingAreaType = UnitType.Wall;
        if (makingAreaType == UnitType.Road)
        {
            areaManager = WallPrefab[0].GetComponent<AreaPrefabManager>();
            CreateArea(WallPrefab[0], areaManager, new Vector2(postion.x, postion.z), areaManager.AreaAngle0.areaOut[0].direction.getAngleFromTargetDirection(needDirection));
        }
        else
        {
            areaManager = WallPrefab[1].GetComponent<AreaPrefabManager>();
            CreateArea(WallPrefab[1], areaManager, new Vector2(postion.x, postion.z), areaManager.AreaAngle0.areaOut[0].direction.getAngleFromTargetDirection(needDirection));
        }
        //todo
    }
    /// <summary>
    /// エリア生成（実体化）/（データ化）
    /// </summary>
    /// <param name="go">生成するエリアのprefab</param>
    /// <param name="position">生成座標</param>
    /// <param name="angle">エリアの回る角度</param>
    void CreateArea(GameObject go, AreaPrefabManager areaManager, Vector2 position, AngleFix angle)
    {
        if (mode == MakeMode.MakeSceneMode)
        {
            if (areaContainer == null)
            {
                areaContainer = new GameObject("Environment");
            }
            GameObject gameObject = Instantiate(Resources.Load(go.name), new Vector3(position.x, 0, position.y), Quaternion.Euler(0, (int)angle, 0)) as GameObject;
            gameObject.transform.parent = areaContainer.transform;
        }
        else
        {
            areaDataList.Add(
                new AreaData(
                    GameController._instance.GetGoingToFloor(),
                    go.name,
                    position.x,
                    0,
                    position.y,
                    angle,
                    areaManager.GetAreaPrefabInfo(angle).width,
                    areaManager.GetAreaPrefabInfo(angle).height));


            bool isCreateEnemy = false;

            switch (areaManager.type)
            {
                case UnitType.Road:
                    isCreateEnemy = Random.value > 0.75;
                    break;
                case UnitType.Room:
                    isCreateEnemy = Random.value > 0.25;
                    break;
                case UnitType.End:
                    isCreateEnemy = Random.value > 0.5;
                    break;
                case UnitType.Corner:
                    isCreateEnemy = Random.value > 0.75;
                    break;
            }


            if (isCreateEnemy)
            {
                int enemyCount = Random.Range(1, areaManager.GetAreaPrefabInfo(angle).width * areaManager.GetAreaPrefabInfo(angle).height / 25);
                int enemyID = GameController._instance.GetRandomEnemyInfo().ID;
                enemyDataList.Add(
                    new EnemyPositionData(
                        GameController._instance.GetGoingToFloor(),
                        enemyID,
                        enemyCount,
                        GameController._instance.GetGoingToFloor() + 101,
                        position.x,
                        0,
                        position.y,
                        areaManager.GetAreaPrefabInfo(angle).width,
                        areaManager.GetAreaPrefabInfo(angle).height
                    ));
            }
        }
    }


    #endregion



}

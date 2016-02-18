using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitType
{
    Up, Down, Portal, Road, Room, End
}
public enum AreaShape
{
    OnePoint, TowStrike, ThreeStrike, FourStrike, ForSquare
}
public class AreaInfo
{
    public Vector4 roadOut;
    GameObject prefab;
    public AreaInfo(AreaShape areaShape, UnitType unitType, GameObject prefab)
    {

    }



}

public class SceneMaker : MonoBehaviour
{
    bool connect = false;
    public GameObject temp;
    public int length = 0;
    public GameObject[] UpPrefab;
    public GameObject[] DownPrefab;
    public GameObject[] PortalPrefab;
    public GameObject[] RoadPrefab;
    public GameObject[] RoomPrefab;
    public GameObject[] EndPrefab;
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
    // Use this for initialization
    void Start()
    {
        //Debug.Log(temp.transform.collider.);
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
    #region Random Method
    Vector2 RandomVirtualMapPoiot()
    {
        int x = (int)Random.value * length;
        int y = (int)Random.value * length;
        x = x == length ? length - 1 : x;
        y = y == length ? length - 1 : y;
        return new Vector2(x, y);
    }
    int RandomIndex(int indexMax)
    {
        int x = (int)Random.value * indexMax;
        return x == indexMax ? indexMax - 1 : x;
    }
    void RandomArea()
    {

    }
    #endregion
    #region MakeUpStair
    void MakeUpPoint()
    {
        Vector2 upPoint;
        GameObject area;
        AreaManager areaManager;
        do
        {
            upPoint = RandomVirtualMapPoiot();
            area = UpPrefab[RandomIndex(UpPrefab.Length)];
            areaManager = area.GetComponent<AreaManager>();
        }
        while (RandomMapRotation(upPoint, new Vector2(areaManager.width / 10, areaManager.height / 10), areaManager.areaOut) == -1);

        for (int i = 0; i < areaManager.areaOut.Length; i++)
        {
            MakeNextArea(new Vector2(areaManager.areaOut[i].virtualPosition.x + upPoint.x, areaManager.areaOut[i].virtualPosition.y + upPoint.y), (int)areaManager.areaOut[i].direction + 2 > 4 ? areaManager.areaOut[i].direction - 2 : areaManager.areaOut[i].direction + 2);
        }
    }
    /// <summary>
    /// 生成下一个地区
    /// </summary>
    /// <param name="entryPoint">生成的入口点</param>
    /// <param name="needDirection">入口点的方向</param>
    void MakeNextArea(Vector2 entryPoint, OutDirection needDirection)
    {
        //TODO:生成方法
    }

    int RandomMapRotation(Vector2 position, Vector2 virtualSize, AreaOut[] areaOut)
    {
        AreaOut[] tempAreaOut;

        tempAreaOut = areaOut;
        List<AngleFix> canSetDirection = new List<AngleFix>();

        if (checkRotation(position, virtualSize, areaOut))
        {
            canSetDirection.Add(AngleFix.Angle0);
        }
        tempAreaOut = AreaOut.ChangeDirection(virtualSize, areaOut, AngleFix.Angle90);
        if (checkRotation(position, virtualSize, areaOut))
        {
            canSetDirection.Add(AngleFix.Angle90);
        }
        tempAreaOut = AreaOut.ChangeDirection(virtualSize, areaOut, AngleFix.Angle180);
        if (checkRotation(position, virtualSize, areaOut))
        {
            canSetDirection.Add(AngleFix.Angle180);
        }
        tempAreaOut = AreaOut.ChangeDirection(virtualSize, areaOut, AngleFix.Angle270);
        if (checkRotation(position, virtualSize, areaOut))
        {
            canSetDirection.Add(AngleFix.Angle270);
        }
        return canSetDirection.Count == 0 ? -1 : (int)canSetDirection[Random.Range(0, canSetDirection.Count - 1)];
    }


    bool checkRotation(Vector2 position, Vector2 virtualSize, AreaOut[] areaOut)
    {

        if (position.x + virtualSize.x - 1 >= length || position.y + virtualSize.y - 1 >= length)
        {
            return false;
        }

        GameObject tempInfo;
        Vector2 closeAreaPosition;
        OutDirection needDirection;

        for (int i = 0; i < areaOut.Length; i++)
        {
            switch (areaOut[i].direction)
            {
                case OutDirection.up:
                    {
                        //与该路口的相邻点虚拟（地图）世界坐标
                        closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x, position.y + areaOut[i].virtualPosition.y - 1);
                        needDirection = OutDirection.down;
                        break;
                    }
                case OutDirection.down:
                    {
                        //与该路口的相邻点虚拟（地图）世界坐标
                        closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x, position.y + areaOut[i].virtualPosition.y + 1);
                        needDirection = OutDirection.up;
                        break;
                    }
                case OutDirection.left:
                    {
                        closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x - 1, position.y + areaOut[i].virtualPosition.y);
                        needDirection = OutDirection.right;
                        break;
                    }
                case OutDirection.right:
                    {
                        closeAreaPosition = new Vector2(position.x + areaOut[i].virtualPosition.x - 1, position.y + areaOut[i].virtualPosition.y);
                        needDirection = OutDirection.left;
                        break;
                    }
                default:
                    closeAreaPosition = new Vector2(-1, -1);
                    needDirection = OutDirection.up;
                    break;
            }
            mapDictionary.TryGetValue(closeAreaPosition, out tempInfo);

            //如果该相邻点安置了任何区域，并且该区域该点的路口设置不能通过连接验证（该点没有路口设置或者路口方向不正确）
            if (tempInfo != null && !checkRoadCross(closeAreaPosition, tempInfo.GetComponent<AreaManager>(), needDirection))
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 验证地图上一个点是否为地图可以接通的方向
    /// </summary>
    /// <param name="worldVirtualPosition">验证点的虚拟（地图）世界坐标</param>
    /// <param name="targetArea">对象区域的信息对象</param>
    /// <param name="targetNeedDirection">需求该相邻区域的方向</param>
    /// <returns></returns>
    bool checkRoadCross(Vector2 worldVirtualPosition, AreaManager targetArea, OutDirection targetNeedDirection)
    {
        Vector2 localAreaPosition = worldVirtualPosition - targetArea.virtualPosition;
        for (int i = 0; i < targetArea.areaOut.Length; i++)
        {
            //路口位置坐标找到并且该位置上于所需求的方向一致，则通过验证
            if (localAreaPosition == targetArea.areaOut[i].virtualPosition && targetArea.areaOut[i].direction == targetNeedDirection)
            {
                return true;
            }
        }
        return false;
    }
    #endregion 
    void MakeDowPoint()
    {

    }
    void MakeMap()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    bool CheckArea()
    {
        return false;
    }

}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeightPoint
{
    public int roadPoint = 25;
    public int roomPoint = 25;
    public int endPoint = 1;
    public int cornerPoint = 25;
    //public UnitType GetNextAreaType()
    //{
    //    int random = Random.Range(0, roadPoint + roomPoint + endPoint + cornerPoint);
    //    if (random < roadPoint)
    //    {
    //        return UnitType.Road;
    //    }
    //    else if (random < roadPoint + roomPoint)
    //    {
    //        return UnitType.Room;
    //    }
    //    else if (random < roadPoint + roomPoint + endPoint)
    //    {
    //        return UnitType.End;
    //    }
    //    else
    //    {
    //        return UnitType.Corner;
    //    }
    //}
    /// <summary>
    /// 根据权重进行随机，生成随机生成区域种类的顺序
    /// </summary>
    /// <returns></returns>
    public UnitType[] GetWeightRandomTypeOrder(UnitType[] typeList)
    {
        int roadPointTemp = roadPoint;
        int roomPointTemp = roomPoint;
        int endPointTemp = endPoint;
        int cornerPointTemp = cornerPoint;
        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, roadPointTemp + roomPointTemp + endPointTemp + cornerPointTemp);
            if (random < roadPointTemp)
            {
                typeList[i] = UnitType.Road;
                roadPointTemp = 0;
            }
            else if (random < roadPointTemp + roomPointTemp)
            {
                typeList[i] = UnitType.Room;
                roomPointTemp = 0;
            }
            else if (random < roadPointTemp + roomPointTemp + endPointTemp)
            {
                typeList[i] = UnitType.End;
                endPointTemp = 0;
            }
            else if (random < roadPointTemp + roomPointTemp + endPointTemp + cornerPointTemp)
            {
                typeList[i] = UnitType.Corner;
                cornerPointTemp = 0;
            }
            else {

                typeList[i] = UnitType.Wall;
            }

        }
        return typeList;
    }
    public void AddWeight(UnitType type, int point)
    {
        switch (type)
        {
            case UnitType.Road:
                roadPoint += point;
                break;
            case UnitType.Room:
                roomPoint += point;
                break;
            case UnitType.End:
                endPoint += point;
                break;
            case UnitType.Corner:
                cornerPoint += point;
                break;
        }
    }
    public void CutWeight(UnitType type, int point)
    {
        switch (type)
        {
            case UnitType.Road:
                roadPoint = roadPoint - point > 0 ? roadPoint - point : 0;
                break;
            case UnitType.Room:
                roomPoint = roomPoint - point > 0 ? roomPoint - point : 0;
                break;
            case UnitType.End:
                endPoint = endPoint - point > 0 ? endPoint - point : 0;
                break;
            case UnitType.Corner:
                cornerPoint = cornerPoint - point > 0 ? cornerPoint - point : 0;
                break;
        }
    }

    public Dictionary<UnitType, int> GetWeight()
    {
        return new Dictionary<UnitType, int>() {
            {UnitType.Road,roadPoint  },
            {UnitType.Room,roomPoint },
            {UnitType.End,endPoint },
            {UnitType.Corner,cornerPoint }
        };
    }
}

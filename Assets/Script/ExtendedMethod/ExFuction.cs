using UnityEngine;
using System.Collections.Generic;

public static class ExFuction
{

    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        var emission = particleSystem.emission;
        var rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }
    /// <summary>
    /// 入口をPrefabの中心点に参照して回転
    /// </summary>
    /// <param name="areaOut">入口情報</param>
    /// <param name="angle">回転角</param>
    /// <returns></returns>
    public static AreaPrefabOut Rot(this AreaPrefabOut areaOut, AngleFix angle)
    {
        AreaPrefabOut tempArea;
        tempArea.position = areaOut.position;
        tempArea.direction = areaOut.direction;
        switch (angle)
        {
            case AngleFix.Angle90:
                tempArea.position = new Vector3(tempArea.position.z, 0, -tempArea.position.x);
                tempArea.direction = (int)tempArea.direction + 1 > 4 ? tempArea.direction - 3 : tempArea.direction + 1;
                break;
            case AngleFix.Angle180:
                tempArea.position = new Vector3(-tempArea.position.x, 0, -tempArea.position.z);
                tempArea.direction = (int)tempArea.direction + 2 > 4 ? tempArea.direction - 2 : tempArea.direction + 2;
                break;
            case AngleFix.Angle270:
                tempArea.position = new Vector3(-tempArea.position.z, 0, tempArea.position.x);
                tempArea.direction = (int)tempArea.direction + 3 > 4 ? tempArea.direction - 1 : tempArea.direction + 3;
                break;
            default:
                break;
        }
        return tempArea;
    }
    /// <summary>
    /// GameObjectを一つの点に参照して回転
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="referencePoint">参照点</param>
    /// <param name="angle">回転角</param>
    /// <returns></returns>
    public static GameObject Rot(this GameObject gameObject, Vector3 referencePoint, AngleFix angle)
    {
        GameObject go = gameObject;
        go.transform.Rotate(referencePoint, (int)angle);
        return go;
    }
    /// <summary>
    /// 180°回転
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static AngleFix TureBack(this AngleFix angle)
    {
        return ((int)angle) + 180 > 270 ? angle - 180 : angle + 180;
    }
    /// <summary>
    /// 反対の方向を返す
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static OutDirection TureBack(this OutDirection direction)
    {
        return ((int)direction) + 2 > 4 ? direction - 2 : direction + 2;
    }
    /// <summary>
    /// 二つ方向の角度を計算
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="targetDirection"></param>
    /// <returns></returns>
    public static AngleFix getAngleFromTargetDirection(this OutDirection direction, OutDirection targetDirection)
    {
        return (AngleFix)(
            (int)targetDirection - (int)direction >= 0
            ?
            ((int)targetDirection - (int)direction) * 90
            :
            ((int)targetDirection - (int)direction + 4) * 90
            )
            ;
    }
    /// <summary>
    /// 原数组顺序不变,返回乱序后的数组
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="array">泛型数组</param>
    /// <returns></returns>
    public static T[] getRandomArray<T>(this T[] array)
    {
        T[] tempArray = new T[array.Length];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = array[i];
        }
        int x;
        T temp;
        for (int i = 0; i < tempArray.Length; i++)
        {
            x = Random.Range(0, tempArray.Length);
            temp = tempArray[i];
            tempArray[i] = tempArray[x];
            tempArray[x] = temp;
        }
        return tempArray;
    }
    /// <summary>
    /// string型のarrayを全部int型に変更
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static int[] ParseToInt(this string[] array)
    {
        int[] arrayInt=new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            arrayInt[i] = int.Parse(array[i]);
        }
        return arrayInt;
    }

    /// <summary>
    /// arrayから、ランダムで一つを返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static T getRandomOne<T>(this T[] array)
    {
        int x = Random.Range(0, array.Length);

        return array[x];
    }

    /// <summary>
    /// dictionaryのバリューから、ランダムで一つを返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static K getRandomOne<T, K>(this Dictionary<T, K> dictionary)
    {

        int randomIndex = Random.Range(0, dictionary.Count);
        int index = 0;
        foreach (K item in dictionary.Values)
        {
            if (index == randomIndex)
            {
                return item;
            }
            index++;
        }
        return default(K);
    }

}

using UnityEngine;


public static class ExFuction
{

    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        var emission = particleSystem.emission;
        var rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }
    public static AreaOut Rot(this AreaOut areaOut, AngleFix angle)
    {
        AreaOut tempArea;
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
    public static GameObject Rot(this GameObject gameObject, Vector3 referencePoint, AngleFix angle)
    {
        GameObject go = gameObject;
        go.transform.Rotate(referencePoint, (int)angle);
        return go;
    }
    public static AngleFix TureBack(this AngleFix angle)
    {
        return ((int)angle) + 180 > 270 ? angle - 180 : angle + 180;
    }
    public static OutDirection TureBack(this OutDirection direction)
    {
        return ((int)direction) + 2 > 4 ? direction - 2 : direction + 2;
    }
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
    public static T getRandomOne<T>(this T[] array)
    {
        int x = Random.Range(0, array.Length);

        return array[x];
    }

}

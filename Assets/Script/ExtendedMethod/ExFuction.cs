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
    public static Vector3 Rot(this AreaOut areaOut, AngleFix angle)
    {
        AreaOut tempArea;
        tempArea.position = areaOut.position;
        tempArea.direction = areaOut.direction;
        tempArea.position.transform.Rotate(Vector3.zero, (int)angle);
        return tempArea.position.transform.position;
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
        return (AngleFix)((((int)targetDirection - (int)direction > 0 ? (int)targetDirection - (int)direction : (int)targetDirection - (int)direction + 4) - 1) * 90);
    }

}

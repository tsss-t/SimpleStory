using UnityEngine;
using System.Collections;

public class AreaData
{
    public int floorNumber;
    public string areaName;
    public Vector3 areaPosition;
    public Quaternion areaAngle;

    public int widthHalf;
    public int heightHalf;

    public AreaData(int floorNumber, string areaName, float areaPositionX, float areaPositionY, float areaPositionZ, AngleFix angle)
    {
        this.floorNumber = floorNumber;
        this.areaName = areaName;
        this.areaPosition = new Vector3(areaPositionX, areaPositionY, areaPositionZ);
        this.areaAngle = Quaternion.Euler(0, (int)angle, 0);
    }

    public AreaData(int floorNumber, string areaName, float areaPositionX, float areaPositionY, float areaPositionZ, AngleFix angle, int width, int height)
    {
        this.floorNumber = floorNumber;
        this.areaName = areaName;
        this.areaPosition = new Vector3(areaPositionX, areaPositionY, areaPositionZ);
        this.areaAngle = Quaternion.Euler(0, (int)angle, 0);
        this.heightHalf = (int)(height * 0.5);
        this.widthHalf = (int)(width * 0.5);
    }
}

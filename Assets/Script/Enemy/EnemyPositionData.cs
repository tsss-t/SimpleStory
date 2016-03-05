using UnityEngine;
using System.Collections;

public class EnemyPositionData
{
    public int floorNum;
    public string enemyName;
    public int enemeyCount;
    public int enemyLevel;
    public Vector3 leftUpPosition;
    public float width, height;

    public EnemyPositionData(int floorNum, string enemyName, int enemeyCount, int enemyLevel, float positionX, float positonY, float positionZ, float width, float height)
    {
        this.floorNum = floorNum;
        this.enemyName = enemyName;
        this.enemeyCount = enemeyCount;
        this.enemyLevel = enemyLevel;
        this.leftUpPosition = new Vector3(positionX, positonY, positionZ);
        this.width = width;
        this.height = height;
    }
}

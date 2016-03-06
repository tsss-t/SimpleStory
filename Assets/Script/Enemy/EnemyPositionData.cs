using UnityEngine;
using System.Collections;

public class EnemyPositionData
{
    int floorNum;
    int enemyID;
    int enemeyCount;
    int enemyLevel;
    Vector3 leftUpPosition;
    float width, height;

    #region get/set
    public int FloorNum
    {
        get
        {
            return floorNum;
        }

        set
        {
            floorNum = value;
        }
    }

    public int EnemyID
    {
        get
        {
            return enemyID;
        }

        set
        {
            enemyID = value;
        }
    }

    public int EnemeyCount
    {
        get
        {
            return enemeyCount;
        }

        set
        {
            enemeyCount = value;
        }
    }

    public int EnemyLevel
    {
        get
        {
            return enemyLevel;
        }

        set
        {
            enemyLevel = value;
        }
    }

    public Vector3 LeftUpPosition
    {
        get
        {
            return leftUpPosition;
        }

        set
        {
            leftUpPosition = value;
        }
    }

    public float Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }
    #endregion
    public EnemyPositionData(int floorNum, int enemyID, int enemeyCount, int enemyLevel, float positionX, float positonY, float positionZ, float width, float height)
    {
        this.FloorNum = floorNum;
        this.enemyID = enemyID;
        this.EnemeyCount = enemeyCount;
        this.EnemyLevel = enemyLevel;
        this.LeftUpPosition = new Vector3(positionX, positonY, positionZ);
        this.Width = width;
        this.Height = height;
    }
}

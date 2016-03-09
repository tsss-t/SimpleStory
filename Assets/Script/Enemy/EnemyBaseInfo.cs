using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct dropItem
{
    public int itemID;
    public int dropItemLV;
    public int dropItemPre;
}
[System.Serializable]
public class EnemyDropInfo  {
    
    int Id;
    string name;
    List<dropItem> dropItemList;
    float moneyDropPre;

    #region get/set
    public int ID
    {
        get
        {
            return Id;
        }

        set
        {
            Id = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public List<dropItem> DropItemList
    {
        get
        {
            return dropItemList;
        }

        set
        {
            dropItemList = value;
        }
    }

    public float MoneyDropPre
    {
        get
        {
            return moneyDropPre;
        }

        set
        {
            moneyDropPre = value;
        }
    }
    #endregion
    public EnemyDropInfo(int ID,string name,List<dropItem> dropItemList,float moneyDropPre)
    {
        this.Id = ID;
        this.name = name;
        this.dropItemList = dropItemList;
        this.moneyDropPre = moneyDropPre;
    }
}

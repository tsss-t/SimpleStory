using UnityEngine;
using System.Collections.Generic;

public class PlayerEquep
{
    #region para
    public static PlayerEquep nowPlayerEquep
    {
        get
        {
            if (_instance == null)
            {

                _instance = new PlayerEquep();
            }
            return _instance;
        }
    }
    public Dictionary<ItemType, Item> dictionaryEquep;
    private static PlayerEquep _instance;
    #endregion
    #region 初期化
    private PlayerEquep()
    {
        Init();
    }

    private void Init()
    {
        dictionaryEquep = new Dictionary<ItemType, Item>();
        dictionaryEquep.Add(ItemType.head, null);
        dictionaryEquep.Add(ItemType.necklace, null);
        dictionaryEquep.Add(ItemType.body, null);
        dictionaryEquep.Add(ItemType.foot, null);
        dictionaryEquep.Add(ItemType.bracelet, null);
        dictionaryEquep.Add(ItemType.ring, null);
        dictionaryEquep.Add(ItemType.wing, null);
        dictionaryEquep.Add(ItemType.weapon, null);
        //TODO:Load from server
        //TODO:Load from text
    }
    #endregion
    #region 装備管理
    public void SetEquep(Item item)
    {
        if (item.info.type == ItemType.head ||
            item.info.type == ItemType.body ||
            item.info.type == ItemType.necklace ||
            item.info.type == ItemType.ring ||
            item.info.type == ItemType.bracelet ||
            item.info.type == ItemType.foot ||
            item.info.type == ItemType.weapon ||
            item.info.type == ItemType.wing
            )
        {
            dictionaryEquep[item.info.type] = item;
        }

    }
    public void SetdownEquep(ItemType type)
    {
        if (type == ItemType.head ||
        type == ItemType.body ||
        type == ItemType.necklace ||
        type == ItemType.ring ||
        type == ItemType.bracelet ||
        type == ItemType.foot ||
        type == ItemType.weapon ||
        type == ItemType.wing
        )
        {
            dictionaryEquep[type] = null;
        }
    }

    #endregion

}

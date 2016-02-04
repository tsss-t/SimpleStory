using System.Collections.Generic;

public class PlayerBag
{
    #region 初期化
    PlayerEquep playerEquep;
    public static PlayerBag nowPlayerBag
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerBag();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    private int count;
    private static PlayerBag _instance;

    /// <summary>
    /// key---bag index
    /// </summary>
    public Dictionary<int, Item> dictionBag;

    private PlayerBag()
    {
        playerEquep = PlayerEquep.nowPlayerEquep;
        Init();
    }

    /// <summary>
    /// 最初の一回、初期化
    /// </summary>
    private void Init()
    {
        //TODO:Load from server

        //TODO:Load from text

        //TODO:test Load
        dictionBag = new Dictionary<int, Item>();
        //dictionBagのkeyは、データベースのBagのidと対応
        dictionBag.Add(1, new Item(1, true, ItemList.getItem(1)));
        dictionBag.Add(2, new Item(2, 5, ItemList.getItem(3)));
        dictionBag.Add(3, new Item(3, true, ItemList.getItem(2)));

        for (int i = 4; i <= 40; i++)
        {
            dictionBag.Add(i, new Item(i, false, ItemList.getItem(2)));
        }

        foreach (KeyValuePair<int, Item> part in dictionBag)
        {
            if (part.Value.isEqueped)
            {
                ChangeEquep(part.Key);
            }
        }
        count = 40;
    }
    #endregion
    #region 装備変更
    /// <summary>
    /// 装備の撤下、外部使用はplayerstateから使用してください
    /// </summary>
    /// <param name="id">バッグ中のID</param>
    public void SetdownEquep(int id)
    {
        if (playerEquep.dictionaryEquep[dictionBag[id].info.type] != null)
        {
            playerEquep.dictionaryEquep[dictionBag[id].info.type].isEqueped = false;
            playerEquep.SetdownEquep(dictionBag[id].info.type);

        }
    }

    /// <summary>
    /// 装備交換、外部使用はplayerstateから使用してください
    /// </summary>
    /// <param name="id">bagのID</param>
    public void ChangeEquep(int id)
    {
        //TODO:サーバで、アイテムのisEquepedの値を変更


        //前のitemのisEquepedの値を変更
        if (playerEquep.dictionaryEquep[dictionBag[id].info.type] != null)
        {
            playerEquep.dictionaryEquep[dictionBag[id].info.type].isEqueped = false;

        }
        dictionBag[id].isEqueped = true;
        playerEquep.SetEquep(dictionBag[id]);
    }
    #endregion
    #region 新アイテムGet,外部使用はplayerstateから使用してください
    /// <summary>
    /// 新アイテムGet,外部使用はplayerstateから使用してください
    /// </summary>
    /// <param name="itemID">ItemListなかのID</param>
    public void AddItem(int itemID)
    {
        ItemInfo item = ItemList.getItem(itemID);
        if (item.type == ItemType.head ||
        item.type == ItemType.body ||
        item.type == ItemType.necklace ||
        item.type == ItemType.ring ||
        item.type == ItemType.bracelet ||
        item.type == ItemType.foot ||
        item.type == ItemType.weapon ||
        item.type == ItemType.wing
        )
        {
            count++;
            dictionBag.Add(count, new Item(count, false, ItemList.getItem(itemID)));
        }
        else
        {
            bool isFind = false;
            foreach (KeyValuePair<int, Item> itemTemp in dictionBag)
            {
                if (item == itemTemp.Value.info)
                {
                    itemTemp.Value.count++;
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
            {
                count++;
                dictionBag.Add(count, new Item(count, false, ItemList.getItem(itemID)));
            }

        }
    }
    /// <summary>
    /// バッグ中のアイテムID
    /// </summary>
    /// <param name="itemBagID"></param>
    public void DeleteItem(int itemBagID)
    {
        ItemInfo item = dictionBag[itemBagID].info;

        if (item.type == ItemType.head ||
        item.type == ItemType.body ||
        item.type == ItemType.necklace ||
        item.type == ItemType.ring ||
        item.type == ItemType.bracelet ||
        item.type == ItemType.foot ||
        item.type == ItemType.weapon ||
        item.type == ItemType.wing
        )
        {
            dictionBag.Remove(itemBagID);
        }
        else
        {
            if (dictionBag[itemBagID].count != 1)
            {
                dictionBag[itemBagID].count--;
            }
            else
            {
                dictionBag.Remove(itemBagID);
            }
        }
    }
    /// <summary>
    /// 選択した装備は今装備していますかとか
    /// </summary>
    /// <param name="itemBagID">バッグ中のID</param>
    /// <returns></returns>
    public bool isEqueped(int itemBagID)
    {
        if (dictionBag[itemBagID].info.type == ItemType.head ||
        dictionBag[itemBagID].info.type == ItemType.body ||
        dictionBag[itemBagID].info.type == ItemType.necklace ||
        dictionBag[itemBagID].info.type == ItemType.ring ||
        dictionBag[itemBagID].info.type == ItemType.bracelet ||
        dictionBag[itemBagID].info.type == ItemType.foot ||
        dictionBag[itemBagID].info.type == ItemType.weapon ||
        dictionBag[itemBagID].info.type == ItemType.wing
        )
        {
            if (playerEquep.dictionaryEquep[dictionBag[itemBagID].info.type].id == itemBagID)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 指定アイテムの数を算出
    /// </summary>
    /// <param name="itemID">アイテムリスト中のID</param>
    /// <returns></returns>
    public int GetItemCount(int itemID)
    {
        int count = 0;
        foreach (Item item in dictionBag.Values)
        {
            if (item.info.id == itemID)
            {
                count += item.count;
            }
        }
        return count;
    }
    #endregion
}



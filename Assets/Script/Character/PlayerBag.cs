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
        dictionBag = GameController._instans.LoadBag();

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
    #region 外部API
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
        if (!ItemInfo.IsEquep(item.type)
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
    #endregion
    #region アイテム削除
    /// <summary>
    /// 使用/削除　アイテム
    /// </summary>
    /// <param name="itemBagID">バッグ中のアイテムID</param>
    public void DeleteItem(int itemBagID)
    {
        ItemInfo item = dictionBag[itemBagID].info;

        if (ItemInfo.IsEquep(item.type))
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
    /// 使用/削除　アイテム
    /// </summary>
    /// <param name="itemBagID">バッグ中のアイテムID</param>
    /// <param name="count">アイテム数</param>
    public void DeleteItem(int itemBagID, int count)
    {
        ItemInfo item = dictionBag[itemBagID].info;

        if (ItemInfo.IsEquep(item.type))
        {
            dictionBag.Remove(itemBagID);
        }
        else
        {
            if (dictionBag[itemBagID].count > count)
            {
                dictionBag[itemBagID].count -= count;
            }
            else
            {
                dictionBag.Remove(itemBagID);
            }
        }
    }
    /// <summary>
    ///　クエスト必要なものを提出
    /// </summary>
    /// <param name="itemID">アイテムリストの中のID</param>
    /// <param name="count">アイテム必要数</param>
    public void UseItemToOverQuest(int itemID, int count)
    {
        List<int> deleteList = new List<int>();
        foreach (Item item in dictionBag.Values)
        {
            if (count > 0)
            {
                if (item.info.id == itemID && !item.isEqueped)
                {
                    if (ItemInfo.IsEquep(item.info.type))
                    {
                        deleteList.Add(item.id);
                        count--;
                    }
                    else
                    {
                        DeleteItem(item.id, count);
                        break;
                    }
                }
            }
            else
            {
                break;
            }
        }
        for (int i = 0; i < deleteList.Count; i++)
        {
            DeleteItem(deleteList[i]);
        }
    }
    #endregion

    /// <summary>
    /// バッグ中指定したアイテムのリストIDを貰う
    /// </summary>
    /// <param name="bagItemID">バッグ中のID</param>
    /// <returns>アイテムリストのID</returns>
    public int BagIDToItemID(int bagItemID)
    {
        return dictionBag[bagItemID].info.id;
    }

    /// <summary>
    /// 選択した装備は今装備していますかとか
    /// </summary>
    /// <param name="itemBagID">バッグ中のID</param>
    /// <returns></returns>
    public bool isEqueped(int itemBagID)
    {
        if (ItemInfo.IsEquep(dictionBag[itemBagID].info.type)
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
            if (item.info.id == itemID && !item.isEqueped)
            {
                count += item.count;
            }
        }
        return count;
    }
    #endregion
}



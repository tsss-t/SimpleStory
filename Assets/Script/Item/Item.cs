using UnityEngine;
using System.Collections;

public class Item
{
    #region para
    /// <summary>
    /// バッグ中のID
    /// </summary>
    public int id;
    public int count;
    public bool isEqueped;
    public ItemInfo info;
    #endregion
    #region 初期化
    public Item(int id, bool isEqueped, ItemInfo info)
    {
        this.id = id;
        this.count = 1;
        this.info = info;
        this.isEqueped = isEqueped;
    }
    public Item(int id, int count, ItemInfo info)
    {
        this.id = id;
        this.count = count;
        this.info = info;
        this.isEqueped = false;
    }
    #endregion
}

using System.Collections.Generic;

/// <summary>
/// ゲーム中のアイテムリスト（全ゲーム）
/// </summary>
public static class ItemList {

    private static Dictionary<int, ItemInfo> _instance;
    public static Dictionary<int, ItemInfo> getItemList
    {
        get {
            if (_instance == null)
            {
                _instance = new Dictionary<int, ItemInfo>();
                LoadItemData();

                return _instance;
            }
            else {
                return _instance;
            }
        }
    }
    static void LoadItemData()
    {
        //TODO:アイテムリストをサーバーから読み込む

        //test
        _instance.Add(1, new ItemInfo(1, ItemType.head, "男性头盔 (2)", "坚韧头盔", 100, 100, 100, 100, 100, 1, 1, 1, 1,1,10));

        _instance.Add(2, new ItemInfo(2, ItemType.body, "男性盔甲 (2)", "无谓铠甲", 100, 100, 100, 100, 100, 1, 1, 1, 1,1,10));

        _instance.Add(3, new ItemInfo(3, "小体力丹", "HP回復剤（小）",500,0,3));

        _instance.Add(4, new ItemInfo(4, "pic_星星", "地上の星", 500, 100,3));

        _instance.Add(5, new ItemInfo(5, ItemType.bracelet, "男性 手镯  (2)", "大手镯", 100, 100, 100, 100, 100, 1, 1, 1, 1, 1, 10));
    }
    public static ItemInfo getItem(int id)
    {
        if(_instance==null)
        {
            _instance = new Dictionary<int, ItemInfo>();
            LoadItemData();
        }
        ItemInfo itemInfo;
        if (_instance.TryGetValue(id, out itemInfo))
        {
            return itemInfo;
        }
        else
        {
            return null;
        }

    }

}

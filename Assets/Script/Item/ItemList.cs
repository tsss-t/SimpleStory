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
        _instance = GameController._instance.GetItemList();
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

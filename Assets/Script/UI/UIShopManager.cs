using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIShopManager : MonoBehaviour
{
    private int selectedShopID;
    public GameObject prefabItem;
    public GameObject prefabItemE;
    private GameObject containShopGrid;
    private Dictionary<int, Item> shopItemsDictionary;
    private Dictionary<int, GameObject> dictionaryUIShopItem;
    // Use this for initialization
    void Start()
    {
        shopItemsDictionary = new Dictionary<int, Item>();
        dictionaryUIShopItem = new Dictionary<int, GameObject>();
        containShopGrid = transform.Find("Scroll View").Find("Items").gameObject;
    }

    void LoadData(int shopID)
    {
        //TODO:データベースからデータ読み取り

        //test用
        shopItemsDictionary.Clear();
        shopItemsDictionary.Add(1, new Item(1, false, ItemList.getItem(1)));
        shopItemsDictionary.Add(2, new Item(2, false, ItemList.getItem(2)));
        shopItemsDictionary.Add(3, new Item(3, false, ItemList.getItem(1)));
        shopItemsDictionary.Add(4, new Item(4, false, ItemList.getItem(1)));
        shopItemsDictionary.Add(5, new Item(5, 6, ItemList.getItem(4)));
        shopItemsDictionary.Add(6, new Item(6, 5, ItemList.getItem(3)));
        shopItemsDictionary.Add(7, new Item(7, false, ItemList.getItem(5)));
    }
    public void UpdateShop(int shopID)
    {
        LoadData(shopID);
        ShopUpdated();
    }
    public Dictionary<int, Item> getShopDictionary()
    {
        return shopItemsDictionary;
    }
    #region updateShop
    GameObject go;
    private void ShopUpdated()
    {
        for (int i = 1; i <= dictionaryUIShopItem.Count; i++)
        {
            dictionaryUIShopItem.TryGetValue(i, out go);
            NGUITools.Destroy(go);
        }
        dictionaryUIShopItem.Clear();
        int j = 1;
        foreach (KeyValuePair<int, Item> item in shopItemsDictionary)
        {
            if (item.Value.isEqueped)
            {
                go = NGUITools.AddChild(containShopGrid, prefabItemE);
            }
            else if (item.Value.count > 1)
            {
                go = NGUITools.AddChild(containShopGrid, prefabItemE);
                go.transform.Find("E").Find("Label").GetComponent<UILabel>().text = item.Value.count.ToString();
                go.transform.Find("E").Find("Label").GetComponent<UILabel>().color = Color.white;
            }
            else
            {
                go = NGUITools.AddChild(containShopGrid, prefabItem);
            }
            go.transform.Find("Sprite").GetComponent<UISprite>().spriteName = item.Value.info.adress;
            go.name = item.Key.ToString();
            dictionaryUIShopItem.Add(j++, go);
        }
        containShopGrid.GetComponent<UIGrid>().enabled = true;
    }
    #endregion
    public void Show()
    {
        UpdateShop(selectedShopID);
        this.gameObject.SetActive(true);
    }
    public void SetSelectShop(int shopID)
    {
        selectedShopID = shopID;
    }
}

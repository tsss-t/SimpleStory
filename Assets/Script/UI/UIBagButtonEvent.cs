using UnityEngine;
using System.Collections;
public enum ItemFrom
{
    Bag,Equep,Shop
}
public class UIBagButtonEvent : MonoBehaviour {
    public ItemFrom from;
    UIItemInfoPanel ItemInfoPanel;
    PlayerState playerState;
    Item item;
    // Use this for initialization
    void Start () {
        playerState = PlayerState.GamePlayerState;

        ItemInfoPanel = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("EquepMenu").Find("ItemInfoPanel").GetComponent<UIItemInfoPanel>();
        switch (from)
        {
            case ItemFrom.Bag:
                item = playerState.GetPlayerBag().dictionBag[int.Parse(gameObject.transform.parent.name)];
                break;
            case ItemFrom.Equep:

                break;
            case ItemFrom.Shop:
                item = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("EquepMenu").Find("ShopBG").GetComponent<UIShopManager>().getShopDictionary()[int.Parse(gameObject.transform.parent.name)];
                break;
            default:
                break;
        }
    }
    void OnClick()
    {
        ItemInfoPanel.ShowInfo(item,from);
    }
}

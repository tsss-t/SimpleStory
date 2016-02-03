using UnityEngine;
using System.Collections;

public class UIItemInfoPanel : MonoBehaviour
{
    #region para
    UIBagManager bagManager;
    PlayerState playerState;
    GameObject EquepDownButton;
    GameObject EquepUpButton;
    GameObject UseButton;
    GameObject SellButton;
    GameObject BuyButton;
    GameObject ItemInfoBG;
    UISprite equepSprite;
    UILabel STRLable;
    UILabel CONLable;
    UILabel INTLable;
    UILabel DEXLable;
    UILabel LUKLable;
    #endregion
    #region Start
    // Use this for initialization
    void Start()
    {
        playerState = PlayerState.GamePlayerState;
        bagManager = transform.parent.GetComponent<UIBagManager>();
        ItemInfoBG = transform.Find("ItemInfoBG").gameObject;
        equepSprite = ItemInfoBG.transform.Find("Item").Find("Sprite").GetComponent<UISprite>();
        STRLable = ItemInfoBG.transform.Find("LabelSTRInfo").GetComponent<UILabel>();
        INTLable = ItemInfoBG.transform.Find("LabelINTInfo").GetComponent<UILabel>();
        DEXLable = ItemInfoBG.transform.Find("LabelDEXInfo").GetComponent<UILabel>();
        CONLable = ItemInfoBG.transform.Find("LabelCONInfo").GetComponent<UILabel>();
        LUKLable = ItemInfoBG.transform.Find("LabelLUKInfo").GetComponent<UILabel>();

        EquepDownButton = ItemInfoBG.transform.Find("EquepDownButton").gameObject;
        EquepUpButton = ItemInfoBG.transform.Find("EquepUpButton").gameObject;
        UseButton = ItemInfoBG.transform.Find("UseButton").gameObject;
        SellButton = ItemInfoBG.transform.Find("SellButton").gameObject;
        BuyButton = ItemInfoBG.transform.Find("BuyButton").gameObject;

        
    }
    #endregion
    #region show EquepInfoPanel
    void MakeInfoPanle(Item item,ItemFrom from)
    {
        EquepDownButton.SetActive(false);
        EquepUpButton.SetActive(false);
        UseButton.SetActive(false);
        SellButton.SetActive(false);
        BuyButton.SetActive(false);

        equepSprite.spriteName = item.info.adress;
        if (item.info.type == ItemType.head ||
            item.info.type == ItemType.necklace ||
            item.info.type == ItemType.body ||
            item.info.type == ItemType.foot ||
            item.info.type == ItemType.bracelet ||
            item.info.type == ItemType.weapon ||
            item.info.type == ItemType.wing ||
            item.info.type == ItemType.ring)
        {
            STRLable.text = item.info.STR.ToString();
            INTLable.text = item.info.INT.ToString();
            DEXLable.text = item.info.DEX.ToString();
            CONLable.text = item.info.CON.ToString();
            LUKLable.text = item.info.LUK.ToString();
            switch (playerState.GetActionInfoNow())
            {
                case PlayerState.PlayerAction.Free:
                    if (item.isEqueped)
                    {
                        EquepDownButton.SetActive(true);
                    }
                    else
                    {
                        EquepUpButton.SetActive(true);
                    }
                    break;
                case PlayerState.PlayerAction.Died:
                    break;
                case PlayerState.PlayerAction.Shopping:
                    if(from== ItemFrom.Bag)
                    {
                        SellButton.SetActive(true);
                    }
                    else if(from== ItemFrom.Shop)
                    {
                        BuyButton.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

        }
        else
        {
            switch (playerState.GetActionInfoNow())
            {
                case PlayerState.PlayerAction.Free:
                    UseButton.SetActive(true);
                    break;
                case PlayerState.PlayerAction.Died:
                    break;
                case PlayerState.PlayerAction.Shopping:
                    if (from == ItemFrom.Bag)
                    {
                        SellButton.SetActive(true);
                    }
                    else if (from == ItemFrom.Shop)
                    {
                        BuyButton.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void ShowInfo(Item item,ItemFrom from)
    {
        if (item != null)
        {
            MakeInfoPanle(item,from);
            bagManager.SetSelectItem(item);
            bagManager.OnEquepInfoPanelOpenButtonClick();
        }
    }
    #endregion
}

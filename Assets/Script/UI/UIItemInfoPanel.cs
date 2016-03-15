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
    GameObject ConstLabels;
    GameObject MoneyIcon;
    UISprite equepSprite;
    UILabel STRLabel;
    UILabel CONLabel;
    UILabel INTLabel;
    UILabel DEXLabel;
    UILabel LUKLabel;
    UILabel NameLabel;
    UILabel DescripteLabel;
    UILabel MoneyLabel;

    #endregion
    #region Start
    // Use this for initialization
    void Start()
    {
        playerState = PlayerState._instance;
        bagManager = transform.parent.GetComponent<UIBagManager>();
        ItemInfoBG = transform.Find("ItemInfoBG").gameObject;
        equepSprite = ItemInfoBG.transform.Find("Item").Find("Sprite").GetComponent<UISprite>();
        STRLabel = ItemInfoBG.transform.Find("LabelSTRInfo").GetComponent<UILabel>();
        INTLabel = ItemInfoBG.transform.Find("LabelINTInfo").GetComponent<UILabel>();
        DEXLabel = ItemInfoBG.transform.Find("LabelDEXInfo").GetComponent<UILabel>();
        CONLabel = ItemInfoBG.transform.Find("LabelCONInfo").GetComponent<UILabel>();
        LUKLabel = ItemInfoBG.transform.Find("LabelLUKInfo").GetComponent<UILabel>();
        NameLabel = ItemInfoBG.transform.Find("LabelName").GetComponent<UILabel>();
        DescripteLabel = ItemInfoBG.transform.Find("LabelDescript").GetComponent<UILabel>();
        EquepDownButton = ItemInfoBG.transform.Find("EquepDownButton").gameObject;
        EquepUpButton = ItemInfoBG.transform.Find("EquepUpButton").gameObject;
        UseButton = ItemInfoBG.transform.Find("UseButton").gameObject;
        SellButton = ItemInfoBG.transform.Find("SellButton").gameObject;
        BuyButton = ItemInfoBG.transform.Find("BuyButton").gameObject;
        ConstLabels = ItemInfoBG.transform.Find("ConstLabels").gameObject;
        MoneyIcon = ItemInfoBG.transform.Find("MoneyIcon").gameObject;
        MoneyLabel = MoneyIcon.transform.Find("MoneyLabel").GetComponent<UILabel>();
    }
    #endregion
    #region show EquepInfoPanel
    void MakeInfoPanle(Item item, ItemFrom from)
    {
        EquepDownButton.SetActive(false);
        EquepUpButton.SetActive(false);
        UseButton.SetActive(false);
        SellButton.SetActive(false);
        BuyButton.SetActive(false);
        MoneyIcon.SetActive(false);
        equepSprite.spriteName = item.info.adress;
        NameLabel.text = item.info.name;
        DescripteLabel.text = item.info.descript;

        if (ItemInfo.IsEquep(item.info.type))
        {
            ConstLabels.SetActive(true);
            STRLabel.text = item.info.STR.ToString();
            INTLabel.text = item.info.INT.ToString();
            DEXLabel.text = item.info.DEX.ToString();
            CONLabel.text = item.info.CON.ToString();
            LUKLabel.text = item.info.LUK.ToString();
            MoneyLabel.text = item.info.money.ToString();
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
                    if (from == ItemFrom.Bag)
                    {
                        MoneyIcon.SetActive(true);
                        SellButton.SetActive(true);
                    }
                    else if (from == ItemFrom.Shop)
                    {
                        MoneyIcon.SetActive(true);
                        BuyButton.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

        }
        else
        {
            ConstLabels.SetActive(false);
            STRLabel.text = string.Empty;
            INTLabel.text = string.Empty;
            DEXLabel.text = string.Empty;
            CONLabel.text = string.Empty;
            LUKLabel.text = string.Empty;
            MoneyLabel.text = item.info.money.ToString();
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
                        MoneyIcon.SetActive(true);
                        SellButton.SetActive(true);
                    }
                    else if (from == ItemFrom.Shop)
                    {
                        MoneyIcon.SetActive(true);
                        BuyButton.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void ShowInfo(Item item, ItemFrom from)
    {
        if (item != null)
        {
            MakeInfoPanle(item, from);
            bagManager.SetSelectItem(item);
            bagManager.OnEquepInfoPanelOpenButtonClick();
        }
    }
    #endregion
}

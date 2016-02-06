using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class UIBagManager : MonoBehaviour
{
    private enum BagMode
    {
        Equep, Shop
    }
    #region para
    PlayerState playerState;
    UIController mainControllerUI;

    bool isShowPanel;

    #region bag
    public GameObject prefabItem;
    public GameObject prefabItemE;
    /// <summary>
    /// バッグの内容
    /// </summary>
    private Dictionary<int, GameObject> dictionaryUIBagItem;
    private PlayerBag playerBag;


    private GameObject containBagGrid;
    #endregion

    #region equep
    public Dictionary<ItemType, ItemInfo> dictionaryEquep;
    private GameObject containEqueps;
    #endregion

    #region Shop

    private GameObject containShop;
    private UIShopManager shopManagerUI;

    #endregion

    #region statusUI

    private Transform containState;
    private UILabel HPstate;
    private UILabel STRstate;
    private UILabel DEXstate;
    private UILabel INTstate;
    private UILabel Energystate;
    private UILabel EXPstate;
    private UILabel ATKstate;
    private UILabel DEFstate;
    private UILabel CONstate;
    private UILabel Moneystate;
    #endregion

    GameObject containEquepInfo;
    Item selectItem;
    #endregion
    #region start
    void Start()
    {
        playerState = PlayerState.GamePlayerState;
        playerBag = playerState.GetPlayerBag();
        isShowPanel = false;
        //equepInfo = plyerState.getPlayerEquep();

        dictionaryUIBagItem = new Dictionary<int, GameObject>();
        dictionaryEquep = new Dictionary<ItemType, ItemInfo>();

        mainControllerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).GetComponent<UIController>();

        containBagGrid = transform.Find("BagBG").Find("Scroll View").Find("Items").gameObject;
        containEqueps = transform.Find("EquepBG").gameObject;
        containEquepInfo = transform.Find("ItemInfoPanel").Find("ItemInfoBG").gameObject;
        containShop = transform.Find("ShopBG").gameObject;

        containState = containEqueps.transform.Find("StateBG");


        HPstate = containState.Find("LabelHPstate").GetComponent<UILabel>();
        STRstate = containState.Find("LabelSTRstate").GetComponent<UILabel>();
        DEXstate = containState.Find("LabelDEXstate").GetComponent<UILabel>();
        INTstate = containState.Find("LabelINTstate").GetComponent<UILabel>();
        Energystate = containState.Find("LabelEnergystate").GetComponent<UILabel>();
        EXPstate = containState.Find("LabelEXPstate").GetComponent<UILabel>();
        ATKstate = containState.Find("LabelATKstate").GetComponent<UILabel>();
        DEFstate = containState.Find("LabelDEFstate").GetComponent<UILabel>();
        CONstate = containState.Find("LabelCONstate").GetComponent<UILabel>();
        Moneystate = transform.Find("BagBG").Find("LabelMoney").GetComponent<UILabel>();
        shopManagerUI = containShop.GetComponent<UIShopManager>();

        playerState.OnPlayerStateChanged += OnStatesChanged;
        gameObject.SetActive(false);

        containEquepInfo.SetActive(false);
    }
    #endregion

    #region Delegate /UI update
    private void OnStatesChanged(PlayerStateChangeType type)
    {
        if (type == PlayerStateChangeType.STATE || type == PlayerStateChangeType.all)
        {
            updateState();
        }
        if (type == PlayerStateChangeType.equep || type == PlayerStateChangeType.bag)
        {
            if (gameObject.activeSelf)
            {
                updateBag();
                //updateEquep();
            }
        }
        if (type == PlayerStateChangeType.all || type == PlayerStateChangeType.money)
        {
            if (gameObject.activeSelf)
            {
                updateMoney();
            }
        }

    }
    #region updateBag
    GameObject go;
    /// <summary>
    /// 初期とバッグ内容変更の時使用
    /// </summary>
    private void updateBag()
    {
        playerBag = PlayerState.GamePlayerState.GetPlayerBag();
        for (int i = 1; i <= dictionaryUIBagItem.Count; i++)
        {
            dictionaryUIBagItem.TryGetValue(i, out go);
            NGUITools.Destroy(go);
        }

        dictionaryUIBagItem.Clear();
        int j = 1;
        foreach (KeyValuePair<int, Item> item in playerBag.dictionBag)
        {
            if (item.Value.isEqueped)
            {
                go = NGUITools.AddChild(containBagGrid, prefabItemE);
            }
            else if (item.Value.count > 1)
            {
                go = NGUITools.AddChild(containBagGrid, prefabItemE);
                go.transform.Find("E").Find("Label").GetComponent<UILabel>().text = item.Value.count.ToString();
                go.transform.Find("E").Find("Label").GetComponent<UILabel>().color = Color.white;
            }
            else
            {
                go = NGUITools.AddChild(containBagGrid, prefabItem);
            }
            go.transform.Find("Sprite").GetComponent<UISprite>().spriteName = item.Value.info.adress;
            go.name = item.Key.ToString();
            dictionaryUIBagItem.Add(j++, go);
        }
        containBagGrid.GetComponent<UIGrid>().enabled = true;
    }
    #endregion
    #region updateState
    /// <summary>
    /// 主人公の数値更新する時、使用する
    /// </summary>
    private void updateState()
    {
        HPstate.text = playerState.HPMax.ToString();
        STRstate.text = playerState.STR.ToString();
        DEXstate.text = playerState.DEX.ToString();
        INTstate.text = playerState.INT.ToString();
        Energystate.text = playerState.energyMax.ToString();
        EXPstate.text = playerState.EXP.ToString();
        ATKstate.text = playerState.ATK.ToString();
        DEFstate.text = playerState.DEF.ToString();
        CONstate.text = playerState.CON.ToString();
    }
    #endregion
    void updateMoney()
    {
        Moneystate.text = playerState.money.ToString();
    }
    #endregion
    #region UI Event
    public void SetSelectItem(Item item)
    {
        this.selectItem = item;
    }
    #region Menu Event
    public void OnCloseButtonClick()
    {
        PlayerState.GamePlayerState.ChangeAction(PlayerState.PlayerAction.Free);
        OnEquepInfoPanelCloseButtonClick();
        Hide();
    }
    public void OpenPanelEquep()
    {
        MenuButtonClick(BagMode.Equep);
    }
    public void OpenPanelShop(int shopID)
    {
        if (!isShowPanel)
        {
            shopManagerUI.SetSelectShop(shopID);
            MenuButtonClick(BagMode.Shop);
            PlayerState.GamePlayerState.ChangeAction(PlayerState.PlayerAction.Shopping);
        }
    }

    void MenuButtonClick(BagMode mode)
    {
        OnEquepInfoPanelCloseButtonClick();
        containEqueps.SetActive(false);
        containShop.SetActive(false);

        switch (mode)
        {
            case BagMode.Equep:
                {
                    containEqueps.SetActive(true);
                    break;
                }
            case BagMode.Shop:
                {
                    shopManagerUI.Show();
                    break;
                }
            default:
                {

                    break;
                }
        }
        if (gameObject.activeSelf)
        {
            //GetComponent<UITweener>().PlayReverse();
            Hide();
        }
        else
        {
            Show();
            //GetComponent<UITweener>().PlayForward();
            OnStatesChanged(PlayerStateChangeType.bag);
            OnStatesChanged(PlayerStateChangeType.equep);
            OnStatesChanged(PlayerStateChangeType.money);
        }
    }
    void Show()
    {
        if (!isShowPanel)
        {
            this.gameObject.SetActive(true);
            mainControllerUI.CloseAllWindows();
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            StartCoroutine(HidePanel());
        }
    }
    #endregion
    #region InfoPanel Event
    public void OnEquepInfoPanelCloseButtonClick()
    {
        if (containEquepInfo.activeSelf)
        {
            containEquepInfo.SetActive(false);
        }

    }
    public void OnEquepInfoPanelOpenButtonClick()
    {
        if (!containEquepInfo.activeSelf)
        {
            containEquepInfo.SetActive(true);
        }
    }

    public void OnSetdownButtonClicked()
    {
        playerState.SetdownEquep(selectItem.id);
        OnEquepInfoPanelCloseButtonClick();
    }
    public void OnSetupButtonClick()
    {
        playerState.ChangeEquep(selectItem.id);
        OnEquepInfoPanelCloseButtonClick();
    }
    public void OnUseButtonClick()
    {
        if (selectItem.info.type == ItemType.drug)
        {
            if (selectItem.count == 1)
            {
                OnEquepInfoPanelCloseButtonClick();
            }
            playerState.UseItem(selectItem.id);
        }

    }
    #endregion
    #region Shop Event
    public void OnBuyButtonClick()
    {
        playerState.BuyItem(selectItem.info.id);
        OnEquepInfoPanelCloseButtonClick();
    }
    public void OnShopButtonClick()
    {
    }
    public void OnSellButtonClick()
    {
        playerState.SellItem(selectItem.id);
        OnEquepInfoPanelCloseButtonClick();
    }
    #endregion
    #region Panel CUTIN/OUT
    IEnumerator HidePanel()
    {

        this.GetComponent<UITweener>().PlayForward();
        yield return new WaitForSeconds(this.transform.GetComponent<UITweener>().duration);
        this.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        this.GetComponent<UITweener>().PlayReverse();
        yield return new WaitForSeconds(this.transform.GetComponent<UITweener>().duration);
        isShowPanel = true;
    }
    #endregion
    #endregion
}

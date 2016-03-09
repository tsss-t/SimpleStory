using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIComposeManager : MonoBehaviour
{

    #region para
    #region editor
    public static UIComposeManager _instance;

    public GameObject composeItemOneClipPrefab;
    public GameObject composeItemTwoClipPrefab;
    public GameObject composeItemThreeClipPrefab;
    public GameObject composeItemFourClipPrefab;

    #endregion

    bool isShowPanel;
    UIComposeButtonEvent selectedComposeItem;
    UIButton selectType;

    List<GameObject> composeShowList;
    List<ItemCompose> composeItemList;
    UIController uiRoot;

    #region UI
    UIGrid composeItemsGrid;
    UILabel paraInfoLabel;
    UIButton composeButton;

    UIButton weapenButtonBG;
    UIButton headButtonBG;
    UIButton neckButtonBG;
    UIButton coadButtonBG;
    UIButton footButtonBG;
    UIButton ringButtonBG;
    UIButton braButtonBG;
    UIButton wingButtonBG;
    UIButton itemButtonBG;



    #endregion



    #endregion
    #region 初期化

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        isShowPanel = false;
        uiRoot = UIController._instance;
        composeShowList = new List<GameObject>();
        composeItemList = new List<ItemCompose>();

        composeItemsGrid = transform.Find("ComposeContainer/BG/Scroll View/Items").GetComponent<UIGrid>();
        paraInfoLabel = transform.Find("ComposeContainer/BG/ParaBG/Scroll View/LabelInfo").GetComponent<UILabel>();
        composeButton = transform.Find("ComposeContainer/BG/ComposeButton").GetComponent<UIButton>();


        weapenButtonBG = transform.Find("ComposeContainer/BG/TypeBG/WeapenButton").GetComponent<UIButton>();
        headButtonBG = transform.Find("ComposeContainer/BG/TypeBG/HeadButton").GetComponent<UIButton>();
        neckButtonBG = transform.Find("ComposeContainer/BG/TypeBG/NeckButton").GetComponent<UIButton>();
        coadButtonBG = transform.Find("ComposeContainer/BG/TypeBG/CoadButton").GetComponent<UIButton>();
        footButtonBG = transform.Find("ComposeContainer/BG/TypeBG/FootButton").GetComponent<UIButton>();
        ringButtonBG = transform.Find("ComposeContainer/BG/TypeBG/RingButton").GetComponent<UIButton>();
        braButtonBG = transform.Find("ComposeContainer/BG/TypeBG/BraButton").GetComponent<UIButton>();
        wingButtonBG = transform.Find("ComposeContainer/BG/TypeBG/WingButton").GetComponent<UIButton>();
        itemButtonBG = transform.Find("ComposeContainer/BG/TypeBG/ItemButton").GetComponent<UIButton>();


    }


    void UpdatePanel(ItemType type)
    {
        GameObject go;
        this.paraInfoLabel.text = "";
        for (int i = 0; i < composeShowList.Count; i++)
        {
            NGUITools.Destroy(composeShowList[i]);
        }
        composeShowList.Clear();


        for (int i = 0; i < composeItemList.Count; i++)
        {
            switch (composeItemList[i].NeedItem.Count)
            {
                case 1:
                    {
                        go = NGUITools.AddChild(composeItemsGrid.gameObject, composeItemOneClipPrefab);

                        break;
                    }

                case 2:
                    {
                        go = NGUITools.AddChild(composeItemsGrid.gameObject, composeItemTwoClipPrefab);

                        break;
                    }
                case 3:
                    {
                        go = NGUITools.AddChild(composeItemsGrid.gameObject, composeItemThreeClipPrefab);

                        break;
                    }

                case 4:
                    {
                        go = NGUITools.AddChild(composeItemsGrid.gameObject, composeItemFourClipPrefab);

                        break;
                    }
                default:
                    {
                        Debug.LogError(string.Format("Item ID: {0} Item Compose Data Error", composeItemList[i]));
                        return;
                    }

            }
            go.GetComponent<UIComposeButtonEvent>().SetItemCompose(composeItemList[i]);
            composeShowList.Add(go);
        }
        this.composeItemsGrid.enabled = true;
    }

    #endregion

    #region UI Events

    public void OnCloseButtonClick()
    {
        this.Hide();
    }
    public void OnCommunicationStart()
    {
        composeButton.SetState(UIButtonColor.State.Disabled, false);
        composeButton.GetComponent<Collider>().enabled = false;
        paraInfoLabel.text = "";
        this.UpdatePanel(ItemType.weapon);
        this.Show();
    }

    public void OnWeapenButtonClick()
    {
        selectType = weapenButtonBG;
        composeTypeChange(ItemType.weapon);
    }
    public void OnHeadButtonClick()
    {
        if (selectType != headButtonBG)
        {
            selectType = headButtonBG;
            composeTypeChange(ItemType.head);
        }

    }
    public void OnNeckButtonClick()
    {
        if (selectType != neckButtonBG)
        {
            selectType = neckButtonBG;
            composeTypeChange(ItemType.necklace);
        }
    }
    public void OnCoadButtonClick()
    {
        if (selectType != coadButtonBG)
        {
            selectType = coadButtonBG;
            composeTypeChange(ItemType.body);
        }

    }

    public void OnFootButtonClick()
    {
        if (selectType != footButtonBG)
        {
            selectType = footButtonBG;

            composeTypeChange(ItemType.foot);
        }
    }
    public void OnRingButtonClick()
    {
        if (selectType != ringButtonBG)
        {
            selectType = ringButtonBG;

            composeTypeChange(ItemType.ring);
        }

    }
    public void OnBraButtonClick()
    {
        if (selectType != braButtonBG)
        {
            selectType = braButtonBG;
            composeTypeChange(ItemType.bracelet);
        }
    }
    public void OnWingButtonClick()
    {
        if (selectType != wingButtonBG)
        {
            selectType = wingButtonBG;
            composeTypeChange(ItemType.wing);
        }

    }
    public void OnItemButtonClick()
    {
        if (selectType != itemButtonBG)
        {
            selectType = itemButtonBG;
            composeTypeChange(ItemType.clip);
        }
    }
    public void OnComposeButtonClick()
    {
        AudioManager._instance.Play(AudioName.compose);
        PlayerState._instance.ComposeItem(selectedComposeItem.getItemCompose());
        OnComposeItemClick(selectedComposeItem);
        selectedComposeItem.UpdateButton();
    }

    public void OnComposeItemClick(UIComposeButtonEvent itemCompose)
    {
        selectedComposeItem = itemCompose;
        if (PlayerState._instance.GetPlayerBag().CheckCompose(itemCompose.getItemCompose()))
        {
            composeButton.SetState(UIButtonColor.State.Normal, true);
            composeButton.GetComponent<Collider>().enabled = true;
        }
        else
        {
            composeButton.SetState(UIButtonColor.State.Disabled, false);
            composeButton.GetComponent<Collider>().enabled = false;

        }
        ItemInfo info = ItemList.getItem(itemCompose.getItemCompose().ResultItem.itemID);
        if (ItemInfo.IsEquep(info.type))
        {
            paraInfoLabel.text = string.Format(
            "{0}\n\n{1}\n\n STR  :  {2:##}  INT  :  {3:##}\n DEX  :  {4:##}  CON  :  {5:##}\n LUK  :  {6:##}",
            info.name, info.descript, info.STR, info.INT, info.DEX, info.CON, info.LUK);
        }
        else
        {
            paraInfoLabel.text = string.Format(
            "{0}\n\n{1}",
            info.name, info.descript);
        }



    }
    #endregion
    #region UI Action
    void clearSelectType()
    {
        weapenButtonBG.defaultColor = Color.white;
        headButtonBG.defaultColor = Color.white;
        neckButtonBG.defaultColor = Color.white;
        coadButtonBG.defaultColor = Color.white;
        footButtonBG.defaultColor = Color.white;
        ringButtonBG.defaultColor = Color.white;
        braButtonBG.defaultColor = Color.white;
        wingButtonBG.defaultColor = Color.white;
        itemButtonBG.defaultColor = Color.white;
    }
    void Show()
    {
        if (!isShowPanel)
        {

            this.gameObject.SetActive(true);
            uiRoot.CloseAllWindows();
            PlayerState._instance.ChangeAction(PlayerState.PlayerAction.Talking);
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            PlayerState._instance.ChangeAction(PlayerState.PlayerAction.Free);
            StartCoroutine(HidePanel());
        }
    }
    void composeTypeChange(ItemType type)
    {
        composeItemList = GameController._instance.GetItemComposeList(type);
        composeButton.SetState(UIButtonColor.State.Disabled, false);
        composeButton.GetComponent<Collider>().enabled = false;

        UpdatePanel(type);
        clearSelectType();
        selectType.defaultColor = Color.green;
    }
    #endregion
    #region Panel CUTIN/OUT
    IEnumerator HidePanel()
    {

        this.GetComponentInChildren<UITweener>().PlayReverse();
        yield return new WaitForSeconds(this.transform.GetComponentInChildren<UITweener>().duration);
        this.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        this.GetComponentInChildren<UITweener>().PlayForward();
        yield return new WaitForSeconds(0.05f);
        isShowPanel = true;
    }
    #endregion
}

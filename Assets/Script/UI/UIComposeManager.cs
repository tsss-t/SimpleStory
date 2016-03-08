using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIComposeManager : MonoBehaviour
{

    #region para
    #region editor
    public static UIComposeManager _instance;

    public GameObject composeItemTwoClipPrefab;
    public GameObject composeItemThreeClipPrefab;
    public GameObject composeItemFourClipPrefab;
    #endregion

    bool isShowPanel;
    UIComposeButtonEvent selectedComposeItem;
    List<GameObject> composeShowList;
    List<ItemCompose> composeItemList;
    UIController uiRoot;

    #region UI
    UIGrid composeItemsGrid;
    UILabel paraInfoLabel;
    UIButton composeButton;

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
        composeTypeChange(ItemType.weapon);
    }
    public void OnHeadButtonClick()
    {
        composeTypeChange(ItemType.head);

    }
    public void OnNeckButtonClick()
    {
        composeTypeChange(ItemType.necklace);

    }
    public void OnCoadButtonClick()
    {
        composeTypeChange(ItemType.body);

    }

    public void OnFootButtonClick()
    {
        composeTypeChange(ItemType.foot);

    }
    public void OnRingButtonClick()
    {
        composeTypeChange(ItemType.ring);

    }
    public void OnBraButtonClick()
    {
        composeTypeChange(ItemType.bracelet);

    }
    public void OnWingButtonClick()
    {
        composeTypeChange(ItemType.wing);

    }
    public void OnItemButtonClick()
    {
        composeTypeChange(ItemType.drug);

    }
    public void OnComposeButtonClick()
    {
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

        paraInfoLabel.text = string.Format(
            "アイテム名：{0}\nアイテム説明：{1}\nアイテム属性：\n STR  :  {2:##}  INT  :  {3:##}\n DEX  :  {4:##}  CON  :  {5:##}\n LUK  :  {6:##}",
            info.name, info.descript,info.STR,info.INT,info.DEX,info.CON,info.LUK);

    }
    #endregion
    #region UI Action
    void Show()
    {
        if (!isShowPanel)
        {
            this.gameObject.SetActive(true);
            uiRoot.CloseAllWindows();
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
    void composeTypeChange(ItemType type)
    {
        composeItemList = GameController._instance.GetItemComposeList(type);
        UpdatePanel(type);
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

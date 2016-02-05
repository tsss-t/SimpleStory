using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class UICommunicationManager : MonoBehaviour
{
    #region para
    bool isShowPanel;
    private PlayerState playerState;
    public GameObject NPC;
    private NPCManager npcManager;
    private Dictionary<int, GameObject> NPCDictionary;
    private int selectNPCID;

    #region UI
    UIController mainControllerUI;

    UILabel talkLabel;
    UIButton shopButton;
    UIButton questButton;
    UIBagManager bagManagerUI;

    GameObject containItems;

    UINPCQuestManager npcQuestManagerUI;
    #endregion

    #endregion
    #region Start
    // Use this for initialization
    void Start()
    {
        isShowPanel = false;
        npcManager = NPC.GetComponent<NPCManager>();
        NPCDictionary = npcManager.GetNPCDctionary();
        playerState = PlayerState.GamePlayerState;

        mainControllerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).GetComponent<UIController>();

        foreach (KeyValuePair<int, GameObject> item in NPCDictionary)
        {
            item.Value.GetComponent<NPCInfomation>().CommunicationStart += CommunicationTalk;
        }

        #region UI
        containItems = transform.Find("TalkContainer").Find("Scroll View").Find("Items").gameObject;


        talkLabel = containItems.transform.Find("TalkLabel").GetComponent<UILabel>();
        shopButton = containItems.transform.Find("ShopButton").GetComponent<UIButton>();
        questButton = containItems.transform.Find("QuestButton").GetComponent<UIButton>();
        bagManagerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("EquepMenu").GetComponent<UIBagManager>();
        npcQuestManagerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("NPCQuestPanel").GetComponent<UINPCQuestManager>();
        #endregion

        gameObject.SetActive(false);
    }
    #endregion
    #region delegate
    void CommunicationTalk(int NPCNumber)
    {
        selectNPCID = NPCNumber;
        npcQuestManagerUI.SetSelectedShopID(selectNPCID);
        if (NPCDictionary[selectNPCID].GetComponent<NPCInfomation>().GetNPCType()[CommunicationType.Talk])
        {
            showTalk(npcManager.GetNPCInfo(selectNPCID).talkInfomation);
            if (NPCDictionary[selectNPCID].GetComponent<NPCInfomation>().GetNPCType()[CommunicationType.Shop])
            {
                shopButton.gameObject.SetActive(true);
            }
            else
            {
                shopButton.gameObject.SetActive(false);
            }
            if (NPCDictionary[selectNPCID].GetComponent<NPCInfomation>().GetNPCType()[CommunicationType.Quest])
            {
                questButton.gameObject.SetActive(true);
            }
            else
            {
                questButton.gameObject.SetActive(false);
            }

        }
        else
        {
            if (NPCDictionary[selectNPCID].GetComponent<NPCInfomation>().GetNPCType()[CommunicationType.Shop])
            {
                ShowShopMenu();
            }
            else if (NPCDictionary[selectNPCID].GetComponent<NPCInfomation>().GetNPCType()[CommunicationType.Quest])
            {
                ShowQuestMenu();
            }
        }
    }
    #endregion
    #region Init/Update
    void UpdateTalk(string talkInfo)
    {
        talkLabel.text = talkInfo;
    }
    #endregion
    #region UI EVENT
    void showTalk(string talkInfo)
    {
        playerState.ChangeAction(PlayerState.PlayerAction.Talking);
        UpdateTalk(talkInfo);
        OnOpenButtonClick();
    }
    public void OnOpenButtonClick()
    {
        Show();
    }

    public void OnCloseButtonClick()
    {
        Hide();
    }

    void Show()
    {
        if (!isShowPanel)
        {
            this.gameObject.SetActive(true);
            mainControllerUI.CloseAllWindows();
            containItems.GetComponent<UITable>().repositionNow = true;
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            playerState.ChangeAction(PlayerState.PlayerAction.Free);
            StartCoroutine(HidePanel());
        }
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
        yield return new WaitForSeconds(0.05f);
        isShowPanel = true;
    }


    public void OnShopButtonClick()
    {
        ShowShopMenu();
    }


    public void OnQuestButtonClick()
    {
        ShowQuestMenu();
    }
    #endregion
    #region UI Action
    void ShowShopMenu()
    {
        bagManagerUI.OpenPanelShop(selectNPCID);
    }
    void ShowQuestMenu()
    {
        OnCloseButtonClick();
        npcQuestManagerUI.OnOpenButtonClick();
    }
    #endregion
}

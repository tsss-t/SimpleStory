using UnityEngine;
using System.Collections.Generic;

public class UICommunicationManager : MonoBehaviour
{
    #region para
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
            if (NPCDictionary[selectNPCID].GetComponent<NPCInfomation>().GetNPCType()[CommunicationType.Quest])
            {
                questButton.gameObject.SetActive(true);
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
        mainControllerUI.CloseAllWindows();
        this.gameObject.SetActive(true);
        containItems.GetComponent<UITable>().repositionNow = true;
    }

    public void OnCloseButtonClick()
    {
        playerState.ChangeAction(PlayerState.PlayerAction.Free);
        this.gameObject.SetActive(false);
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

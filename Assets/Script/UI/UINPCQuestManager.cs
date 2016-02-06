using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UINPCQuestManager : MonoBehaviour
{
    #region para
    bool isShowPanel;

    PlayerState playerState;

    public GameObject questItemPrefab;
    public GameObject questItemOverPrefab;
    public GameObject NPC;
    private int selectShopID;
    /// <summary>
    /// NPCクエストリスト中のID
    /// </summary>
    private int selectQuestID;

    private NPCManager npcManager;
    private Dictionary<int, GameObject> questDictionary;
    #region UI
    GameObject containRoot;

    GameObject containQuestList;
    GameObject containQuestInfo;

    GameObject acceptButton;
    GameObject overButton;

    UIController mainControllerUI;
    UILabel questInfoLabel;
    #endregion

    #endregion
    #region Start
    // Use this for initialization
    void Start()
    {
        isShowPanel = false;
        playerState = PlayerState.GamePlayerState;
        npcManager = NPC.GetComponent<NPCManager>();

        questDictionary = new Dictionary<int, GameObject>();


        containRoot = GameObject.FindGameObjectWithTag(Tags.UIRoot);
        mainControllerUI = containRoot.GetComponent<UIController>();


        containQuestList = transform.Find("QuestList").Find("Scroll View").Find("Items").gameObject;
        containQuestInfo = transform.Find("QuestInfo").Find("Scroll View").Find("Items").gameObject;

        acceptButton = transform.Find("AcceptButton").gameObject;
        overButton = transform.Find("OverButton").gameObject;
        questInfoLabel = containQuestInfo.transform.Find("QuestInfoLabel").GetComponent<UILabel>();
        this.gameObject.SetActive(false);
    }
    #endregion
    #region Update
    void Init()
    {
        selectQuestID = -1;
        UpdateDate();
        UpdateQuestInfo();
    }
    GameObject go;
    void UpdateDate()
    {
        for (int i = 1; i <= questDictionary.Count; i++)
        {
            questDictionary.TryGetValue(i, out go);
            NGUITools.Destroy(go);
        }
        questDictionary.Clear();

        int j = 1;

        //NPCクエストのLoop
        for (int i = 0; i < npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList().Count; i++)
        {
            //まだ受け取っていない
            if (!CheckQuest(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[i].ID))
            {
                if (selectQuestID == -1)
                {
                    selectQuestID = i;
                }
                go = NGUITools.AddChild(containQuestList, questItemPrefab);
                go.transform.Find("QuestName").GetComponent<UILabel>().text = npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[i].name;
                go.gameObject.name = i.ToString();

                containQuestList.GetComponent<UITable>().repositionNow = true;
                questDictionary.Add(j++, go);
            }
            else if (IsOverStep(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[i].ID))
            {
                if (selectQuestID == -1)
                {
                    selectQuestID = i;
                }
                go = NGUITools.AddChild(containQuestList, questItemOverPrefab);
                go.transform.Find("QuestName").GetComponent<UILabel>().text = npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[i].name;
                go.gameObject.name = i.ToString();

                containQuestList.GetComponent<UITable>().repositionNow = true;
                questDictionary.Add(j++, go);
            }
        }
    }
    void UpdateQuestInfo()
    {
        acceptButton.SetActive(false);
        overButton.SetActive(false);
        if (selectQuestID != -1)
        {
            //新規クエスト
            if (!CheckQuest(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].ID))
            {
                if (PlayerState.GamePlayerState.GetPlayerQuest().GetAcceptQuestList().TryGetValue(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].ID, out quest))
                {
                    questInfoLabel.text = quest.GetStepNow().description;
                }
                else
                {
                    questInfoLabel.text = npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].description;
                }
                acceptButton.SetActive(true);
            }
            //クエスト報告
            else if (IsOverStep(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].ID))
            {
                int questID = npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].ID;
                quest = playerState.GetPlayerQuest().GetAcceptQuestList()[questID];
                questInfoLabel.text = quest.GetStepNow().description;

                overButton.SetActive(true);
            }
        }
        else
        {
            questInfoLabel.text = "";
        }
    }

    Quest quest;
    /// <summary>
    /// このクエストは既に受け取ったか？　受け取ったら：true
    /// </summary>
    /// <param name="questID">クエストリスト中のID</param>
    /// <returns></returns>
    bool CheckQuest(int questID)
    {
        if (PlayerState.GamePlayerState.GetPlayerQuest().GetAcceptQuestList().TryGetValue(questID, out quest))
        {
            if(quest.isAccept)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 完成した報告できるクエストですか
    /// </summary>
    /// <param name="questID">クエストリスト中のID</param>
    /// <returns></returns>
    bool IsOverStep(int questID)
    {
        PlayerState.GamePlayerState.GetPlayerQuest().GetAcceptQuestList().TryGetValue(questID, out quest);
        if (!quest.isOver)
        {
            if (quest.GetStepNow().count <= quest.count)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    #region 外部API
    public void SetSelectedShopID(int shopID)
    {
        selectShopID = shopID;
    }
    public void SetSelectedQuestID(int questID)
    {
        selectQuestID = questID;
    }
    #endregion


    #region UI Event
    public void OnQuestButtonClick()
    {
        UpdateQuestInfo();
    }
    public void OnAcceptButtonClick()
    {
        AcceptQuest();
    }
    public void OnOverButtonClick()
    {
        OverQuest();
    }
    public void OnCloseButtonClick()
    {
        Hide();
    }
    public void OnOpenButtonClick()
    {
        Show();
    }
    #endregion
    #region UI Action
    void Show()
    {
        if (!isShowPanel)
        {

            this.gameObject.SetActive(true);
            Init();
            mainControllerUI.CloseAllWindows();
            PlayerState.GamePlayerState.ChangeAction(PlayerState.PlayerAction.Talking);
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            PlayerState.GamePlayerState.ChangeAction(PlayerState.PlayerAction.Free);
            StartCoroutine(HidePanel());
        }
    }

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
    #endregion
    void AcceptQuest()
    {
        if (playerState.AcceptQuest(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].ID))
        {
            Init();
        }
        else
        {
            Debug.Log("任務受取る失敗！");
            //TODO:エラー処理
        }
    }
    void OverQuest()
    {
        if (playerState.OverQuest(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].ID))
        {
            Init();
        }
        else
        {
            Debug.Log("任務完成失敗！");
            //TODO:エラー処理
        }
    }
    #endregion
}

using UnityEngine;
using System.Collections.Generic;

public class UINPCQuestManager : MonoBehaviour
{
    #region para
    PlayerState playerState;

    public GameObject questIntemPrefab;
    public GameObject NPC;
    private int selectShopID;
    private int selectQuestID;

    private NPCManager npcManager;
    private Dictionary<int, GameObject> questDictionary;
    #region UI
    GameObject containRoot;

    GameObject containQuestList;
    GameObject containQuestInfo;

    UIController mainControllerUI;
    UILabel questInfoLabel;
    #endregion

    #endregion
    #region Start
    // Use this for initialization
    void Start()
    {
        playerState = PlayerState.GamePlayerState;
        npcManager = NPC.GetComponent<NPCManager>();

        questDictionary = new Dictionary<int, GameObject>();


        containRoot = GameObject.FindGameObjectWithTag(Tags.UIRoot);
        mainControllerUI = containRoot.GetComponent<UIController>();


        containQuestList = transform.Find("QuestList").Find("Scroll View").Find("Items").gameObject;
        containQuestInfo = transform.Find("QuestInfo").Find("Scroll View").Find("Items").gameObject;

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

        for (int i = 0; i < npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList().Count; i++)
        {

            //まだ受け取っていない
            if (!CheckQuest(npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[i].ID))
            {
                if (selectQuestID == -1)
                {
                    selectQuestID = i;
                }
                go = NGUITools.AddChild(containQuestList, questIntemPrefab);
                go.transform.Find("QuestName").GetComponent<UILabel>().text = npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[i].name;
                go.gameObject.name = i.ToString();

                containQuestList.GetComponent<UITable>().repositionNow = true;
                questDictionary.Add(j++, go);
            }
        }
    }
    void UpdateQuestInfo()
    {
        if (selectQuestID != -1)
        {
            questInfoLabel.text = npcManager.GetNPCDctionary()[selectShopID].GetComponent<NPCInfomation>().GetQuestList()[selectQuestID].description;
        }
        else
        {
            questInfoLabel.text = "";
        }

    }

    /// <summary>
    /// このクエストは既に受け取ったか？　受け取ったら：true
    /// </summary>
    /// <param name="QuestID">NPCのクエスト　クエストリスト中のID</param>
    /// <returns></returns>
    bool CheckQuest(int QuestID)
    {

        Quest quest;
        if (PlayerState.GamePlayerState.GetPlayerQuest().GetAcceptQuestList().TryGetValue(QuestID, out quest))
        {
            return true;
        }
        else
        {
            return false;
        }
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
    public void OnCloseButtonClick()
    {
        Hide();
    }
    public void OnOpenButtonClick()
    {
        mainControllerUI.CloseAllWindows();
        Show();
    }
    #endregion
    #region UI Action
    public void Show()
    {
        PlayerState.GamePlayerState.ChangeAction(PlayerState.PlayerAction.Talking);
        Init();
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        PlayerState.GamePlayerState.ChangeAction(PlayerState.PlayerAction.Free);
        this.gameObject.SetActive(false);
    }
    public void AcceptQuest()
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

    #endregion
}

using System.Collections.Generic;

public class PlayerQuest
{
    #region _instance
    private static PlayerQuest _instance;
    public static PlayerQuest nowPlayerQuest
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerQuest();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    #endregion
    #region para
    /// <summary>
    /// 受け取ったクエストのリスト、Keyはクエストリスト中のクエストID
    /// </summary>
    Dictionary<int, Quest> questAcceptList;
    #endregion

    #region 構造方法/初期化
    private PlayerQuest()
    {
        questAcceptList = new Dictionary<int, Quest>();
        Init();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Init()
    {
        //TODO:Load from server

        //TODO:Load from text

        //TODO:test Load

        questAcceptList.Add(QuestList.getQuest(1).ID, new Quest(13, 1, 0, QuestList.getQuest(1), false));
        questAcceptList.Add(QuestList.getQuest(2).ID, new Quest(21, 0, 0, QuestList.getQuest(2), false));
    }
    #endregion
    #region 外部API
    public Dictionary<int, Quest> GetAcceptQuestList()
    {
        return questAcceptList;
    }

    /// <summary>
    /// 任務をゲット
    /// </summary>
    /// <param name="info"></param>
    public bool AcceptQuest(QuestInfo info)
    {
        Quest quest;
        if (questAcceptList.TryGetValue(info.ID, out quest))
        {
            quest.isAccept = true;
        }
        else
        {
            questAcceptList.Add(info.ID, new Quest(info.ID, 1, 0, info, false));
            //TODO:サーバへ提出
        }
        return true;
    }
    /// <summary>
    /// 任務をゲット
    /// </summary>
    /// <param name="questID">クエストリスト中のID </param>
    public bool AcceptQuest(int questID)
    {
        Quest quest;
        if (questAcceptList.TryGetValue(questID, out quest))
        {
            quest.isAccept = true;
        }
        else
        {
            questAcceptList.Add(questID, new Quest(questID, 1, 0, QuestList.getQuest(questID), false));

            //TODO:サーバへ提出
        }
        return true;
    }


    public void DoQuest(QuestType type, int ID)
    {
        foreach (Quest item in questAcceptList.Values)
        {
            if (item.isAccept&& item.GetStepNow().questType == type && item.GetStepNow().targetID == ID && item.count < item.info.GetStepCount())
            {
                item.count++;
            }
        }
    }
    public bool OverQuest(int questID)
    {
        if (questAcceptList[questID].stepNow < questAcceptList[questID].info.GetStepCount())
        {
            questAcceptList[questID].stepNow++;
            questAcceptList[questID].isAccept = false;
            questAcceptList[questID].count = 0;
        }
        else if (questAcceptList[questID].stepNow == questAcceptList[questID].info.GetStepCount())
        {
            questAcceptList[questID].isOver = true;
        }

        return true;
    }
    public bool IsOverStep(int questID)
    {
        if (questAcceptList[questID].count == questAcceptList[questID].GetStepNow().count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}

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
            return false;
        }
        else
        {
            questAcceptList.Add(info.ID, new Quest(info.ID, 1, 0, info, false));

            //TODO:サーバへ提出

            return true;
        }

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
            return false;
        }
        else
        {
            questAcceptList.Add(questID, new Quest(questID, 1, 0, QuestList.getQuest(questID), false));

            //TODO:サーバへ提出

            return true;
        }
    }


    public void DoQuest(QuestType type, int ID)
    {
        foreach (Quest item in questAcceptList.Values)
        {
            if (item.info.step[item.stepNow].questType == type && item.info.step[item.stepNow].targetID == ID && item.count < item.info.step[item.stepNow].count)
            {
                
                item.count++;
            }
        }
    }
    #endregion
}

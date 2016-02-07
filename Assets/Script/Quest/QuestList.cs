using System.Collections.Generic;

public static class QuestList
{
    private static Dictionary<int, QuestInfo> _instance;
    private static Dictionary<int, QuestInfo> getQuestList
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Dictionary<int, QuestInfo>();
                LoadItemData();

                return _instance;
            }
            else
            {
                return _instance;
            }
        }
    }
    private static void LoadItemData()
    {
        //TODO:LOAD FROM SERVER

        //tset data
        _instance = GameController._instance.LoadQuestList();
    }
    /// <summary>
    /// クエストリストからクエストの情報を貰う
    /// </summary>
    /// <param name="questID">リスト中のクエストID（リストのインデックスではない）</param>
    /// <returns></returns>
    public static QuestInfo getQuest(int questID)
    {
        QuestInfo questInfo;
        QuestList.getQuestList.TryGetValue(questID, out questInfo);
        return questInfo;
    }


}

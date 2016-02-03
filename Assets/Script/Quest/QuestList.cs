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
        _instance.Add(1, new QuestInfo(1, "Kill Boss", "aaaaaaaaaaaaaaaaaaaaaaaaaaaa", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the boss", QuestType.findNPC, 1, 1), QuestInfo.makeStep("You must kill the boss", QuestType.killEnemy, 1, 1) }));
        _instance.Add(2, new QuestInfo(2, "Find NPC", "ddddddddddddddddddddddddddddd", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the NPC", QuestType.findNPC, 2, 1) }));
        _instance.Add(3, new QuestInfo(3, "Find Item", "cccccccccccccccccccccccccccc", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the Item", QuestType.findItem, 1, 5) }));
        _instance.Add(4, new QuestInfo(4, "Find Item1", "abc", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the Item1", QuestType.findItem, 2, 5) }));
        _instance.Add(5, new QuestInfo(5, "Find Item2", "cba", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the Item2", QuestType.findItem, 3, 5) }));


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

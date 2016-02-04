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
        _instance.Add(1, new QuestInfo(1, "Kill Boss", "this is the frist quest", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the NPC right", QuestType.findNPC, 1, 1,100,100), QuestInfo.makeStep("You must kill the boss", QuestType.killEnemy, 1, 1,1000,1000,5) }));
        _instance.Add(2, new QuestInfo(2, "Find NPC", "this is the second quest", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the NPC left", QuestType.findNPC, 2, 1,100,100) }));
        _instance.Add(3, new QuestInfo(3, "Find Item", "quest3", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the Item", QuestType.findItem, 1, 5,100,100) }));
        _instance.Add(4, new QuestInfo(4, "Find Item1", "quest4", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the Item1", QuestType.findItem, 2, 5, 100, 100) }));
        _instance.Add(5, new QuestInfo(5, "Find Item2", "quest5", new List<QuestInfo.QuestStep>() { QuestInfo.makeStep("You must find the Item2", QuestType.findItem, 3, 5, 100, 100) }));


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

using System.Collections.Generic;

/// <summary>
/// クエスト自体の情報
/// </summary>
public class QuestInfo
{
    public int ID;
    public string name;
    public string description;
    public struct QuestStep
    {
        public string description;
        public QuestType questType;
        public int count;
        public int targetID;
    }
    public List<QuestStep> step;
    /// <summary>
    /// 初期化、
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="step"></param>
    public QuestInfo(int ID,string name,string description, List<QuestStep> step)
    {
        this.ID = ID;
        this.step = step;
        this.name = name;
        this.description = description;
    }

    

    public static QuestStep makeStep(string description, QuestType questType,int targetID, int count)
    {
        QuestStep step;
        step.description = description;
        step.questType = questType;
        step.count = count;
        step.targetID = targetID;
        return step;
    }

}

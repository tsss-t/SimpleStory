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
        public Award award;
    }
    public struct Award
    {
        public int EXP;
        public int money;
        public int itemID;
    }
    List<QuestStep> step;
    /// <summary>
    /// 初期化、
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="step"></param>
    public QuestInfo(int ID, string name, string description, List<QuestStep> step)
    {
        this.ID = ID;
        this.step = step;
        this.name = name;
        this.description = description;
    }



    public static QuestStep makeStep(string description, QuestType questType, int targetID, int count, int awardEXP, int awardMoney, int awardItemID)
    {
        QuestStep step;
        step.description = description;
        step.questType = questType;
        step.count = count;
        step.targetID = targetID;
        step.award = new Award() { EXP = awardEXP, money = awardMoney, itemID = awardItemID };
        return step;

    }
    public static QuestStep makeStep(string description, QuestType questType, int targetID, int count, int awardEXP, int awardMoney)
    {
        QuestStep step;
        step.description = description;
        step.questType = questType;
        step.count = count;
        step.targetID = targetID;
        step.award = new Award() { EXP = awardEXP, money = awardMoney, itemID = -1 };
        return step;
    }

    /// <summary>
    /// ステップ情報を貰う
    /// </summary>
    /// <param name="stepNumber">1からのステップインデックス</param>
    /// <returns></returns>
    public QuestStep GetStep(int stepNumber)
    {
        return step[stepNumber - 1];
    }

    public int GetStepCount()
    {
        return step.Count;
    }


}

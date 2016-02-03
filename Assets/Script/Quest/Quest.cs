using System.Collections.Generic;
public enum QuestType
{
    findNPC, findItem, killEnemy
}
/// <summary>
/// プレイヤーのクエスト情報
/// </summary>
public class Quest
{
    public int ID;
    public int count;
    public int stepNow;
    public QuestInfo info;
    public bool isOver;
    /// <summary>
    /// プレイヤーのクエスト情報
    /// </summary>
    /// <param name="ID">データベース”QuestUser”中の既に進行中・完成したクエストID</param>
    /// <param name="stepNow">今進行中のステップ</param>
    /// <param name="info">任務情報（QuestInfoテーブルと対応）</param>
    /// <param name="isOver">進行中 or 完成</param>
    public Quest(int ID, int stepNow,int count, QuestInfo info,bool isOver)
    {
        this.ID = ID;
        this.count = count;
        this.stepNow = stepNow;
        this.info = info;
        this.isOver = isOver;
    }
}

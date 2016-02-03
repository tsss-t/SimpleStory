using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ActionEvent
{
    public int actionProb;//アクション発生の確率
    public string actionTrigerName;//動画発生のTrigeｒの名前
    public ActionType actionType;//アクション種類
}
public static class ActionEventEM
{
    /// <summary>
    /// リストからアクション発生の確率リストを貰う
    /// </summary>
    /// <param name="actionList"></param>
    /// <returns></returns>
    public static int[] GetProbs(this ActionEvent[] actionList)
    {
        int[] probs = new int[actionList.Length];
        for (int i = 0; i < actionList.Length; i++)
        {
            probs[i] = actionList[i].actionProb;

        }
        return probs;
    }
    /// <summary>
    /// リストから攻撃のアクションを貰う
    /// </summary>
    /// <param name="actionList">全てアクションのリスト</param>
    /// <returns></returns>
    public static ActionEvent[] GetNormalActionEvents(this ActionEvent[] actionList)
    {
        ActionType[] normalActionType =new ActionType[2]{ActionType.idel, ActionType.run };
        return actionList.GetActionEvents(normalActionType);

    }
    /// <summary>
    /// リストから普段のアクションを貰う
    /// </summary>
    /// <param name="actionList"></param>
    /// <returns></returns>
    public static ActionEvent[] GetAttackActionEvents(this ActionEvent[] actionList)
    {
        ActionType[] normalActionType = new ActionType[1] { ActionType.attack };
        return actionList.GetActionEvents(normalActionType);
    }
    /// <summary>
    /// リストから被攻撃のアクションを貰う
    /// </summary>
    /// <param name="actionList"></param>
    /// <returns></returns>
    public static ActionEvent[] GetHitActionEvents(this ActionEvent[] actionList)
    {
        ActionType[] hitActionType = new ActionType[1] { ActionType.hit };
        return actionList.GetActionEvents(hitActionType);

    }
    public static ActionEvent[] GetDieActionEvents(this ActionEvent[] actionList)
    {
        ActionType[] dieActionType = new ActionType[1] { ActionType.die };
        return actionList.GetActionEvents(dieActionType);

    }
    /// <summary>
    /// アクション種類によってリストから貰う
    /// </summary>
    /// <param name="actionList">選択アクションリスト</param>
    /// <param name="selectTypes">必要なタイプ</param>
    /// <returns></returns>
    public static ActionEvent[] GetActionEvents(this ActionEvent[] actionList,ActionType[] selectTypes)
    {
        List<ActionEvent> normalActionEvents = new List<ActionEvent>();
        foreach (ActionEvent actionEvent in actionList)
        {
            foreach(ActionType selectType in selectTypes)
            {
                if (actionEvent.actionType == selectType)
                    normalActionEvents.Add(actionEvent);
            }

        }
        return normalActionEvents.ToArray();
    }
}

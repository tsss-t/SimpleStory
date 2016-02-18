using UnityEngine;
using System.Collections;

public class UIQuestButtonEvent : MonoBehaviour
{

    UIQuestManager questManager;
    Quest quest;
    // Use this for initialization
    void Start()
    {
        questManager = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("QuestMenu").GetComponent<UIQuestManager>();
        PlayerState._instance.GetPlayerQuest().GetAcceptQuestList().TryGetValue(int.Parse(transform.parent.name), out quest);
    }


    public void OnButtonClick()
    {
        questManager.SetSelectQuest(quest);
        questManager.OnQuestButtonClick();
    }

}

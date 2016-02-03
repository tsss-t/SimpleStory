using UnityEngine;
using System.Collections;

public class UINPCQuestButtonEvent : MonoBehaviour
{

    UINPCQuestManager npcQuestManagerUI;
    // Use this for initialization
    void Start()
    {
        npcQuestManagerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("NPCQuestPanel").GetComponent<UINPCQuestManager>();
    }
    public void OnItemClick()
    {
        npcQuestManagerUI.SetSelectedQuestID(int.Parse(this.transform.parent.name));
        npcQuestManagerUI.OnQuestButtonClick();
    }


}

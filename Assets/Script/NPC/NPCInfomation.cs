using UnityEngine;
using System.Collections.Generic;
using System.Collections;



public class NPCInfomation : MonoBehaviour
{
    public int NPCID;
    public string talkInfomation;
    public List<QuestInfo> questList;

    public delegate void Comunication(int NPCID);
    public event Comunication CommunicationStart;
    public Dictionary<CommunicationType, bool> NPCType;
    void Awake()
    {
        NPCType = new Dictionary<CommunicationType, bool>() { { CommunicationType.Talk, true }, { CommunicationType.Shop, false }, { CommunicationType.Quest, false } };
        questList = new List<QuestInfo>();
        talkInfomation = "";
    }

    public void SetQuest(List<QuestInfo> questList)
    {
        this.questList = questList;
    }

    public int Communication()
    {
        CommunicationStart(this.NPCID);
        return NPCID;
    }
    public void SetNPCType(Dictionary<CommunicationType, bool> NPCType)
    {
        this.NPCType = NPCType;
    }
    public Dictionary<CommunicationType, bool> GetNPCType()
    {
        return NPCType;
    }
    public List<QuestInfo> GetQuestList()
    {
        return questList;
    }

    void OnDestroy()
    {
        CommunicationStart -= UICommunicationManager._instance.CommunicationTalk;
    }

}

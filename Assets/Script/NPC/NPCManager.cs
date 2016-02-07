using UnityEngine;
using System.Collections.Generic;

public enum CommunicationType
{
    Talk,Quest, Shop
}

public class NPCManager : MonoBehaviour {

    #region para
    private Dictionary<int, GameObject> NPCDictionary;
    #endregion

    #region test data
    public GameObject NPC1;
    public GameObject NPC2;
    #endregion

    #region Start
    // Use this for initialization
    void Start () {
        NPCDictionary = new Dictionary<int, GameObject>();

        Init();
    }
    #endregion
    #region 初期化
    void Init()
    {
        LoadData();
    }
    Dictionary<int, Dictionary<CommunicationType, bool>> npcsType;
    void LoadData()
    {
        GameObject[] NPClist = GameObject.FindGameObjectsWithTag(Tags.NPC);
        for (int i =0; i < NPClist.Length; i++)
        {
            NPCDictionary.Add(NPClist[i].GetComponent<NPCInfomation>().NPCID, NPClist[i]);

        }
        npcsType = GameController._instance.LoadNpcType();
        foreach (KeyValuePair<int,Dictionary<CommunicationType,bool>> npc in npcsType)
        {
            NPCDictionary[npc.Key].GetComponent<NPCInfomation>().SetNPCType(npc.Value);
            NPCDictionary[npc.Key].GetComponent<NPCInfomation>().SetQuest(GameController._instance.LoadNpcQuest(npc.Key));
        }
    }
    #endregion

    #region 外部API
    public Dictionary<int, GameObject> GetNPCDctionary()
    {
        return NPCDictionary;
    }
    public NPCInfomation GetNPCInfo(int NPCID)
    {
        GameObject NPC;
        NPCDictionary.TryGetValue(NPCID, out NPC);
        return NPC.GetComponent<NPCInfomation>();
    }
    #endregion
}

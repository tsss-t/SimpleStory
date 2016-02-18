using UnityEngine;
using System.Collections.Generic;

public enum CommunicationType
{
    Talk, Quest, Shop
}

public class NPCManager : MonoBehaviour
{
    public static NPCManager _instance;

    #region para
    private Dictionary<int, NPCDATA> NPCDictionary;
    Dictionary<int, GameObject> floorNPCDictionary;
    #endregion

    #region test data
    public GameObject[] NPCprefabList;

    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        _instance = this;
        NPCDictionary = new Dictionary<int, NPCDATA>();

        Init();
    }
    #endregion
    #region 初期化
    void Init()
    {
        LoadData();
    }
    private GameObject npcTemp;
    void LoadData()
    {
        floorNPCDictionary = new Dictionary<int, GameObject>();
        //GameObject.Find(Tags.sceneManager).GetComponent<SceneManager>().floorNum
        NPCDictionary = GameController._instance.LoadNPCData();



        foreach (NPCDATA npc in NPCDictionary.Values)
        {
            if(npc.FloorNum== SceneManager._instance.floorNum)
            {
                npcTemp = Instantiate(NPCprefabList[npc.PrefabID], npc.Position, Quaternion.Euler(0,npc.Euler, 0)) as GameObject;
                npcTemp.name = npc.Name;
                npcTemp.GetComponent<NPCInfomation>().NPCID = npc.Id;
                npcTemp.GetComponent<NPCInfomation>().talkInfomation = npc.TalkInfo;
                npcTemp.GetComponent<NPCInfomation>().SetNPCType(npc.NPCtype1);
                npcTemp.GetComponent<NPCInfomation>().SetQuest(npc.QuestList);
                npcTemp.tag = Tags.NPC;
                floorNPCDictionary.Add(npc.Id, npcTemp);
            }
        }
    }
    #endregion

    #region 外部API
    public Dictionary<int, GameObject> GetNPCDctionary()
    {
        return floorNPCDictionary;
    }
    public NPCInfomation GetFloorNPCInfo(int NPCID)
    {
        GameObject NPC;
        floorNPCDictionary.TryGetValue(NPCID, out NPC);
        return NPC.GetComponent<NPCInfomation>();
    }
    public NPCDATA GetNPCInfo(int NPCID)
    {
        return NPCDictionary[NPCID];
    }

    public Dictionary<int , Item> getShopItem(int shopID)
    {
        return NPCDictionary[shopID].SellItemList;
    }
    #endregion
}
public class NPCDATA
{
    int id;
    string name;
    int prefabID;
    Dictionary<CommunicationType, bool> NPCtype;
    Vector3 position;
    int floorNum;
    string talkInfo;
    List<QuestInfo> questList;
    Dictionary<int, Item> sellItemList;
    int euler;

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }



    public Dictionary<CommunicationType, bool> NPCtype1
    {
        get
        {
            return NPCtype;
        }

        set
        {
            NPCtype = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public int FloorNum
    {
        get
        {
            return floorNum;
        }

        set
        {
            floorNum = value;
        }
    }

    public string TalkInfo
    {
        get
        {
            return talkInfo;
        }

        set
        {
            talkInfo = value;
        }
    }

    public List<QuestInfo> QuestList
    {
        get
        {
            return questList;
        }

        set
        {
            questList = value;
        }
    }

    public Dictionary<int, Item> SellItemList
    {
        get
        {
            return sellItemList;
        }

        set
        {
            sellItemList = value;
        }
    }

    public int Euler
    {
        get
        {
            return euler;
        }

        set
        {
            euler = value;
        }
    }

    public int PrefabID
    {
        get
        {
            return prefabID;
        }

        set
        {
            prefabID = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public NPCDATA(int id,string name,int prefabID ,Dictionary<CommunicationType, bool> nPCtype, int positionX,int  positionY,int positionZ, int floorNum, string talkInfo, List<QuestInfo> questList, Dictionary<int, Item> sellItemList,int euler)
    {
        this.id = id;
        this.name = name;
        this.prefabID = prefabID;
        NPCtype = nPCtype;
        this.position = new Vector3(positionX,positionY,positionZ);
        this.floorNum = floorNum;
        this.talkInfo = talkInfo;
        this.questList = questList;
        this.sellItemList = sellItemList;
        this.euler = euler;
    }


}
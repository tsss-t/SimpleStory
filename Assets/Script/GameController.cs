using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// セーブデータクラス
/// </summary>
public class GameData
{
    //暗号キー
    public string key;

    //セーブデータの内容
    public string PlayerAcceptData;
    public string PlayerBag;
    public string PlayerState;
    public GameData()
    {

    }
    public GameData(string PlayerAcceptData, string PlayerBag, string PlayerState)
    {
        this.PlayerAcceptData = PlayerAcceptData;
        this.PlayerBag = PlayerBag;
        this.PlayerState = PlayerState;
    }
}
public class GameController : MonoBehaviour
{


    #region paramater
    private const string dataFileName = "tankyWarData.dat";//セーブデータの名前

    private XmlSaver xs;
    public GameData gameData;
    string saveString;
    //PlayerState playerState;
    public TextAsset playerStateData;
    public TextAsset playerTypeData;
    public TextAsset bagData;
    public TextAsset itemListData;
    public TextAsset playerQuestData;
    public TextAsset questListData;
    public TextAsset NPCData;
    public TextAsset skillData;
    public TextAsset openingData;
    #endregion
    #region Awake
    public static GameController _instance;
    void Awake()
    {
        _instance = this;
        //Application.targetFrameRate = 45;

        xs = new XmlSaver();
        gameData = new GameData();
        gameData.key = SystemInfo.deviceUniqueIdentifier;
        InitSave();
        Load();
    }
    #endregion
    #region Save
    public void WriteData()
    {
        string gameDataFile = GetDataPath() + "/" + dataFileName;
        string dataString = xs.SerializeObject(gameData, typeof(GameData));
        xs.CreateXML(gameDataFile, dataString);
    }
    void Save()
    {
        SaveAcceptQuest();
        SaveBag();  
        SavePlayerState();
        WriteData();
    }
    void InitSave()
    {
        gameData.PlayerAcceptData = playerQuestData.ToString();
        gameData.PlayerBag = bagData.ToString();
        gameData.PlayerState = playerStateData.ToString();
        WriteData();
    }
    #region Item
    const string bagUper = "ID,isEqueped,cout,itemID";
    void SaveBag()
    {
        saveString= "";
        foreach (KeyValuePair<int,Item> item in PlayerState.GamePlayerState.GetPlayerBag().dictionBag)
        {
            saveString+=string.Format("{0},{1},{2},{3}\n",item.Key,item.Value.isEqueped?"1":"0",item.Value.count,item.Value.info.id);
        }
        gameData.PlayerBag = string.Format("{0}\n{1}", bagUper, saveString);
    }
    #endregion
    #region Quest
    const string questUper = "ID,StepNow,overCount,QuestID,isOver,isAccept";
    void SaveAcceptQuest()
    {
        saveString = "";
        foreach (KeyValuePair<int,Quest> item in PlayerState.GamePlayerState.GetPlayerQuest().GetAcceptQuestList())
        {
            saveString += string.Format("{0},{1},{2},{3},{4},{5}\n", item.Key,item.Value.stepNow,item.Value.count,item.Value.ID,item.Value.isOver?"1":"0",item.Value.isAccept?"1":"0");
        }
        gameData.PlayerAcceptData = string.Format("{0}\n{1}", questUper, saveString);
    }
    #endregion
    #region PlayerState
    const string playerStateUper = "EXP,money,isWalk,Type";
    void SavePlayerState()
    {
        gameData.PlayerState = string.Format("{0}\n{1},{2},{3},{4}\n", playerStateUper, PlayerState.GamePlayerState.EXP, PlayerState.GamePlayerState.money, PlayerState.GamePlayerState.isWalk ? 1 : 0, (int)PlayerState.GamePlayerState.type);
    }
    #endregion
    #region Skill
    void SaveSkill()
    {

    }

    #endregion
    #endregion
    #region Load
    public void Load()
    {
        string gameDataFile = GetDataPath() + "/" + dataFileName;
        if (xs.hasFile(gameDataFile))
        {
            string dataString = xs.LoadXML(gameDataFile);
            GameData gameDataFromXML = xs.DeserializeObject(dataString, typeof(GameData)) as GameData;

            //是合法存档//
            if (gameDataFromXML.key == gameData.key)
            {
                gameData = gameDataFromXML;
            }
            //是非法拷贝存档//
            else
            {
                InitSave();
                Load();
            }
        }
        //セーブデータが存在しません
        else
        {
            InitSave();
            Load();
        }
    }
    #region Item

    Dictionary<int, ItemInfo> itemList;
    ItemInfo itemInfo;
    /// <summary>
    /// ファイルからアイテムのデータを導入
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, ItemInfo> LoadItemList()
    {
        string[] proArray;
        string[] dataArray = itemListData.ToString().Split('\n');

        itemList = new Dictionary<int, ItemInfo>();

        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                if (ItemInfo.IsEquep((ItemType)int.Parse(proArray[1])))
                {
                    itemInfo = new ItemInfo(int.Parse(proArray[0]),
                        (ItemType)int.Parse(proArray[1]),
                        proArray[2],
                        proArray[3],
                        int.Parse(proArray[4]),
                        int.Parse(proArray[5]),
                        int.Parse(proArray[6]),
                        int.Parse(proArray[7]),
                        int.Parse(proArray[8]),
                        int.Parse(proArray[11]),
                        int.Parse(proArray[12]),
                        int.Parse(proArray[13]),
                        int.Parse(proArray[14]),
                        int.Parse(proArray[15]),
                        int.Parse(proArray[16])
                        );
                }
                else
                {
                    itemInfo = new ItemInfo(
                        int.Parse(proArray[0]),
                        proArray[2],
                        proArray[3],
                        int.Parse(proArray[9]),
                        int.Parse(proArray[10]),
                        int.Parse(proArray[16])
                        );
                }
                itemList.Add(itemInfo.id, itemInfo);
            }
        }
        return itemList;
    }

    Dictionary<int, Item> bagList;
    Item item;
    /// <summary>
    /// ファイルからバッグのデータを導入
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Item> LoadBag()
    {
        string[] proArray;
        string[] dataArray = gameData.PlayerBag.ToString().Split('\n');
        bagList = new Dictionary<int, Item>();

        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                if (ItemInfo.IsEquep(ItemList.getItem(int.Parse(proArray[3])).type))
                {
                    try
                    {
                        item = new Item(int.Parse(proArray[0]), proArray[1] == "0" ? false : true, ItemList.getItem(int.Parse(proArray[3])));
                    }
                    catch
                    {
                        Debug.Log(dataArray[i]);
                        Debug.Log(int.Parse(proArray[0]));
                        Debug.Log(proArray[1] == "0" ? false : true);
                        Debug.Log(int.Parse(proArray[3]));

                    }

                }
                else
                {
                    item = new Item(int.Parse(proArray[0]), int.Parse(proArray[2]), ItemList.getItem(int.Parse(proArray[3])));
                }
                bagList.Add(item.id, item);
            }
        }
        return bagList;
    }
    #endregion
    #region Quest
    QuestInfo info;
    List<QuestInfo.QuestStep> stepList;
    Dictionary<int, QuestInfo> questList;
    public Dictionary<int, QuestInfo> LoadQuestList()
    {
        string[] proArray;
        string[] dataArray = questListData.ToString().Split('\n');
        questList = new Dictionary<int, QuestInfo>();
        int maxIndex;

        for (int minIndex = 1; minIndex < dataArray.Length; minIndex++)
        {
            if (dataArray[minIndex] != "")
            {
                for (maxIndex = minIndex; maxIndex < dataArray.Length; maxIndex++)
                {
                    if (dataArray[maxIndex].Split(',')[0] != dataArray[minIndex].Split(',')[0])
                    {
                        break;
                    }
                }
                stepList = new List<QuestInfo.QuestStep>();
                for (int i = minIndex; i < maxIndex; i++)
                {
                    proArray = dataArray[i].Split(',');
                    stepList.Add(QuestInfo.makeStep(proArray[3], (QuestType)int.Parse(proArray[4]), int.Parse(proArray[5]), int.Parse(proArray[6]), int.Parse(proArray[7]), int.Parse(proArray[8]),int.Parse(proArray[9])));
                }
                proArray = dataArray[minIndex].Split(',');
                info = new QuestInfo(int.Parse(proArray[0]), proArray[1], proArray[2], stepList);
                questList.Add(info.ID, info);
                minIndex = maxIndex - 1;
            }
        }
        return questList;
    }
    Dictionary<int, Quest> acceptQuestList;
    public Dictionary<int, Quest> LoadAcceptQuest()
    {
        string[] proArray;
        string[] dataArray = gameData.PlayerAcceptData.ToString().Split('\n');
        acceptQuestList = new Dictionary<int, Quest>();
        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                acceptQuestList.Add(int.Parse(proArray[3]),
                    new Quest(
                        int.Parse(proArray[0]),
                        int.Parse(proArray[1]),
                        int.Parse(proArray[2]),
                        QuestList.getQuest(int.Parse(proArray[3])),
                        proArray[4].Equals("0") ? false : true,
                        proArray[5].Equals("0") ? false : true));
            }
        }
        return acceptQuestList;
    }
    #endregion
    #region NPC
    Dictionary<int, Item> shopItemList;
    Item shopItem;
    public Dictionary<int, Item> LoadShopItem(int shopID)
    {
        string[] itemArray;
        string[] proArray;
        string[] dataArray = NPCData.ToString().Split('\n');
        shopItemList = new Dictionary<int, Item>();
        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                if (int.Parse(proArray[0]) == shopID)
                {
                    itemArray = proArray[6].Split('|');
                    for (int j = 0; j < itemArray.Length; j++)
                    {
                        if (ItemInfo.IsEquep(ItemList.getItem(int.Parse(itemArray[j])).type))
                        {
                            item = new Item(j + 1, false, ItemList.getItem(int.Parse(itemArray[j])));
                        }
                        else
                        {
                            item = new Item(j + 1, 1, ItemList.getItem(int.Parse(itemArray[j])));
                        }
                        shopItemList.Add(item.id, item);
                    }
                }
            }
        }
        return shopItemList;
    }


    List<QuestInfo> npcQuestList;
    public List<QuestInfo> LoadNpcQuest(int NpcID)
    {
        string[] questArray;
        string[] proArray;
        string[] dataArray = NPCData.ToString().Split('\n');
        npcQuestList = new List<QuestInfo>();

        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                if (int.Parse(proArray[0]) == NpcID)
                {
                    questArray = proArray[5].Split('|');
                    for (int j = 0; j < questArray.Length; j++)
                    {
                        npcQuestList.Add(QuestList.getQuest(int.Parse(questArray[j])));
                    }
                }
            }
        }
        return npcQuestList;
    }
    Dictionary<int, Dictionary<CommunicationType, bool>> npcsType;
    public Dictionary<int, Dictionary<CommunicationType, bool>> LoadNpcType(int floorNum)
    {
        string[] proArray;
        string[] dataArray = NPCData.ToString().Split('\n');
        npcsType = new Dictionary<int, Dictionary<CommunicationType, bool>>();
        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "" )
            {

                proArray = dataArray[i].Split(',');
                if (proArray[1].EndsWith(floorNum.ToString()))
                {
                    npcsType.Add(int.Parse(proArray[0]), new Dictionary<CommunicationType, bool>() {
                    { CommunicationType.Talk, proArray[2] == "0" ? false : true },
                    { CommunicationType.Shop,proArray[3]=="0"?false: true },
                    { CommunicationType.Quest,proArray[4]=="0"?false:true }
                });
                }

            }
        }
        return npcsType;
    }

    #endregion
    #region PlayerState
    public PlayerStateData LoadPlayerState()
    {
        PlayerStateData state = new PlayerStateData();
        string[] dataArray = gameData.PlayerState.ToString().Split('\n');
        string[] proArray = dataArray[1].Split(',');
        state.EXP = int.Parse(proArray[0]);
        state.Money = int.Parse(proArray[1]);
        state.IsWalk = proArray[2] == "0" ? false : true;
        state.Type = (PlayerType) int.Parse(proArray[3]);

        dataArray = playerTypeData.ToString().Split('\n');
        for (int i = 0; i < dataArray.Length; i++)
        {
            proArray = dataArray[i].Split(',');
            if (state.Type.ToString() == proArray[0])
            {
                state.BaseSTR = int.Parse(proArray[1]);
                state.BaseDEX = int.Parse(proArray[2]);
                state.BaseINT = int.Parse(proArray[3]);
                state.BaseCON = int.Parse(proArray[4]);
                state.BaseLUK = int.Parse(proArray[5]);
            }
        }
        return state;
    }
    #endregion
    List<TalkText> textList;
    public List<TalkText> LoadText(int eventID)
    {
        textList = new List<TalkText>();
        string[] proArray;
        string[] dataArray = openingData.ToString().Split('\n');
        for (int i = 1; i < dataArray.Length; i++)
        {
            if(dataArray[i]!="")
            {
                proArray = dataArray[i].Split(',');
                if(proArray[0].EndsWith(eventID.ToString()))
                {
                    TalkText text = new TalkText();
                    switch(proArray[1])
                    {
                        case "text":
                            {
                                text.textType = TextType.text;
                                break;
                            }
                        case "textArea":
                            {
                                text.textType = TextType.textArea;
                                break;
                            }
                        case "picture":
                            {
                                text.textType = TextType.picture;
                                break;
                            }
                    }
                    switch (proArray[2])
                    {
                        case "foi":
                            {
                                text.textEffect = TextEffect.foi;
                                break;
                            }
                        case "in":
                            {
                                text.textEffect = TextEffect.pictureIn;
                                break;
                            }
                        case "out":
                            {
                                text.textEffect = TextEffect.pictureOut;
                                break;
                            }
                    }
                    text.textInfo = proArray[3];
                    textList.Add(text);
                }
            }
        }
        return textList;
    }

    #region Skill
    List<Skill> skillList;
    public List<Skill> LoadSkill()
    {
        skillList = new List<Skill>();
        string[] proArray;
        string[] dataArray = skillData.ToString().Split('\n');
        foreach (string item in dataArray)
        {
            proArray = item.Split(',');
            Skill skill = new Skill();
            skill.Id = int.Parse(proArray[0]);
            skill.Name = proArray[1];
            skill.Icon = proArray[2];
            switch (proArray[3])
            {

                default:
                    break;
            }
            switch (proArray[4])
            {
                case "Basic":
                    skill.SkillType = SkillType.basic;
                    break;
                case "Skill":
                    skill.SkillType = SkillType.skill;
                    break;
                default:
                    break;
            }
            switch (proArray[5])
            {
                case "Basic":
                    skill.PosType = PosType.basic;
                    break;
                case "One":
                    {
                        skill.PosType = PosType.one;
                        break;
                    }
                case "Two":
                    {
                        skill.PosType = PosType.two;
                        break;
                    }
                case "Three":
                    {
                        skill.PosType = PosType.three;
                        break;
                    }
            }
            skill.ColdTime = int.Parse(proArray[6]);
            skill.Damage = int.Parse(proArray[7]);
            skill.Level = 1;
            skillList.Add(skill);
        }
        return skillList;
    }
    #endregion
    #endregion

    #region Path

    //获取路径//
    private static string GetDataPath()
    {
        // Your game has read+write access to /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/Documents
        // Application.dataPath returns ar/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data             
        // Strip "/Data" from path
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            // Strip application name
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents";
        }
        else
            //    return Application.dataPath + "/Resources";
            return Application.dataPath;
    }
    #endregion
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#region セーブデータクラス
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
    public string PlayerEventProcess;
    public string PlayerPortalPorcess;
    public string SenceData;
    public string EnemyPositionData;
    public GameData()
    {

    }
    public GameData(string PlayerAcceptData, string PlayerBag, string PlayerState, string PlayerEventProcess, string PlayerPortalPorcess, string SenceData, string EnemyPositionData)
    {
        this.PlayerAcceptData = PlayerAcceptData;
        this.PlayerBag = PlayerBag;
        this.PlayerState = PlayerState;
        this.PlayerEventProcess = PlayerEventProcess;
        this.PlayerPortalPorcess = PlayerPortalPorcess;
        this.SenceData = SenceData;
        this.EnemyPositionData = EnemyPositionData;
    }
}
#endregion


#region GameController
public class GameController
{
    #region para
    private static GameController gameController;
    public static GameController _instance
    {
        get
        {
            if (gameController == null)
            {
                gameController = new GameController();
                return gameController;
            }
            else
            {
                return gameController;
            }
        }
    }
    string playerStateData;
    string playerTypeData;
    string bagData;
    string itemListData;
    string itemComposeData;
    string playerQuestData;
    string questListData;
    string NPCData;
    string skillData;
    string openingData;
    string eventData;
    string portalData;
    string areaData;
    string enemyInfoData;
    string enemyPositionData;
    #endregion
    #region Game Running Data
    private EntryType lastChangeSceneType;
    public void SetLastChangeSceneType(EntryType type)
    {
        this.lastChangeSceneType = type;
    }
    public EntryType GetLastChangeSceneType()
    {
        return this.lastChangeSceneType;
    }

    private int playerInFloor;
    public void SetGoingToFloor(int floorNumber)
    {
        this.playerInFloor = floorNumber;
    }
    public int GetGoingToFloor()
    {
        return this.playerInFloor;
    }
    #endregion

    #region 初期化
    private GameController()
    {
        playerStateData = GameManager._instance.playerStateData.text;
        playerTypeData = GameManager._instance.playerTypeData.text;
        bagData = GameManager._instance.bagData.text;
        itemListData = GameManager._instance.itemListData.text;
        itemComposeData = GameManager._instance.itemComposeData.text;
        playerQuestData = GameManager._instance.playerQuestData.text;
        questListData = GameManager._instance.questListData.text;
        NPCData = GameManager._instance.NPCData.text;
        skillData = GameManager._instance.skillData.text;
        openingData = GameManager._instance.openingData.text;
        eventData = GameManager._instance.eventData.text;
        portalData = GameManager._instance.portalData.text;
        areaData = GameManager._instance.sceneData.text;
        enemyPositionData = GameManager._instance.enemyPositionData.text;
        enemyInfoData = GameManager._instance.enemyInfoData.text;


        //Application.targetFrameRate = 45;
        xs = new XmlSaver();
        gameData = new GameData();
        gameData.key = GameManager._instance.gameDataKey;

        //Load(0);


        #region test
        testLoad();

        lastChangeSceneType = EntryType.Portal;
        playerInFloor = -8;

        #endregion

    }
    #endregion
    #region paramater
    private string dataFileName = "save0.dat";//セーブデータの名前

    private XmlSaver xs;
    public GameData gameData;
    string saveString;
    //PlayerState playerState;

    #endregion

    #region newGame
    public void newGame()
    {
        gameController = new GameController();
        testLoad();
        lastChangeSceneType = EntryType.Down;
    }
    #endregion

    #region Save
    public void WriteData()
    {
        string gameDataFile = GameManager.GetDataPath() + "/" + dataFileName;
        string dataString = xs.SerializeObject(gameData, typeof(GameData));
        xs.CreateXML(gameDataFile, dataString);
    }
    public void Save(int dataNumber)
    {
        dataFileName = string.Format("save{0}.dat", dataNumber);
        SaveAcceptQuest();
        SaveBag();
        SavePlayerState();
        SaveEventProcess();
        SavePortalProcess();
        SaveAreaData();
        SaveEnemyPositionData();
        WriteData();
    }
    void InitSave()
    {
        dataFileName = "save0.dat";
        gameData.PlayerAcceptData = playerQuestData.ToString();
        gameData.PlayerBag = bagData.ToString();
        gameData.PlayerState = playerStateData.ToString();
        gameData.PlayerEventProcess = eventData.ToString();
        gameData.PlayerPortalPorcess = portalData.ToString();
        gameData.SenceData = areaData.ToString();
        gameData.EnemyPositionData = enemyPositionData.ToString();
        WriteData();
    }
    #region Item
    const string bagUper = "ID,isEqueped,cout,itemID";
    void SaveBag()
    {
        saveString = "";
        foreach (KeyValuePair<int, Item> item in PlayerState._instance.GetPlayerBag().dictionBag)
        {
            saveString += string.Format("{0},{1},{2},{3}\n", item.Key, item.Value.isEqueped ? "1" : "0", item.Value.count, item.Value.info.id);
        }
        gameData.PlayerBag = string.Format("{0}\n{1}", bagUper, saveString);
    }
    #endregion
    #region Quest
    const string questUper = "ID,StepNow,overCount,QuestID,isOver,isAccept";
    void SaveAcceptQuest()
    {
        saveString = "";
        foreach (KeyValuePair<int, Quest> item in PlayerState._instance.GetPlayerQuest().GetAcceptQuestList())
        {
            saveString += string.Format("{0},{1},{2},{3},{4},{5}\n", item.Key, item.Value.stepNow, item.Value.count, item.Value.ID, item.Value.isOver ? "1" : "0", item.Value.isAccept ? "1" : "0");
        }
        gameData.PlayerAcceptData = string.Format("{0}\n{1}", questUper, saveString);
    }
    #endregion
    #region PlayerState
    const string playerStateUper = "EXP,money,isWalk,Type,floorNumber";
    void SavePlayerState()
    {
        gameData.PlayerState = string.Format("{0}\n{1},{2},{3},{4},{5},{6}\n",
            playerStateUper,
            PlayerState._instance.EXP,
            PlayerState._instance.money,
            PlayerState._instance.isWalk ? 1 : 0,
            (int)PlayerState._instance.type,
            playerInFloor,
            (int)this.lastChangeSceneType
            );
    }
    #endregion
    #region Skill
    void SaveSkill()
    {

    }

    #endregion
    #region event
    const string playerEventProcessUper = "eventID,eventDone";
    void SaveEventProcess()
    {
        saveString = "";
        foreach (KeyValuePair<int, bool> item in eventDictionary)
        {
            saveString += string.Format("{0},{1}\n", item.Key, item.Value ? "1" : "0");

        }
        gameData.PlayerEventProcess = string.Format("{0}\n{1}", playerEventProcessUper, saveString);
    }
    #endregion
    #region protal
    const string playerPortalProcessUper = "floorNumber,isOpen";
    void SavePortalProcess()
    {
        saveString = "";
        foreach (KeyValuePair<int, bool> item in portalDictionary)
        {
            saveString += string.Format("{0},{1}\n", item.Key, item.Value ? "1" : "0");
        }
        gameData.PlayerPortalPorcess = string.Format("{0}\n{1}", playerPortalProcessUper, saveString);
    }
    #endregion
    #region areaInfo
    const string AreaDataUper = "floorNumber,areaName,areaPositionX,areaPositionY,areaPositionZ,areaAngle";
    void SaveAreaData()
    {
        saveString = "";
        foreach (KeyValuePair<int, List<AreaData>> item in areaDictionary)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                saveString += string.Format("{0},{1},{2},{3},{4},{5}\n", item.Value[i].floorNumber, item.Value[i].areaName, item.Value[i].areaPosition.x, item.Value[i].areaPosition.y, item.Value[i].areaPosition.z, item.Value[i].areaAngle.eulerAngles.y);
            }

        }
        gameData.SenceData = string.Format("{0}\n{1}", AreaDataUper, saveString);
    }
    #endregion
    #region enemyPositionInfo
    const string enemyPositionDataUper = "floorNum,enemyName,enemyLevel,leftUpPositionX,leftUpPositionY,leftUpPositionZ,width,height";
    void SaveEnemyPositionData()
    {
        saveString = "";
        foreach (List<EnemyPositionData> item in enemyPositionDictionary.Values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                saveString += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", item[i].FloorNum, item[i].EnemyID, item[i].EnemeyCount, item[i].EnemyLevel, item[i].LeftUpPosition.x, item[i].LeftUpPosition.y, item[i].LeftUpPosition.z, item[i].Width, item[i].Height);
            }
        }
        gameData.EnemyPositionData = string.Format("{0}\n{1}", AreaDataUper, saveString);
    }


    #endregion
    #endregion

    #region Load
    void testLoad()
    {
        InitSave();
        dataFileName = string.Format("save0.dat");
        string gameDataFile = GameManager.GetDataPath() + "/" + dataFileName;
        if (xs.hasFile(gameDataFile))
        {
            string dataString = xs.LoadXML(gameDataFile);
            GameData gameDataFromXML = xs.DeserializeObject(dataString, typeof(GameData)) as GameData;
            //セーブデータ　使用可能
            gameData = gameDataFromXML;
            LoadEventProcess();
            LoadPortalList();
            LoadAreaData();
            LoadPlayerPosition();
            LoadEnemyPositionData();
            LoadenemyInfo();

        }
    }
    void clearData()
    {
        if (bagList != null)
        {
            bagList.Clear();
        }
        if (eventDictionary != null)
        {
            eventDictionary.Clear();

        }

        if (stepList != null)
        {
            stepList.Clear();
        }
        if (portalDictionary != null)
        {

            portalDictionary.Clear();
        }
        if (areaDictionary != null)
        {
            areaDictionary.Clear();

        }
    }
    public void Load(int dataNumber)
    {
        dataFileName = string.Format("save{0}.dat", dataNumber);
        string gameDataFile = GameManager.GetDataPath() + "/" + dataFileName;
        if (xs.hasFile(gameDataFile))
        {
            string dataString = xs.LoadXML(gameDataFile);
            GameData gameDataFromXML = xs.DeserializeObject(dataString, typeof(GameData)) as GameData;
            //セーブデータ　使用可能
            gameData = gameDataFromXML;
            clearData();
            LoadBagItem();
            LoadEventProcess();
            LoadPortalList();
            LoadAreaData();
            LoadPlayerPosition();
            LoadEnemyPositionData();
            LoadenemyInfo();

            int floorNumber = GameController._instance.GetGoingToFloor();
            if (floorNumber == (int)SceneFloorInfo.Town)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Town));
            }
            else if (floorNumber == (int)SceneFloorInfo.FirstFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.FirstFloor));

            }
            else if (floorNumber == (int)SceneFloorInfo.LastFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.LastFloor));

            }
            else if (floorNumber == (int)SceneFloorInfo.ShopFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.ShopFloor));

            }
            else if (floorNumber == (int)SceneFloorInfo.BossFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.BossFloor));

            }
            else
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.RandomMapFloor));
            }
            PlayerState._instance.ReLoad();

        }
        //セーブデータが存在しません
        else
        {
            Debug.Log("Not found savedata");
            InitSave();
            Load(0);
        }
    }
    #region Item

    Dictionary<int, ItemInfo> itemList;
    ItemInfo itemInfo;
    void LoadItemList()
    {
        if (itemList == null || itemList.Count == 0)
        {
            //if(itemList!=null)
            //{
            //    return itemList;
            //}
            //else
            //{
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
                            int.Parse(proArray[16]),
                            proArray[17]
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
                            int.Parse(proArray[16]),
                            proArray[17],
                            (ItemType)int.Parse(proArray[1])
                            );
                    }
                    itemList.Add(itemInfo.id, itemInfo);
                }
            }
        }

    }

    /// <summary>
    /// ファイルからアイテムのデータを導入
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, ItemInfo> GetItemList()
    {
        if (itemList == null || itemList.Count == 0)
        {
            LoadItemList();
        }
        return itemList;
        //}
    }

    public ItemInfo GetItem(int itemID)
    {
        if (itemList == null || itemList.Count == 0)
        {
            LoadItemList();
        }
        itemList.TryGetValue(itemID, out itemInfo);
        return itemInfo;
    }

    Dictionary<int, Item> bagList;
    Item item;
    void LoadBagItem()
    {
        if (bagList == null || bagList.Count == 0)
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
                        item = new Item(int.Parse(proArray[0]), proArray[1] == "0" ? false : true, ItemList.getItem(int.Parse(proArray[3])));
                    }
                    else
                    {
                        item = new Item(int.Parse(proArray[0]), int.Parse(proArray[2]), ItemList.getItem(int.Parse(proArray[3])));
                    }
                    bagList.Add(item.id, item);
                }
            }
        }
    }


    /// <summary>
    /// ファイルからバッグのデータを導入
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Item> GetBagItemList()
    {
        if (bagList == null || bagList.Count == 0)
        {
            LoadBagItem();
        }
        return bagList;
    }
    #endregion

    #region ItemCompose
    Dictionary<ItemType, List<ItemCompose>> itemComposeDictionary;
    List<ItemCompose> itemComposeList;
    ItemCompose itemCompose;
    void LoadItemComposeList()
    {
        if (itemComposeDictionary == null || itemComposeDictionary.Count == 0)
        {
            string[] proArray;
            string[] idArray;
            string[] countArray;
            string[] dataArray = itemComposeData.ToString().Split('\n');
            itemComposeDictionary = new Dictionary<ItemType, List<ItemCompose>>();
            ItemType itemtype;

            for (int i = 1; i < dataArray.Length; i++)
            {
                if (dataArray[i] != "")
                {
                    proArray = dataArray[i].Split(',');
                    idArray = proArray[2].Split('|');
                    countArray = proArray[3].Split('|');

                    itemCompose = new ItemCompose(int.Parse(proArray[0]), int.Parse(proArray[1]), idArray.ParseToInt(), countArray.ParseToInt());

                    itemtype = GetItem(itemCompose.ResultItem.itemID).type;


                    if (!itemComposeDictionary.TryGetValue(itemtype, out itemComposeList))
                    {
                        itemComposeList = new List<ItemCompose>();

                    }
                    itemComposeList.Add(itemCompose);
                    itemComposeDictionary[itemtype] = itemComposeList;
                }
            }
        }
    }

    public List<ItemCompose> GetItemComposeList(ItemType type)
    {
        if (itemComposeDictionary == null || itemComposeDictionary.Count == 0)
        {
            LoadItemComposeList();
        }
        if (itemComposeDictionary.TryGetValue(type, out itemComposeList))
        {
            return itemComposeList;
        }
        else
        {
            return new List<ItemCompose>();
        }
    }

    #endregion
    #region Quest
    QuestInfo info;
    List<QuestInfo.QuestStep> stepList;
    Dictionary<int, QuestInfo> questList;
    public Dictionary<int, QuestInfo> LoadQuestList()
    {
        //if(questList!=null)
        //{
        //    return questList;
        //}
        //else
        //{
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
                    stepList.Add(QuestInfo.makeStep(proArray[3], (QuestType)int.Parse(proArray[4]), int.Parse(proArray[5]), int.Parse(proArray[6]), int.Parse(proArray[7]), int.Parse(proArray[8]), int.Parse(proArray[9])));
                }
                proArray = dataArray[minIndex].Split(',');
                info = new QuestInfo(int.Parse(proArray[0]), proArray[1], proArray[2], stepList);
                questList.Add(info.ID, info);
                minIndex = maxIndex - 1;
            }
        }
        return questList;
        //}

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
                        proArray[5].Replace("\r", "").Equals("0") ? false : true));
            }
        }
        return acceptQuestList;
    }
    #endregion
    #region NPC
    Dictionary<int, Item> shopItemList;
    List<QuestInfo> npcQuestList;
    NPCDATA npc;
    Dictionary<int, NPCDATA> npcDiction;
    public Dictionary<int, NPCDATA> LoadNPCData()
    {
        npcDiction = new Dictionary<int, NPCDATA>();
        string[] questArray;
        string[] itemArray;
        string[] proArray;
        string[] dataArray = NPCData.ToString().Split('\n');
        for (int i = 1; i < dataArray.Length; i++)
        {

            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                npcQuestList = new List<QuestInfo>();
                shopItemList = new Dictionary<int, Item>();
                questArray = proArray[11].Split('|');
                itemArray = proArray[12].Split('|');

                for (int j = 0; j < questArray.Length; j++)
                {
                    npcQuestList.Add(QuestList.getQuest(int.Parse(questArray[j])));
                }
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
                npc = new NPCDATA(
                    int.Parse(proArray[0]),
                    proArray[1],
                    int.Parse(proArray[2]),
                     new Dictionary<CommunicationType, bool>() {
                    { CommunicationType.Talk, proArray[7] == "0" ? false : true },
                    { CommunicationType.Shop,proArray[8]=="0"?false: true },
                    { CommunicationType.Quest,proArray[9]=="0"?false:true } },
                         int.Parse(proArray[4]), int.Parse(proArray[5]), int.Parse(proArray[6]),
                         int.Parse(proArray[3]),
                         proArray[10],
                         npcQuestList,
                         shopItemList,
                         int.Parse(proArray[13])
                         );
                npcDiction.Add(npc.Id, npc);
            }
        }
        return npcDiction;
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
        state.Type = (PlayerType)int.Parse(proArray[3]);

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
    public void LoadPlayerPosition()
    {
        string[] dataArray = gameData.PlayerState.ToString().Split('\n');
        string[] proArray = dataArray[1].Split(',');
        playerInFloor = int.Parse(proArray[4]);
        lastChangeSceneType = (EntryType)int.Parse(proArray[5]);
    }

    #endregion
    #region TextEvent
    List<TalkText> textList;
    public List<TalkText> LoadText(int eventID)
    {
        textList = new List<TalkText>();
        string[] proArray;
        string[] dataArray = openingData.ToString().Split('\n');
        for (int i = 1; i < dataArray.Length; i++)
        {
            if (dataArray[i] != "")
            {
                proArray = dataArray[i].Split(',');
                if (proArray[0].Equals(eventID.ToString()))
                {
                    TalkText text = new TalkText();
                    switch (proArray[1])
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
                    text.textInfo = proArray[3].Replace("\\n", "\n").Replace("\r", "");
                    text.iconName = proArray[4];
                    text.speakerName = proArray[5];
                    textList.Add(text);
                }
            }
        }
        return textList;
    }
    #endregion
    #region Skill
    List<Skill> skillList;
    public List<Skill> LoadSkill()
    {
        //if (skillList != null)
        //{
        //    return skillList;
        //}
        //else
        //{
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
            skill.Enegy = int.Parse(proArray[8]);
            skill.Level = 1;
            skillList.Add(skill);
        }
        return skillList;


        //}


    }
    #endregion
    #region EventList
    Dictionary<int, bool> eventDictionary;
    void LoadEventProcess()
    {
        if (eventDictionary == null || eventDictionary.Count == 0)
        {
            string[] proArray;
            string[] dataArray = gameData.PlayerEventProcess.ToString().Split('\n');
            eventDictionary = new Dictionary<int, bool>();
            for (int i = 1; i < dataArray.Length; i++)
            {
                if (dataArray[i] != "")
                {
                    proArray = dataArray[i].Split(',');
                    //1==true==done
                    eventDictionary.Add(int.Parse(proArray[0]), proArray[1].Replace("\r", "") == "1");
                }
            }
        }
    }
    public bool getEventIsDone(int eventID)
    {
        bool eventDone;
        eventDictionary.TryGetValue(eventID, out eventDone);
        return eventDone;
    }
    public void doneEvent(int eventID)
    {
        bool eventDone;
        if (eventDictionary.TryGetValue(eventID, out eventDone))
        {
            eventDictionary[eventID] = true;
        }
    }
    #endregion
    #region PortalList 
    /// <summary>
    /// floorNumber、floor展開？（利用可否）
    /// </summary>
    Dictionary<int, bool> portalDictionary;
    void LoadPortalList()
    {
        if (portalDictionary == null || portalDictionary.Count == 0)
        {
            string[] proArray;
            string[] dataArray = gameData.PlayerPortalPorcess.ToString().Split('\n');
            portalDictionary = new Dictionary<int, bool>();
            for (int i = 1; i < dataArray.Length; i++)
            {
                if (dataArray[i] != "")
                {
                    proArray = dataArray[i].Split(',');
                    portalDictionary.Add(int.Parse(proArray[0]), proArray[1].Replace("\r", "") == "1");
                }
            }
        }
    }
    public Dictionary<int, bool> getPortalList()
    {
        return portalDictionary;
    }
    public void makePortalOpen(int floorNumber)
    {
        try
        {
            portalDictionary[floorNumber] = true;
        }
        catch
        {

        }
    }
    public void makePortalClose(int floorNumber)
    {
        try
        {
            portalDictionary[floorNumber] = false;
        }
        catch
        {

        }
    }

    #endregion
    #region sceneInfo
    Dictionary<int, List<AreaData>> areaDictionary;
    List<AreaData> areaList;
    void LoadAreaData()
    {
        int floorNumber = 0;
        if (areaDictionary == null || areaDictionary.Count == 0)
        {
            string[] proArray;
            string[] dataArray = gameData.SenceData.ToString().Split('\n');
            areaDictionary = new Dictionary<int, List<AreaData>>();
            for (int i = 1; i < dataArray.Length; i++)
            {

                if (dataArray[i] != "")
                {
                    proArray = dataArray[i].Split(',');

                    floorNumber = int.Parse(proArray[0]);


                    areaDictionary.TryGetValue(floorNumber, out areaList);
                    if (areaList == null)
                    {
                        areaList = new List<AreaData>();
                    }
                    areaList.Add(new AreaData(floorNumber, proArray[1], float.Parse(proArray[2]), float.Parse(proArray[3]), float.Parse(proArray[4]), (AngleFix)int.Parse(proArray[5])));

                    areaDictionary[floorNumber] = areaList;
                }
            }
        }
    }
    public List<AreaData> GetAreaDataList(int floorNumber)
    {
        areaDictionary.TryGetValue(floorNumber, out areaList);
        return areaList;
    }
    public void SetAreaData(int floorNumber, List<AreaData> areaDataList)
    {
        areaDictionary[floorNumber] = areaDataList;
    }

    #endregion

    #region enemy
    #region enemyInfo
    Dictionary<int, EnemyDropInfo> enemyInfoDictionary;
    EnemyDropInfo enemyInfo;
    void LoadenemyInfo()
    {
        if (enemyInfoDictionary == null)
        {
            string[] proArray;
            string[] dataArray = enemyInfoData.ToString().Split('\n');
            string[] dropItemIDArray;
            string[] dropItemLVArray;
            string[] dropItemPreArray;
            List<dropItem> dropItemList;
            enemyInfoDictionary = new Dictionary<int, EnemyDropInfo>();
            for (int i = 1; i < dataArray.Length; i++)
            {
                if (dataArray[i] != "")
                {
                    proArray = dataArray[i].Split(',');
                    dropItemIDArray = proArray[2].Split('|');
                    dropItemLVArray = proArray[3].Split('|');
                    dropItemPreArray = proArray[4].Split('|');

                    if (dropItemIDArray.Length != dropItemLVArray.Length || dropItemLVArray.Length != dropItemPreArray.Length)
                    {
                        Debug.LogError("DropInfomationError!");
                        return;
                    }
                    else
                    {
                        dropItemList = new List<dropItem>();
                        for (int j = 0; j < dropItemIDArray.Length; j++)
                        {
                            dropItemList.Add(new dropItem()
                            {
                                itemID = int.Parse(dropItemIDArray[j]),
                                dropItemLV = int.Parse(dropItemLVArray[j]),
                                dropItemPre = int.Parse(dropItemPreArray[j])

                            });
                        }
                    }
                    enemyInfo = new EnemyDropInfo(int.Parse(proArray[0]), proArray[1], dropItemList, float.Parse(proArray[5]));
                    enemyInfoDictionary.Add(enemyInfo.ID, enemyInfo);
                }
            }
        }
    }
    /// <summary>
    /// 敵のIDにより、敵の情報を貰い（ドロップ率など）
    /// </summary>
    /// <param name="enemyID">敵のID</param>
    /// <returns></returns>
    public EnemyDropInfo GetEnemyInfo(int enemyID)
    {
        if (enemyInfoDictionary == null)
        {
            LoadenemyInfo();
        }
        enemyInfoDictionary.TryGetValue(enemyID, out enemyInfo);

        return enemyInfo;
    }
    public EnemyDropInfo GetRandomEnemyInfo()
    {
        return enemyInfoDictionary.getRandomOne();
    }


    #endregion
    #region enemyPosition
    Dictionary<int, List<EnemyPositionData>> enemyPositionDictionary;
    List<EnemyPositionData> enemyPositionList;
    void LoadEnemyPositionData()
    {
        int floorNumber = 0;
        if (enemyPositionDictionary == null)
        {
            string[] proArray;
            string[] dataArray = gameData.EnemyPositionData.ToString().Split('\n');
            enemyPositionDictionary = new Dictionary<int, List<EnemyPositionData>>();
            for (int i = 1; i < dataArray.Length; i++)
            {

                if (dataArray[i] != "")
                {
                    proArray = dataArray[i].Split(',');

                    floorNumber = int.Parse(proArray[0]);


                    enemyPositionDictionary.TryGetValue(floorNumber, out enemyPositionList);
                    if (enemyPositionList == null)
                    {
                        enemyPositionList = new List<EnemyPositionData>();
                    }
                    enemyPositionList.Add(new EnemyPositionData(floorNumber, int.Parse(proArray[1]), int.Parse(proArray[2]), int.Parse(proArray[3]), float.Parse(proArray[4]), float.Parse(proArray[5]), float.Parse(proArray[6]), float.Parse(proArray[7]), float.Parse(proArray[8])));

                    enemyPositionDictionary[floorNumber] = enemyPositionList;
                }
            }
        }
    }
    public List<EnemyPositionData> GetEnemyPosition(int floorNum)
    {
        enemyPositionDictionary.TryGetValue(floorNum, out enemyPositionList);
        return enemyPositionList;
    }
    public void SetEnemyPositon(int floorNum, List<EnemyPositionData> enemeyPositonList)
    {
        this.enemyPositionDictionary[floorNum] = enemeyPositonList;
    }
    #endregion


    #endregion
    #endregion

}
#endregion
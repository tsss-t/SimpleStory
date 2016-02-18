using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIQuestManager : MonoBehaviour
{
    #region para
    bool isShowPanel;

    private Quest selectQuest;

    public GameObject prefabQuestButton;
    public GameObject prefabStepButton;

    private PlayerQuest playerQuest;


    private List<GameObject> questShowList;
    private List<GameObject> stepShowList;

    private NPCManager npcManager;
    private EnemyManager enemyManager;
    #region UI
    UIController mainControllerUI;

    GameObject containQuestGrid;
    GameObject containStepGrid;

    #endregion
    #endregion


    #region start
    // Use this for initialization
    void Start()
    {
        isShowPanel = false;
        questShowList = new List<GameObject>();
        stepShowList = new List<GameObject>();
        playerQuest = PlayerState._instance.GetPlayerQuest();

        npcManager =NPCManager._instance;
        enemyManager = EnemyManager._instance;
        mainControllerUI =UIController._instance;


        containQuestGrid = transform.Find("QuestBG").Find("Scroll View").Find("Items").gameObject;
        containStepGrid = transform.Find("StepBG").Find("Scroll View").Find("Items").gameObject;
        this.gameObject.SetActive(false);
    }
    #endregion
    #region UpdateQuest/UpdateStep
    GameObject goQuest;

    void UpdateQuest()
    {
        selectQuest = null;
        playerQuest = PlayerState._instance.GetPlayerQuest();
        for (int i = 0; i < questShowList.Count; i++)
        {
            NGUITools.Destroy(questShowList[i]);
        }
        questShowList.Clear();

        foreach (KeyValuePair<int, Quest> item in playerQuest.GetAcceptQuestList())
        {
            if (selectQuest == null)
            {
                selectQuest = item.Value;
            }
            if (!item.Value.isOver)
            {
                goQuest = NGUITools.AddChild(containQuestGrid, prefabQuestButton);
                goQuest.transform.Find("Title").GetComponent<UILabel>().text = string.Format("Quest:{0}", item.Value.info.name);
                goQuest.transform.Find("Doing").GetComponent<UILabel>().text = string.Format("Doing : Step {0}", item.Value.stepNow);
                goQuest.name = item.Key.ToString();
                questShowList.Add(goQuest);
            }
        }

        containQuestGrid.GetComponent<UIGrid>().enabled = true;

        UpdateStep();
    }
    GameObject goStep;
    void UpdateStep()
    {
        for (int i = 0; i < stepShowList.Count; i++)
        {
            NGUITools.Destroy(stepShowList[i]);
        }
        stepShowList.Clear();

        if (selectQuest != null)
        {
            for (int i = 1; i <= selectQuest.stepNow; i++)
            {
                goStep = NGUITools.AddChild(containStepGrid, prefabStepButton);
                switch (selectQuest.info.GetStep(i).questType)
                {
                    case QuestType.findNPC:
                        goStep.transform.Find("Tween").Find("LabelDescription").GetComponent<UILabel>().text = string.Format("任務説明：\n {0} \n\n 任務進捗：\n {1}と話してください。{2}",
                            selectQuest.info.GetStep(i).description,
                            npcManager.GetNPCInfo(selectQuest.info.GetStep(i).targetID).Name,
                            selectQuest.info.GetStep(i).count == selectQuest.count ? "(完成)" : "");
                        break;
                    case QuestType.findItem:
                        goStep.transform.Find("Tween").Find("LabelDescription").GetComponent<UILabel>().text = string.Format("任務説明：\n {0} \n\n 任務進捗：\n {1}  :  {2}/{3}",
                            selectQuest.info.GetStep(i).description,
                            ItemList.getItem(selectQuest.info.GetStep(i).targetID).name,
                            PlayerState._instance.GetPlayerBag().GetItemCount(selectQuest.info.GetStep(i).targetID),
                            selectQuest.info.GetStep(i).count);
                        break;
                    case QuestType.killEnemy:
                        goStep.transform.Find("Tween").Find("LabelDescription").GetComponent<UILabel>().text = string.Format("任務説明：\n {0} \n\n 任務進捗：\n {1} :  {2}/{3}",
                            selectQuest.info.GetStep(i).description,
                            enemyManager.getEnemyName(selectQuest.info.GetStep(i).targetID),
                            selectQuest.count, selectQuest.info.GetStep(i).count);
                        break;
                    default:
                        break;
                }
                goStep.transform.Find("Title").GetComponent<UILabel>().text = string.Format("Step : {0}", i);
                goStep.name = i.ToString();
                stepShowList.Add(goStep);
            }
            containStepGrid.GetComponent<UITable>().repositionNow = true;
        }
    }
    #endregion
    #region 外部API
    public void SetSelectQuest(Quest quest)
    {
        this.selectQuest = quest;
    }
    #endregion
    #region UI Event
    public void OnCloseButtonClick()
    {
        Hide();
    }
    public void OnOpenButtonClick()
    {
        Show();
    }
    public void OnTollgleButtonClick()
    {
        if (isShowPanel)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    public void OnQuestButtonClick()
    {
        UpdateStep();
    }
    void Show()
    {
        if (!isShowPanel)
        {
            this.gameObject.SetActive(true);
            mainControllerUI.CloseAllWindows();

            UpdateQuest();
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            StartCoroutine(HidePanel());
        }
    }
    #endregion
    #region Panel CUTIN/OUT
    IEnumerator HidePanel()
    {

        this.GetComponent<UITweener>().PlayForward();
        yield return new WaitForSeconds(this.transform.GetComponent<UITweener>().duration);
        this.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        this.GetComponent<UITweener>().PlayReverse();
        yield return new WaitForSeconds(0.05f);
        isShowPanel = true;
    }
    #endregion
}

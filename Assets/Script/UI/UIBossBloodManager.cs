using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBossBloodManager : MonoBehaviour
{

    public static UIBossBloodManager _instance;
    public GameObject bossBloodBarPrefab;

    bool isShowPanel;
    GameObject[] boss;
    GameObject UIBossBloodBarGrid;
    GameObject UIBossBloodBarContainer;
    UITweener tweener;

    Dictionary<int, UIBossBloodItemEvent> bossBloodBarDictionary;
    Dictionary<int, BossController> bossControllerDictionary;
    void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start()
    {
        boss = GameObject.FindGameObjectsWithTag(Tags.boss);
        isShowPanel = false;
        bossBloodBarDictionary = new Dictionary<int, UIBossBloodItemEvent>();
        bossControllerDictionary = new Dictionary<int, BossController>();

        UIBossBloodBarContainer = transform.Find("BossBloodContainer").gameObject;
        UIBossBloodBarGrid = UIBossBloodBarContainer.transform.Find("BossBloodBarItems").gameObject;
        tweener = UIBossBloodBarContainer.GetComponent<UITweener>();

        if (boss.Length != 0)
        {
            GameObject prefabItem;
            BossController tempBossController;
            for (int i = 0; i < boss.Length; i++)
            {
                prefabItem = NGUITools.AddChild(UIBossBloodBarGrid, bossBloodBarPrefab);
                tempBossController = boss[i].GetComponent<BossController>();
                tempBossController.onStateChanged += OnBossStateChanged;
                bossControllerDictionary.Add(tempBossController.enemyID, tempBossController);
                bossBloodBarDictionary.Add(tempBossController.enemyID, prefabItem.GetComponent<UIBossBloodItemEvent>());
                bossBloodBarDictionary[tempBossController.enemyID].Init(bossControllerDictionary[tempBossController.enemyID].gameObject.name, bossControllerDictionary[tempBossController.enemyID].GetBossMaxHP());
            }
            UIBossBloodBarGrid.GetComponent<UIGrid>().enabled = true;
        }
    }
    #region UI Events
    public void OnCloseButtonClick()
    {
        this.Hide();
    }
    public void OnOpenButtonClick()
    {
        this.Show();
    }
    #endregion
    #region UI Actions
    public void OnBossStateChanged(int BossID)
    {
        UIBossBloodItemEvent item;
        BossController tempBossController;
        if (bossBloodBarDictionary.TryGetValue(BossID, out item))
        {
            if (bossControllerDictionary.TryGetValue(BossID, out tempBossController))
            {
                item.SetHp(tempBossController.GetBossHP(), tempBossController.GetBossMaxHP());

                if (tempBossController.GetBossHP() <= 0)
                {
                    bossControllerDictionary.Remove(BossID);
                }

                if (bossControllerDictionary.Count == 0)
                {
                    OnCloseButtonClick();
                }
            }
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < boss.Length; i++)
        {
            if (boss[i] != null)
            {
                boss[i].GetComponent<BossController>().onStateChanged -= OnBossStateChanged;
            }
        }
    }
    void Show()
    {
        if (!isShowPanel)
        {
            this.gameObject.SetActive(true);
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
        tweener.PlayReverse();
        yield return new WaitForSeconds(tweener.duration);
        this.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        tweener.PlayForward();
        yield return new WaitForSeconds(0.05f);
        isShowPanel = true;
    }
    #endregion

}

using UnityEngine;
using System.Collections;

public class UISystemManager : MonoBehaviour
{
    public static UISystemManager _instance;


    bool isShowPanel;

    GameObject SaveContainer;
    GameObject LoadContainer;
    GameObject SureContainer;

    UILabel systemInfoLabel;

    SystemItemType selectSelectItem;

    void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start()
    {
        isShowPanel = false;
        SaveContainer = transform.Find("SystemContainer/Save").gameObject;
        LoadContainer = transform.Find("SystemContainer/Load").gameObject;
        SureContainer = transform.Find("SystemContainer/SureContainer").gameObject;
        systemInfoLabel = SureContainer.transform.Find("SystemInfoLabel").GetComponent<UILabel>();

        SaveContainer.SetActive(false);
        LoadContainer.SetActive(false);
        SureContainer.SetActive(false);
    }


    #region UI Events
    public void OnSaveButtonClick()
    {
        SaveContainer.SetActive(true);
    }
    public void OnSaveCloseButtonClick()
    {
        SaveContainer.SetActive(false);
    }
    public void OnLoadButtonClick()
    {
        LoadContainer.SetActive(true);
    }
    public void OnLoadCloseButtonClick()
    {
        LoadContainer.SetActive(false);
    }
    public void OnExitButtonClick()
    {
        systemInfoLabel.text = string.Format("ゲームを終了する？");
        this.selectSelectItem = SystemItemType.exit;
        SureContainer.SetActive(true);
    }
    public void OnExitCloseButtonClick()
    {
        SureContainer.SetActive(false);
    }
    public void OnOpenButtonClick()
    {
        this.Show();
    }
    public void OnCloseButtonClick()
    {
        this.Hide();
    }
    public void OnOpenSureMenuButtonClick()
    {
        this.SureContainer.SetActive(true);
    }
    public void OnCloseSureMenuButtonClick()
    {
        this.SureContainer.SetActive(false);
    }
    public void OnSaveLoadDataButtonClick(SystemItemType type)
    {
        selectSelectItem = type;
        switch (selectSelectItem)
        {
            case SystemItemType.save1:
                systemInfoLabel.text = string.Format("データ１の位置でセーブする？");
                break;
            case SystemItemType.save2:
                systemInfoLabel.text = string.Format("データ２の位置でセーブする？");

                break;
            case SystemItemType.save3:
                systemInfoLabel.text = string.Format("データ３の位置でセーブする？");

                break;
            case SystemItemType.save4:
                systemInfoLabel.text = string.Format("データ４の位置でセーブする？");

                break;
            case SystemItemType.load1:
                systemInfoLabel.text = string.Format("データ１の位置でロードする？");

                break;
            case SystemItemType.load2:
                systemInfoLabel.text = string.Format("データ２の位置でロードする？");

                break;
            case SystemItemType.load3:
                systemInfoLabel.text = string.Format("データ３の位置でロードする？");

                break;
            case SystemItemType.load4:
                systemInfoLabel.text = string.Format("データ４の位置でロードする？");

                break;
            case SystemItemType.none:
                break;
            case SystemItemType.exit:
                systemInfoLabel.text = string.Format("ゲームを終了する？");

                break;
            default:
                break;
        }
        SureContainer.SetActive(true);
    }
    public void OnSureContainerYesButtonClick()
    {
        switch (selectSelectItem)
        {
            case SystemItemType.save1:
                GameController._instance.Save(1);
                break;
            case SystemItemType.save2:
                GameController._instance.Save(2);

                break;
            case SystemItemType.save3:
                GameController._instance.Save(3);

                break;
            case SystemItemType.save4:
                GameController._instance.Save(4);
                break;
            case SystemItemType.load1:
                Hide();
                GameController._instance.Load(1);

                break;
            case SystemItemType.load2:
                Hide();

                GameController._instance.Load(2);

                break;
            case SystemItemType.load3:
                Hide();

                GameController._instance.Load(3);

                break;
            case SystemItemType.load4:
                Hide();

                GameController._instance.Load(4);

                break;
            case SystemItemType.none:
                break;
            case SystemItemType.exit:
                Application.Quit();
                break;
            default:
                break;
        }
        SureContainer.SetActive(false);
    }
    public void OnSureContainerNoButtonClick()
    {
        SureContainer.SetActive(false);
    }
    #endregion

    #region UI Actions

    void Show()
    {
        if (!isShowPanel)
        {

            this.gameObject.SetActive(true);
            UIController._instance.CloseAllWindows();
            //Time.timeScale = 0;
            PlayerState._instance.ChangeAction(PlayerState.PlayerAction.Talking);
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            //Time.timeScale = 1;
            PlayerState._instance.ChangeAction(PlayerState.PlayerAction.Free);
            StartCoroutine(HidePanel());
        }
    }

    #endregion


    #region Panel CUTIN/OUT
    IEnumerator HidePanel()
    {

        this.GetComponentInChildren<UITweener>().PlayReverse();
        yield return new WaitForSeconds(this.transform.GetComponentInChildren<UITweener>().duration);
        this.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        this.GetComponentInChildren<UITweener>().PlayForward();
        yield return new WaitForSeconds(0.05f);
        isShowPanel = true;
    }
    #endregion
}

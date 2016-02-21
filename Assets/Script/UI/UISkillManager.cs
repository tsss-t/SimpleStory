using UnityEngine;
using System.Collections;

public class UISkillManager : MonoBehaviour
{
    #region para
    bool isShowPanel;
    UIController mainControllerUI;
    UILabel skillNameLabel;
    UILabel skillDesLabel;
    UIButton upgradeButton;
    UILabel upgradeLabel;
    Skill selectedSkill;
    #endregion
    // Use this for initialization
    #region Start Init
    void Start()
    {
        isShowPanel = false;
        mainControllerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).GetComponent<UIController>();
        skillNameLabel = transform.Find("BG/BG/SkillNameLabel").gameObject.GetComponent<UILabel>();
        skillDesLabel = transform.Find("BG/BG/SkillDesLabel").gameObject.GetComponent<UILabel>();
        upgradeButton = transform.Find("BG/UpgradeButton").GetComponent<UIButton>();
        upgradeLabel = transform.Find("BG/UpgradeButton/Label").GetComponent<UILabel>();
        EventDelegate upgradeButtonClickEvent = new EventDelegate(this, "OnUpgradeButtonClick");
        upgradeButton.onClick.Add(upgradeButtonClickEvent);
        
        Init();
        this.gameObject.SetActive(false);
    }
    void Init()
    {
        skillNameLabel.text = "";
        skillDesLabel.text = "";

        DisabelUpgradeButton("スキルを選ぶ");
    }
    // Update is called once per frame

    void UpdatePanel()
    {
        skillNameLabel.text = string.Format("{0} Lv.{1}", selectedSkill.Name, selectedSkill.Level);
        skillDesLabel.text = string.Format("スキルの攻撃力は{0}、次のレベルの攻撃力は{1}、レベルアップ必要なコインは{2}", selectedSkill.Damage * selectedSkill.Level, selectedSkill.Damage * (selectedSkill.Level + 1), selectedSkill.needMoney());

        if (PlayerState._instance.money > selectedSkill.needMoney())
        {
            EnableUpgradeButton();
        }
        else
        {
            DisabelUpgradeButton("コイン不足");
        }
    }
    #endregion
    #region UI Event
    void DisabelUpgradeButton(string label = "")
    {
        upgradeButton.SetState(UIButtonColor.State.Disabled, true);
        upgradeButton.GetComponent<Collider>().enabled = false;
        if (label != "")
        {
            upgradeLabel.text = label;
        }
    }
    void EnableUpgradeButton(string label = "レベルアップ")
    {
        upgradeButton.SetState(UIButtonColor.State.Normal, true);
        upgradeButton.GetComponent<Collider>().enabled = true;
        if (label != "")
        {
            upgradeLabel.text = label;
        }
    }
    public void OnSkillButtonClick(Skill skill)
    {
        selectedSkill = skill;
        UpdatePanel();
    }
    public void OnUpgradeButtonClick()
    {
        if (PlayerState._instance.SkillUp(selectedSkill.needMoney()))
        {
            selectedSkill.Level += 1;
            UpdatePanel();
        }
    }
    public void OnCloseButtonClick()
    {
        Hide();
    }
    public void OnOpenButtonClick()
    {
        Show();
    }
    public void OnToggleButtonClick()
    {
        if (this.gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    #endregion
    #region UI Action
    void Show()
    {
        if (!isShowPanel)
        {
            this.gameObject.SetActive(true);
            mainControllerUI.CloseAllWindows();
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

    #region Panel CUTIN/OUT
    IEnumerator HidePanel()
    {

        this.GetComponent<UITweener>().PlayReverse();
        yield return new WaitForSeconds(this.transform.GetComponent<UITweener>().duration);
        this.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        this.GetComponent<UITweener>().PlayForward();
        yield return new WaitForSeconds(0.05f);
        isShowPanel = true;
    }
    #endregion
    #endregion


}

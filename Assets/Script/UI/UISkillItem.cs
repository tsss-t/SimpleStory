using UnityEngine;
using System.Collections;

public class UISkillItem : MonoBehaviour {
    public PosType posType;
    private Skill skill;
    UISprite skillSprite;
    UIButton skillButton;
    void Awake()
    {
        skillSprite = gameObject.GetComponent<UISprite>();
        skillButton = gameObject.GetComponent<UIButton>();
        UpdateShow();
    }
	// Update is called once per frame
	void UpdateShow () {
        skill = SkillManager._instance.GetSkillByPosition(posType);
        skillSprite.spriteName = skill.Icon;
        skillButton.normalSprite = skill.Icon;
    }

    void OnClick()
    {
        transform.parent.parent.parent.GetComponent<UISkillManager>().OnSkillButtonClick(skill);
    }

}

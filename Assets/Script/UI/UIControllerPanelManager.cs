using UnityEngine;
using System.Collections;

public class UIControllerPanelManager : MonoBehaviour {
    GameObject skillContainer;
    TweenPosition skillContainerTweener;
    void Awake()
    {
        //skillContainer = transform.Find("SkillContainer").gameObject;
        //skillContainerTweener = skillContainer.GetComponent<TweenPosition>();
        //skillContainerTweener.to = skillContainerTweener.gameObject.transform.position;
        //skillContainerTweener.from = new Vector3(skillContainerTweener.gameObject.transform.position.x + skillContainer.GetComponent<UIWidget>().width, skillContainerTweener.gameObject.transform.position.y, skillContainerTweener.gameObject.transform.position.z);
    }

    public void OnBasicButtonClick()
    {
        PlayerAttack._instance.Attack( SkillType.basic,0);
    }
    public void OnSkill1ButtonClick()
    {
        PlayerAttack._instance.Attack(SkillType.skill, PosType.one);
    }
}

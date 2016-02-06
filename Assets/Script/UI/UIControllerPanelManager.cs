using UnityEngine;
using System.Collections;

public class UIControllerPanelManager : MonoBehaviour {
    PlayerController controller;
    GameObject skillContainer;
    TweenPosition skillContainerTweener;
    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();

        //skillContainer = transform.Find("SkillContainer").gameObject;
        //skillContainerTweener = skillContainer.GetComponent<TweenPosition>();
        //skillContainerTweener.to = skillContainerTweener.gameObject.transform.position;
        //skillContainerTweener.from = new Vector3(skillContainerTweener.gameObject.transform.position.x + skillContainer.GetComponent<UIWidget>().width, skillContainerTweener.gameObject.transform.position.y, skillContainerTweener.gameObject.transform.position.z);
    }

    public void OnBasicButtonClick()
    {
        controller.UseSkill( SkillType.basic);
    }

}

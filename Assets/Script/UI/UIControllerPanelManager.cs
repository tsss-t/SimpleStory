using UnityEngine;
using System.Collections;

public class UIControllerPanelManager : MonoBehaviour {
    CharaControler controller;

    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<CharaControler>();

    }

    public void OnBasicButtonClick()
    {
        controller.UseSkill( SkillType.basic);
    }

}

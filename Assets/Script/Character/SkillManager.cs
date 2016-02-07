using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager _instance;
    private List<Skill> skillList = new List<Skill>();

    void Awake()
    {
        _instance = this;
        InitSkill();
    }


    void InitSkill()
    {
        skillList = GameController._instance.LoadSkill();
    }
    public Skill GetSkillByPosition(PosType type)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].PosType == type)
            {
                return skillList[i];
            }
        }
        return null;
    }


}

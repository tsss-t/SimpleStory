using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    public static SkillManager _instance;

    public TextAsset skillInfoText;
    private ArrayList skillList = new ArrayList();

    void Awake()
    {
        _instance = this;
        InitSkill();
    }


    void InitSkill()
    {
        string[] skillArray = skillInfoText.ToString().Split('\n');
        foreach (string item in skillArray)
        {
            string[] proArray = item.Split(',');
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
            skill.Level = 1;
            skillList.Add(skill);

        }
    }
    public Skill GetSkillByPosition(PosType type)
    {
        foreach (Skill skill in skillList)
        {
            if (skill.PosType == type)
            {
                return skill;
            }
        }
        return null;
    }


}

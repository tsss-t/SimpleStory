
public enum SkillType
{
    basic, skill
}
public enum PosType
{
    basic, one, two, three
}
public class Skill
{
    int id;
    string name;
    string icon;
    SkillType skillType;
    PosType posType;
    int coldTime;
    int damage;
    int level = 1;
    float attackDis = 5;
    float effectDis = 7.5f;
    int enegy;
    #region get/set
    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public SkillType SkillType
    {
        get
        {
            return skillType;
        }

        set
        {
            skillType = value;
        }
    }

    public PosType PosType
    {
        get
        {
            return posType;
        }

        set
        {
            posType = value;
        }
    }

    public int ColdTime
    {
        get
        {
            return coldTime;
        }

        set
        {
            coldTime = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public float AttackDis
    {
        get
        {
            return attackDis;
        }

        set
        {
            attackDis = value;
        }
    }

    public float EffectDis
    {
        get
        {
            return effectDis;
        }

        set
        {
            effectDis = value;
        }
    }

    public int Enegy
    {
        get
        {
            return enegy;
        }

        set
        {
            enegy = value;
        }
    }
    #endregion;

    public int needMoney()
    {
        return level * 500;
    }
}

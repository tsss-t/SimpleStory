
public class PlayerStateData
{
    int exp;
    int baseSTR;
    int baseDEX;
    int baseINT;
    int baseCON;
    int baseLUK;
    int money;
    bool isWalk;
    PlayerType type;
    public int EXP
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    public int BaseSTR
    {
        get
        {
            return baseSTR;
        }

        set
        {
            baseSTR = value;
        }
    }

    public int BaseDEX
    {
        get
        {
            return baseDEX;
        }

        set
        {
            baseDEX = value;
        }
    }

    public int BaseINT
    {
        get
        {
            return baseINT;
        }

        set
        {
            baseINT = value;
        }
    }

    public int BaseCON
    {
        get
        {
            return baseCON;
        }

        set
        {
            baseCON = value;
        }
    }

    public int BaseLUK
    {
        get
        {
            return baseLUK;
        }

        set
        {
            baseLUK = value;
        }
    }

    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            money = value;
        }
    }

    public bool IsWalk
    {
        get
        {
            return isWalk;
        }

        set
        {
            isWalk = value;
        }
    }

    public PlayerType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }
}

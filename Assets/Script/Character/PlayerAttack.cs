using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack _instance;


    private PlayerState playerState;
    Dictionary<string, AttackEffect> AttackEffectDictinary;

    AttackEffect[] normalAttackEffects;
    public AttackEffect[] specialAttackEffects;
    GameObject[] canAttackEnemy;
    private Animator anim;
    private HashIDs hash;
    bool isAttack;

    // Use this for initialization
    void Awake()
    {
        _instance = this;
        playerState = PlayerState._instance;
        AttackEffectDictinary = new Dictionary<string, AttackEffect>();
        normalAttackEffects = this.GetComponentsInChildren<AttackEffect>();
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
        foreach (AttackEffect attackEffect in normalAttackEffects)
        {
            AttackEffectDictinary.Add(attackEffect.gameObject.name, attackEffect);
        }
        foreach (AttackEffect attackEffect in specialAttackEffects)
        {
            AttackEffectDictinary.Add(attackEffect.gameObject.name, attackEffect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.PlayerAliveNow)
        {
            isAttack = Input.GetButtonDown("Attack");
            //basic Attack
            if (isAttack)
            {
                Attack(SkillType.basic, 0);
            }
        }
    }
    #region 攻撃
    Vector3 enemyLocalPosition;
    /// <summary>
    /// ノーマル攻撃力の攻撃効果（特效）,Animatorから読み出し
    /// </summary>
    /// <param name="args">攻撃変数</param>
    void ShowAttack(string args)
    {
        string[] proArray = args.Split(',');
        PosType type = (PosType)int.Parse(proArray[0]);
        string effectName = proArray[1];

        AttackEffect getEffect;
        AttackEffectDictinary.TryGetValue(effectName, out getEffect);

        getEffect.ShowAttack();
        //攻撃判定
        canAttackEnemy = EnemyManager._instance.getAttackEnemy();
        switch (type)
        {
            case PosType.basic:
                {
                    for (int i = 0; i < canAttackEnemy.Length; i++)
                    {
                        if (AttackFront(canAttackEnemy[i].transform.position))
                        {
                            canAttackEnemy[i].GetComponent<EnemyController>().Hit(playerState.ATK);
                        }
                    }
                    break;
                }
            case PosType.one:
                {
                    for (int i = 0; i < canAttackEnemy.Length; i++)
                    {
                        if (AttackAround(canAttackEnemy[i].transform.position))
                        {
                            canAttackEnemy[i].GetComponent<EnemyController>().Hit(SkillManager._instance.GetSkillByPosition(PosType.one).Damage);
                        }
                    }
                    break;
                }
        }
    }

    #region 攻撃判定
    /// <summary>
    /// //目の前の攻撃判定
    /// </summary>
    /// <param name="enemyPosition">敵の位置（ワールド座標）</param>
    /// <returns></returns>
    public bool AttackFront(Vector3 enemyPosition)
    {
        enemyLocalPosition = transform.InverseTransformPoint(enemyPosition);

        if (enemyLocalPosition.z > 0.0f)
        {
            //Debug.Log(playerState.GetAttackDis());
            if (Vector3.Distance(enemyPosition, transform.position) < playerState.GetAttackDis())
            {
                //Debug.Log("hit!");
                return true;
            }
        }
        return false;
    }
    public bool AttackAround(Vector3 enemyPosition)
    {
        if (Vector3.Distance(enemyPosition, transform.position) < playerState.GetAttackDis())
        {
            //Debug.Log("hit!");
            return true;
        }
        return false;
    }
    #endregion
    
    /// <summary>
    /// 動画管理
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="posType"></param>
    public void Attack(SkillType skillType, PosType posType)
    {
        //今回の攻撃が未完成時攻撃を入力すると、次の攻撃を備える
        switch (skillType)
        {
            case SkillType.basic:
                {
                    if ((anim.GetCurrentAnimatorStateInfo(1).fullPathHash == (hash.EmptyState) ||
                        anim.GetCurrentAnimatorStateInfo(1).fullPathHash == (hash.NormalAttack1State) ||
                        anim.GetCurrentAnimatorStateInfo(1).fullPathHash == (hash.NormalAttack2State)) &&
                        CanAttack(SkillType.basic, 0))
                    {
                        anim.SetTrigger(hash.AttackTrigger);
                    }
                    break;
                }
            case SkillType.skill:
                {
                    switch (posType)
                    {
                        case PosType.one:
                            {
                                if (anim.GetCurrentAnimatorStateInfo(1).fullPathHash == (hash.EmptyState) && CanAttack(SkillType.skill, PosType.one))
                                {
                                    anim.SetTrigger(hash.Skill1Trigger);
                                }
                                break;
                            }
                        case PosType.two:
                            {
                                if (anim.GetCurrentAnimatorStateInfo(1).fullPathHash == (hash.EmptyState) && CanAttack(SkillType.skill, PosType.two))
                                {
                                    anim.SetTrigger(hash.Skill2Trigger);
                                }
                                break;
                            }
                        case PosType.three:
                            {
                                if (anim.GetCurrentAnimatorStateInfo(1).fullPathHash == (hash.EmptyState) && CanAttack(SkillType.skill, PosType.three))
                                {
                                    anim.SetTrigger(hash.Skill3Trigger);
                                }
                                break;
                            }
                    }
                    break;
                }
        }
    }
    /// <summary>
    /// 攻撃動作による、体力足りるか否や
    /// </summary>
    /// <param name="skillType">攻撃種類</param>
    /// <param name="posType">スキル種類</param>
    /// <returns></returns>
    bool CanAttack(SkillType skillType, PosType posType)
    {

        switch (skillType)
        {
            case SkillType.basic:
                {
                    if (playerState.energy < 20)
                    {
                        return false;
                    }
                    break;
                }
            case SkillType.skill:
                {
                    switch (posType)
                    {
                        case PosType.one:
                            {
                                if (playerState.energy < 60)
                                {
                                    return false;
                                }
                                break;
                            }
                        case PosType.two:
                            {
                                if (playerState.energy < 40)
                                {
                                    return false;
                                }
                                break;
                            }
                        case PosType.three:
                            {
                                if (playerState.energy < 70)
                                {
                                    return false;
                                }
                                break;
                            }
                    }
                    break;
                }
        }
        return true;
    }
    /// <summary>
    /// Energy消費
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="posType"></param>
    public void UseEnergy(SkillType skillType, PosType posType)
    {

        switch (skillType)
        {
            case SkillType.basic:
                {
                    playerState.UseEnergy(20);
                    break;
                }
            case SkillType.skill:
                {
                    switch (posType)
                    {
                        case PosType.one:
                            {
                                playerState.UseEnergy(60);
                                break;
                            }
                        case PosType.two:
                            {
                                playerState.UseEnergy(40);
                                break;
                            }
                        case PosType.three:
                            {
                                playerState.UseEnergy(70);
                                break;
                            }
                    }
                    break;
                }
        }
    }

    public void ShowEffectSelfToTarget(string effectName)
    {
        AttackEffect effect;
        AttackEffectDictinary.TryGetValue(effectName,out effect);
        canAttackEnemy = EnemyManager._instance.getAttackEnemy();
        foreach (GameObject go in canAttackEnemy)
        {
            GameObject goEffect = (GameObject.Instantiate(effect) as AttackEffect).gameObject;
            goEffect.transform.position = transform.position + Vector3.up;
            goEffect.GetComponent<EffectSettings>().Target = go;
        }
    }

    #endregion
}

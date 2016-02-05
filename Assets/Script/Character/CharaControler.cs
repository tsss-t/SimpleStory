using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 主人公の動作、動作だけ！
/// </summary>
public class CharaControler : MonoBehaviour
{

    #region para
    PlayerState playerState;

    Dictionary<string, NormalAttackEffect> normalAttackDictinary;
    NormalAttackEffect[] normalAttackEffects;
    EnemyManager enemyManager;
    public GameObject[] canAttackEnemy;
    private Animator anim;
    private HashIDs hash;
    private float timerEnergy = 0f;
    private float timerHP = 0f;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.3f;

    public float velocity = 5.5f;
    public float runSpeed = 5.5f;
    public float walkSpeed = 3f;

    #endregion
    #region start
    // Use this for initialization
    void Start()
    {
        playerState = PlayerState.GamePlayerState;
        normalAttackEffects = this.GetComponentsInChildren<NormalAttackEffect>();
        normalAttackDictinary = new Dictionary<string, NormalAttackEffect>();
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager).GetComponent<EnemyManager>();
        foreach (NormalAttackEffect attackEffect in normalAttackEffects)
        {
            normalAttackDictinary.Add(attackEffect.gameObject.name, attackEffect);
        }


        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
        anim.SetLayerWeight(1, 1f);

        //test
        playerState.PlayerStateChanged(PlayerStateChangeType.all);
    }
    #endregion
    #region Update
    bool isAttack;
    // Update is called once per frame
    void Update()
    {
        if (playerState.PlayerAliveNow)
        {
            playerState.playerTransform = transform;
            timerEnergy += Time.deltaTime;
            timerHP += Time.deltaTime;
            if (timerEnergy > playerState.energyUP)
            {
                timerEnergy = 0f;
                EnergyUp();
            }
            if (timerHP > 1f)
            {
                timerHP = 0f;
                Health();
            }


            isAttack = Input.GetButtonDown("Attack");
            //basic Attack
            if(isAttack)
            {
                Attack( SkillType.basic);
            }
            if (Input.GetButtonDown("Walk"))
            {
                WalkChange();
            }
        }
    }

    void FixedUpdate()
    {
        if (playerState.PlayerAliveNow)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            MovementManagement(h, v);
        }
        else
        {
            anim.SetFloat(hash.speedFloat, 0f);
        }
    }
    #endregion
    #region 状態変更（歩く）
    /// <summary>
    /// 歩くと走るの切り替え　ｚボタン
    /// </summary>
    void WalkChange()
    {
        if (playerState.isWalk)
        {
            playerState.isWalk = false;
            velocity = runSpeed;
        }
        else
        {
            playerState.isWalk = true;
            velocity = walkSpeed;
        }

    }
    #endregion

    #region 移動
    /// <summary>
    /// 主人公の移動
    /// </summary>
    /// <param name="horizontal">水平移動値</param>
    /// <param name="vertical">垂直移動値</param>
    /// <param name="attack">攻撃ボタン</param>
    void MovementManagement(float horizontal, float vertical)
    {
        //攻撃の時、位置固定
        //Debug.Log(anim.GetCurrentAnimatorStateInfo(1).IsName("Attack Layer.Empty"));
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack Layer.Empty"))
        {
            Vector3 nowVel = GetComponent<Rigidbody>().velocity;
            if (Mathf.Abs(horizontal) > 0.05f || Mathf.Abs(vertical) > 0.05f)
            {
                Rotating(horizontal, vertical);
                anim.SetFloat(hash.speedFloat, velocity, speedDampTime, Time.deltaTime);

                GetComponent<Rigidbody>().velocity = new Vector3(velocity * horizontal, nowVel.y, vertical * velocity);
                transform.LookAt(new Vector3(horizontal, 0, vertical) + transform.position);

            }
            else
            {
                anim.SetFloat(hash.speedFloat, 0f);
                GetComponent<Rigidbody>().velocity = new Vector3(0, nowVel.y, 0);
            }
        }
        else
        {

            GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        }

    }
    /// <summary>
    /// 主人公移動中転向
    /// </summary>
    /// <param name="horizontal">水平移動値</param>
    /// <param name="vertical">垂直移動値</param>
    void Rotating(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        GetComponent<Rigidbody>().MoveRotation(newRotation);
    }
    #endregion
    #region 攻撃/被攻撃
    Vector3 enemyLocalPosition;
    /// <summary>
    /// ノーマル攻撃力の攻撃効果（特效）,Animatorから読み出し
    /// </summary>
    /// <param name="effectName"></param>
    void ShowAttack(string effectName)
    {
        NormalAttackEffect getEffect;
        normalAttackDictinary.TryGetValue(effectName, out getEffect);

        getEffect.ShowAttack();
        //攻撃判定
        canAttackEnemy = enemyManager.getAttackEnemy();
        for (int i = 0; i < canAttackEnemy.Length; i++)
        {
            enemyLocalPosition = transform.InverseTransformPoint(canAttackEnemy[i].transform.position);
            //目の前の攻撃判定
            if (enemyLocalPosition.z > 0.0f)
            {
                //Debug.Log(playerState.GetAttackDis());
                if (Vector3.Distance(canAttackEnemy[i].transform.position, transform.position) < playerState.GetAttackDis())
                {
                    canAttackEnemy[i].GetComponent<EnemyController>().Hit(playerState.ATK);
                    //Debug.Log("hit!");
                }
            }

        }
    }
    void Attack(SkillType type)
    {
        //今回の攻撃が未完成時攻撃を入力すると、次の攻撃を備える
        switch (type)
        {
            case SkillType.basic:
                {
                    if (!anim.GetBool(hash.AttackBool) && CanAttack(SkillType.basic))
                    {
                        anim.SetBool(hash.AttackBool, true);
                    }
                    break;
                }

        }
    }
    
    /// <summary>
    /// 攻撃動作による、体力減少
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    bool CanAttack(SkillType type)
    {
        
        switch (type)
        {
            case SkillType.basic:
                {
                    if (playerState.energy < 20)
                    {
                        return false;
                    }
                    break;
                }
        }
        return true;
    }




    void die()
    {
        playerState.PlayerAliveNow = false;
        anim.SetTrigger(hash.dieTrigger);
    }
    #endregion
    #region 自然回復
    void EnergyUp()
    {
        if (playerState.energy < playerState.energyMax)
        {
            playerState.energy += 1;
            playerState.PlayerStateChanged(PlayerStateChangeType.energy);
        }
    }
    void Health()
    {
        if (playerState.HP < playerState.HPMax)
        {
            if (playerState.HP + playerState.health > playerState.HPMax)
            {
                playerState.HP = playerState.HPMax;
            }
            else
            {
                playerState.HP += (int)playerState.health;
            }
            playerState.PlayerStateChanged(PlayerStateChangeType.HP);

        }
    }
    #endregion
    #region 外部API
    /// <summary>
    /// 被攻撃
    /// </summary>
    /// <param name="enemyATK"></param>
    /// <param name="ACC"></param>
    public void Hit(int enemyATK, int ACC)
    {
        //TODO:攻撃のダメージ算出
        anim.SetTrigger(hash.hitTrigger);
        int damege = enemyATK - playerState.DEF;
        if (damege <= 0)
        {
            damege = 1;
        }
        playerState.HP -= damege;
        if (playerState.HP <= 0)
        {
            playerState.HP = 0;
            die();
        }
        playerState.PlayerStateChanged(PlayerStateChangeType.HP);
    }
    public void UseSkill(SkillType type)
    {
        Attack(type);
    }

    #endregion

}

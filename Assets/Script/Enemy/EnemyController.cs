using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ActionType
{
    idel, attack, run, hit, die
}
public enum ActionState
{
    foundPlayer, notFoundPlayer, goBack, die
}
public class EnemyController : MonoBehaviour
{
    #region　para
    public int enemyID;//敵のID
    public float fieldDistance;//監視距離
    public float fieldAngle;//監視範囲

    //enemy state
    public int HP;
    public int HPMax;
    public int ATK;
    public int DEF;
    public int ACC;
    public int EXP;
    public int level;
    public int speed = 2;
    public float speedDampTime = 0.03f;

    public float actiontimer = 5;
    public float attackkDelay = 3f;

    public float followDis = 25f;
    public float patrolDis = 10f;
    public float attackDis = 1.25f;

    public float HPbarDis = 20f;

    public ActionEvent[] actionList;
    private ActionEvent[] normalActionList;
    private ActionEvent[] attackActionList;
    private ActionEvent[] hitActionList;
    private ActionEvent[] dieActionList;

    private float timer;
    private float attackTimer;
    private PlayerState playerState;
    private Vector3 playerPosition;
    private CharaControler charaControler;

    private ActionState nowState;
    private ActionType nowAction;

    private Vector3 positionStart;
    private Animator anim;
    private HashIDs hash;
    private Vector2 moveForward;
    private EnemyManager enemyManager;
    private UIHpBarManager hpBarManager;
    private GameObject hpBar;
    private UISlider hpSlider;


    #endregion
    #region start
    // Use this for initialization
    void Start()
    {
        positionStart = new Vector3(transform.position.x, transform.position.y, transform.position.z); ;
        playerState = PlayerState.GamePlayerState;
        nowState = ActionState.notFoundPlayer;
        playerPosition = playerState.playerTransform.position;
        normalActionList = actionList.GetNormalActionEvents();
        attackActionList = actionList.GetAttackActionEvents();
        dieActionList = actionList.GetDieActionEvents();
        hitActionList = actionList.GetHitActionEvents();


        anim = this.GetComponent<Animator>();
        timer = 0f;
        hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
        charaControler = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<CharaControler>();
        moveForward = new Vector2(transform.forward.x, transform.forward.z);
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager).GetComponent<EnemyManager>();
        hpBarManager = UIHpBarManager.hpBarManager;
        hpBar = hpBarManager.CreateHpBar(transform.Find("HpBarPoint").gameObject);
        hpSlider = hpBar.transform.GetComponentInChildren<UISlider>();
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        if (nowState != ActionState.die)
        {
            HPBarSee();
            switch (nowState)
            {
                case ActionState.notFoundPlayer:
                    {

                        //TODO:監視コード animation:なし
                        //    public float fieldDistance;監視距離
                        //    public float fieldAngle;監視範囲
                        See();


                        //TODO：行動ランダムコード　animation:random
                        Action();

                        break;
                    }
                case ActionState.foundPlayer:
                    {
                        if (!playerState.PlayerAliveNow)
                        {
                            nowState = ActionState.notFoundPlayer;
                            break;
                        }
                        playerPosition = playerState.playerTransform.position;
                        //TODO:追撃コード animation:run
                        Follow();
                        //TODO:攻撃コード animation:attack
                        Attack();

                        break;
                    }
                case ActionState.goBack:
                    {
                        GoBack();
                        //TODO:追撃停止
                        break;
                    }
            }
        }

        //BUG防止
        if(gameObject.transform.position.y<-100)
        {
            Die();
        }
    }
    #endregion
    #region HPBar管理
    void HPBarSee()
    {
        if (Vector3.Distance(playerState.playerTransform.position, transform.position) < 20f)
        {
            hpBar.SetActive(true);
        }
        else
        {
            hpBar.SetActive(false);
        }
    }
    #endregion
    #region 監視+判定　コード
    /// <summary>
    /// 監視
    /// </summary>
    private void See()
    {
        playerPosition = playerState.playerTransform.position;
        //監視の条件：１、距離として、監視距離内に入りました（効率のため）　２、プレーヤーまだ生きてる、死んだら監視しない
        if (Vector3.Distance(playerState.playerTransform.position, transform.position) < fieldDistance && playerState.PlayerAliveNow)
        {
            //Quaternion r = transform.rotation;
            //Vector3 forwordPosition = (transform.position + (r * Vector3.forward) * fieldDistance);
            //Debug.DrawLine(transform.position, f0, Color.red);

            Quaternion leftAngel = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - fieldAngle, transform.rotation.eulerAngles.z);
            Quaternion rightAngel = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + fieldAngle, transform.rotation.eulerAngles.z);

            Vector3 leftPosition = (transform.position + (leftAngel * Vector3.forward) * fieldDistance);
            Vector3 rightPosition = (transform.position + (rightAngel * Vector3.forward) * fieldDistance);

            //Debug.DrawLine(transform.position, f1, Color.red);
            //Debug.DrawLine(transform.position, f2, Color.red);1
            //Debug.DrawLine(f1, f2, Color.red);


            if (isINTriangle(playerPosition, transform.position, leftPosition, rightPosition))
            {
                //Debug.Log("player in this !!!");
                nowState = ActionState.foundPlayer;

            }
            else
            {
                //Debug.Log("player not in this !!!");
            }

        }

    }
    /// <summary>
    /// 位置判定
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="myPosition"></param>
    /// <param name="leftPosition"></param>
    /// <param name="rightPosition"></param>
    /// <returns></returns>
    bool isINTriangle(Vector3 playerPosition, Vector3 myPosition, Vector3 leftPosition, Vector3 rightPosition)
    {
        float x = playerPosition.x;
        float y = playerPosition.z;

        //平面化
        float v0x = myPosition.x;
        float v0y = myPosition.z;

        float v1x = leftPosition.x;
        float v1y = leftPosition.z;

        float v2x = rightPosition.x;
        float v2y = rightPosition.z;

        float t = triangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = triangleArea(v0x, v0y, v1x, v1y, x, y) + triangleArea(v0x, v0y, x, y, v2x, v2y) + triangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float triangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }

    #endregion
    #region　行動ランダムコード
    private void Action()
    {
        timer += Time.deltaTime;
        if (timer > actiontimer)
        {
            ActionEvent playEvent = actionList[GetRandomEvent(normalActionList.GetProbs())];
            nowAction = playEvent.actionType;
            if (playEvent.actionType == ActionType.run)
            {

                anim.SetFloat(hash.enemySpeedFloat, speed);
                float x = Random.value;
                float z = Mathf.Sqrt(1 - x * x);
                if (Random.value > 0.5f)
                {
                    x = -x;
                }
                if (Random.value > 0.5f)
                {
                    z = -z;
                }
                moveForward.Set(x, z);
            }
            else if (playEvent.actionType == ActionType.idel)
            {
                anim.SetFloat(hash.enemySpeedFloat, 0f);
            }
            transform.LookAt(new Vector3(moveForward.x, 0, moveForward.y) + transform.position);
            timer = 0f;
        }
        else
        {
            switch (nowAction)
            {
                case ActionType.idel:
                    {
                        anim.SetFloat(hash.enemySpeedFloat, 0f);
                        break;
                    }
                case ActionType.run:
                    {
                        if (Vector3.Distance(positionStart, transform.position) > patrolDis)
                        {
                            transform.LookAt(positionStart);
                        }
                        else
                        {
                        }

                        anim.SetFloat(hash.enemySpeedFloat, speed, speedDampTime, Time.deltaTime);
                        transform.GetComponent<CharacterController>().SimpleMove(transform.forward * speed);
                        //GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, nowVel.y, speed * transform.forward.z);
                        break;
                    }
            }

        }

    }
    /// <summary>
    /// ランダム方法
    /// </summary>
    /// <param name="probs">各事件に対する確率</param>
    /// <returns>第何番の事件</returns>
    private int GetRandomEvent(int[] probs)
    {
        int total = 0;
        for (int i = 0; i < probs.Length; i++)
        {
            total += probs[i];
        }
        //Random.value：０から１までの値を生成
        float randomPoint = Random.value * total;
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }

        return probs.Length - 1;
    }

    #endregion
    #region 追撃コード
    public void Follow()
    {

        if (nowAction == ActionType.run || nowAction == ActionType.idel)
        {
            transform.LookAt(playerPosition);


            if (Vector3.Distance(transform.position, playerPosition) > attackDis)
            {
                anim.SetFloat(hash.enemySpeedFloat, speed, speedDampTime, Time.deltaTime);
                transform.GetComponent<CharacterController>().SimpleMove(transform.forward * speed);
                //GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, GetComponent<Rigidbody>().velocity.y, speed * transform.forward.z);
            }
            else
            {
                anim.SetFloat(hash.enemySpeedFloat, 0f);
            }
            if (Vector3.Distance(positionStart, transform.position) > followDis)
            {
                nowState = ActionState.goBack;
            }
        }

    }
    #endregion
    #region 攻撃コード
    private ActionEvent attackAction;
    public void Attack()
    {
        SetAction(ActionType.attack, 1);
        SetAction(ActionType.hit, 2);
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if (nowAction == ActionType.run)
            {
                if (Vector3.Distance(transform.position, playerPosition) <= attackDis)
                {
                    if (attackActionList.Length == 1)
                    {
                        attackAction = attackActionList[0];
                    }
                    else
                    {
                        attackAction = attackActionList[GetRandomEvent(attackActionList.GetProbs())];
                    }

                    anim.SetTrigger(attackAction.actionTrigerName);
                    charaControler.Hit(ATK, ACC);
                    nowAction = ActionType.attack;
                    attackTimer = attackkDelay;
                    if (!playerState.PlayerAliveNow)
                    {
                        nowState = ActionState.notFoundPlayer;
                    }
                }
                //Debug.Log(Vector3.Distance(transform.position, playerPosition));
            }
        }
    }
    #endregion
    #region 追撃停止、原点に戻る
    public void GoBack()
    {
        transform.LookAt(positionStart);
        anim.SetFloat(hash.enemySpeedFloat, speed, speedDampTime, Time.deltaTime);
        transform.GetComponent<CharacterController>().SimpleMove(transform.forward * speed);
        //GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, GetComponent<Rigidbody>().velocity.y, speed * transform.forward.z);
        if (Vector3.Distance(positionStart, transform.position) < 0.5f)
        {
            nowState = ActionState.notFoundPlayer;
        }
    }
    #endregion

    #region  nowActionとアクション動作動画と関連
    ActionEvent[] tempActionList;
    /// <summary>
    /// nowActionとアクション動作動画と関連
    /// </summary>
    /// <param name="type">関連するアクションタイプ</param>
    public void SetAction(ActionType type, int indexLayer)
    {
        if (type == ActionType.attack)
        {
            tempActionList = attackActionList;
        }
        else if (type == ActionType.hit)
        {
            tempActionList = hitActionList;
        }

        for (int i = 0; i < tempActionList.Length; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(indexLayer).IsName(tempActionList[i].actionTrigerName))
            {
                nowAction = type;
                break;
            }
            else
            {
                nowAction = ActionType.run;
            }
        }
    }
    #endregion

    #region 被攻撃
    ActionEvent hitAction;
    ActionEvent dieAction;
    int damage;
    /// <summary>
    /// 被攻撃
    /// </summary>
    /// <param name="ATK">プレイヤーの攻撃力</param>
    public void Hit(int ATK)
    {
        if (nowState != ActionState.die)
        {
            if (hitActionList.Length == 1)
            {
                hitAction = hitActionList[0];
            }
            else
            {
                hitAction = hitActionList[GetRandomEvent(hitActionList.GetProbs())];
            }

            damage = ATK - DEF >= 0 ? ATK - DEF : 1;
            //Debug.Log(ATK+"  damage:" +damage);
            HP -= damage;
            //Debug.Log(HP / (float)HPMax);
            hpSlider.value = HP / (float)HPMax;
            if (HP <= 0)
            {
                Die();
            }
            else
            {
                anim.SetTrigger(hitAction.actionTrigerName);
            }
            if (nowState == ActionState.notFoundPlayer)
            {
                nowState = ActionState.foundPlayer;
            }

        }

    }

    private void Die( )
    {
        HP = 0;

        nowState = ActionState.die;
        //ランダムの死亡動画
        if (dieActionList.Length == 1)
        {
            dieAction = dieActionList[0];
        }
        else
        {
            dieAction = dieActionList[GetRandomEvent(dieActionList.GetProbs())];
        }
        anim.SetTrigger(dieAction.actionTrigerName);

        playerState.KillEnemy(enemyID,EXP,level);



        StartCoroutine(AfterDie());
        this.GetComponent<CharacterController>().enabled = false;

    }


    #endregion

    #region 
    float height = 5;
    Vector3 targetPos;
    IEnumerator AfterDie()
    {
        
        hpBarManager.DestoryHpBar(hpBar);
        targetPos = new Vector3(transform.position.x, transform.position.y - height, transform.position.z);
        for (float timer = 0; timer < 10f; timer += Time.deltaTime)
            yield return 0;
        while (transform.position.y - 5f > targetPos.y)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f * Time.deltaTime);

            for (float timer = 0; timer < 0.01f; timer += Time.deltaTime)
                yield return 0;
        }
        enemyManager.destroyEnemy(gameObject);

        Destroy(this.gameObject);

        yield return 0;
    }
    #endregion
}

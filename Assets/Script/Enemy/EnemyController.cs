using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyController : Enemy
{
    #region　para
    ///監視距離
    public float fieldDistance = 10;
    ///監視範囲
    public float fieldAngle = 30;
    public int addATK = 20;
    public int addDEF = 5;
    public int addHP = 10;
    public int baseATK = 100;
    public int baseDEF = 10;
    public int baseHP = 1000;
    #region enemy state

    //公式：
    //HPMax = level*addLevelHP+baseHP
    //ATK   = level*addLevelATK+baseATK
    //DEF   = level*addLevelDEF+baseDEF
    //ACC   = playerLevel - enemyLevel > 0 ? 70% + playerDEX * 0.5% : ( 70% -  (enemyLevel - playerLevel)) <= 0 ? playerDEX * 0.5% : (( 70% -  (enemyLevel - playerLevel) * 15% + playerDEX * 0.5%): 

    //public int speed = 2;
    //public float speedDampTime = 0.03f;

    public float actiontimer = 5;
    public float attackDelay = 3f;

    public float followDis = 25f;
    public float patrolDis = 10f;
    public float attackDis = 1.25f;

    //public float HPbarDis = 20f;
    #endregion


    private float timer;
    private float attackTimer;

    private Vector3 positionStart;
    private Vector2 moveForward;

    #region UI
    private UIHpBarManager hpBarManagerUI;
    private GameObject hpBar;
    private UISlider hpSlider;
    #endregion

    private CharacterController charaController;


    #endregion
    #region start
    protected override void Start()
    {
        base.Start();
        HPMax = level * addHP + baseHP;
        ATK = level * addATK + baseATK;
        DEF = level * addDEF + baseDEF;
        HP = HPMax;
        if (HP == 0)
        {
            HP = level * 100;
        }


        positionStart = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (nowState != ActionState.locked)
        {
            nowState = ActionState.notFoundPlayer;
        }

        timer = 0f;

        moveForward = new Vector2(transform.forward.x, transform.forward.z);


        //HPbar
        hpBarManagerUI = UIHpBarManager._instance;
        hpBar = hpBarManagerUI.CreateHpBar(transform.Find("HpBarPoint").gameObject);
        hpSlider = hpBar.transform.GetComponentInChildren<UISlider>();

        charaController = this.GetComponent<CharacterController>();
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        if (nowState != ActionState.locked)
        {
            if (nowState != ActionState.die)
            {
                checkState();


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
                            RandomAction();
                            break;
                        }
                    case ActionState.foundPlayer:
                        {
                            if (!PlayerState._instance.PlayerAliveNow)
                            {
                                nowState = ActionState.notFoundPlayer;
                                break;
                            }
                            playerPosition = PlayerState._instance.playerTransform.position;
                            //次の動作を設定
                            SetAction();
                            ////TODO:攻撃コード animation:attack
                            Attack();
                            //TODO:追撃コード animation:run
                            Follow();
                            break;
                        }
                    case ActionState.goBack:
                        {
                            GoBack();
                            //TODO:追撃停止
                            break;
                        }
                }
                //BUG防止
                if (gameObject.transform.position.y < -100)
                {
                    BugDie();
                }
            }
            else
            {
                if (charaController.enabled)
                {
                    charaController.SimpleMove(Vector3.zero);
                }
            }
        }
    }
    #endregion
    #region HPBar管理
    void HPBarSee()
    {
        if (hpBar != null)
        {
            if (Vector3.Distance(PlayerState._instance.playerTransform.position, transform.position) < 20f)
            {
                hpBar.SetActive(true);
            }
            else
            {
                hpBar.SetActive(false);
            }
        }
    }
    #endregion
    #region 監視+判定　コード
    /// <summary>
    /// 監視
    /// </summary>
    private void See()
    {
        playerPosition = PlayerState._instance.playerTransform.position;
        //監視の条件：１、距離として、監視距離内に入りました（効率のため）　２、プレーヤーまだ生きてる、死んだら監視しない
        if (Vector3.Distance(PlayerState._instance.playerTransform.position, transform.position) < fieldDistance && PlayerState._instance.PlayerAliveNow)
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


            if (isInTriangle(playerPosition, transform.position, leftPosition, rightPosition))
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
    /// ▽位置判定、指定する点が三角形の中にいるかどうか
    /// </summary>
    /// <param name="playerPosition">判定点座標</param>
    /// <param name="downPosition">▽三角形下の点の座標</param>
    /// <param name="leftPosition">▽三角形左の点の座標</param>
    /// <param name="rightPosition">▽三角形右の点の座標</param>
    /// <returns></returns>
    bool isInTriangle(Vector3 playerPosition, Vector3 downPosition, Vector3 leftPosition, Vector3 rightPosition)
    {
        float x = playerPosition.x;
        float y = playerPosition.z;

        //平面化
        float v0x = downPosition.x;
        float v0y = downPosition.z;

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
    private void RandomAction()
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
                        charaController.SimpleMove(Vector3.zero);
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
                        charaController.SimpleMove(transform.forward * speed);
                        //GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, nowVel.y, speed * transform.forward.z);
                        break;
                    }
            }

        }

    }


    #endregion
    #region 追撃コード
    protected override void Follow()
    {
        base.Follow();
        if (nowAction == ActionType.run || nowAction == ActionType.idel)
        {
            transform.LookAt(playerPosition);
            if (Vector3.Distance(transform.position, playerPosition) > attackDis)
            {
                anim.SetFloat(hash.enemySpeedFloat, speed, speedDampTime, Time.deltaTime);
                charaController.SimpleMove(transform.forward * speed);
                //GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, GetComponent<Rigidbody>().velocity.y, speed * transform.forward.z);
            }
            else
            {
                anim.SetFloat(hash.enemySpeedFloat, 0f);
                charaController.SimpleMove(Vector3.zero);
            }
            if (Vector3.Distance(positionStart, transform.position) > followDis)
            {
                nowState = ActionState.goBack;
            }
        }
    }
    #endregion
    #region 攻撃コード
    protected override void Attack(int attackWeight = 1)
    {
        if (attackTimer >= 0f)
        {
            attackTimer -= Time.deltaTime;

        }
        else
        {
            //攻撃距離判定
            if (Vector3.Distance(transform.position, playerPosition) <= attackDis)
            {
                if (attackActionList.Length == 1)
                {
                    anim.SetTrigger(attackActionList[0].actionTrigerName);
                }
                else
                {
                    anim.SetTrigger(attackActionList[GetRandomEvent(attackActionList.GetProbs())].actionTrigerName);
                }
                base.Attack();
                attackTimer = attackDelay;
            }
        }
    }
    #endregion
    #region 追撃停止、原点に戻る
    private void GoBack()
    {
        transform.LookAt(positionStart);
        anim.SetFloat(hash.enemySpeedFloat, speed, speedDampTime, Time.deltaTime);
        charaController.SimpleMove(transform.forward * speed);
        //GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, GetComponent<Rigidbody>().velocity.y, speed * transform.forward.z);
        if (Vector3.Distance(positionStart, transform.position) < 0.5f)
        {
            nowState = ActionState.notFoundPlayer;
        }
    }
    #endregion

    #region  nowActionとアクション動作動画と関連
    bool playingAttackNow;
    bool playingHit;
    /// <summary>
    /// 現在の動画再生より、今の状態を設定
    /// </summary>
    private void SetAction()
    {
        playingAttackNow = !CheckActionOver(ActionType.attack, 1);
        playingHit = !CheckActionOver(ActionType.hit, 2);
        if (!playingAttackNow && !playingHit)//今は攻撃と受け身両方でもない
        {
            nowAction = ActionType.run;
        }
        else
        {
            if (playingAttackNow)
            {
                nowAction = ActionType.attack;
            }
            else if (playingHit)
            {
                nowAction = ActionType.hit;
            }
        }
    }
    /// <summary>
    /// 指定するタップの動画が再生完了したか
    /// </summary>
    /// <param name="type">関連するアクションタイプ</param>
    /// <param name="indexLayer">動画所属のインデックス</param>
    /// <returns>再生完了=true</returns>
    private bool CheckActionOver(ActionType type, int indexLayer)
    {
        ActionEvent[] tempActionList;
        if (type == ActionType.attack)
        {
            tempActionList = attackActionList;
        }
        else if (type == ActionType.hit)
        {
            tempActionList = hitActionList;
        }
        else
        {
            return false;
        }

        for (int i = 0; i < tempActionList.Length; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(indexLayer).IsName(tempActionList[i].actionTrigerName))
            {
                return false;
            }
        }
        return true;
    }
    #endregion
    #region Lock

    public void Lock()
    {
        this.nowState = ActionState.locked;
    }
    public void UnLock()
    {
        this.nowState = ActionState.foundPlayer;
    }

    #endregion
    #region 被攻撃 / 死亡 / ドロップ

    #region TakeDamage



    /// <summary>
    /// 被攻撃、後退、飛ぶ
    /// </summary>
    /// <param name="ATK"></param>
    /// <param name="jumpDis">飛ぶ距離</param>
    /// <param name="backDis">後退距離</param>
    public void TakeDamage(int ATK, int jumpDis, int backDis)
    {
        TakeDamage(ATK);
        if (nowState != ActionState.die && nowState != ActionState.locked)
        {
            Vector3[] path = new Vector3[3];
            path[0] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            path[1] = path[0] - (PlayerState._instance.playerTransform.position - transform.position).normalized * backDis * 0.5f + Vector3.up * jumpDis;
            path[2] = path[0] - (PlayerState._instance.playerTransform.position - transform.position).normalized * backDis + Vector3.up * 0.3f;

            RaycastHit hitInfo;
            if (Physics.Raycast(path[0], path[1] - path[0], out hitInfo, (path[1] - path[0]).magnitude, LayerMask.GetMask("Building")))
            {
                path[1] = hitInfo.point;
                Physics.Raycast(path[1] + Vector3.up, Vector3.down, out hitInfo, 10f, LayerMask.GetMask("Building"));
                path[2] = hitInfo.point;
            }
            else if (Physics.Raycast(path[2], path[2] - path[1], out hitInfo, (path[2] - path[1]).magnitude, LayerMask.GetMask("Building")))
            {
                Vector3[] newPath = new Vector3[4];
                newPath[0] = path[0];
                newPath[1] = path[1];
                newPath[2] = hitInfo.point;
                Physics.Raycast(path[2] + Vector3.up, Vector3.down, out hitInfo, 10f, LayerMask.GetMask("Building"));
                newPath[3] = hitInfo.point;

                path = newPath;
            }

            while (!Physics.Raycast(path[path.Length - 1] + Vector3.up, Vector3.down, out hitInfo, 10f, LayerMask.GetMask("Building")))
            {
                Debug.Log("drop down!");
                path[path.Length - 1] -= (path[path.Length - 1] - path[0]).normalized * 0.1f;
            }


            iTween.MoveTo(this.gameObject, iTween.Hash("path", path, "time", 0.6f, "easeType", iTween.EaseType.linear));
            this.nowState = ActionState.locked;
            StartCoroutine(ChangeState(0.4f, ActionState.foundPlayer));
        }
        //iTween.MoveBy(this.gameObject, transform.InverseTransformDirection(playerState.playerTransform.forward) * backDis + Vector3.up * jumpDis, 0.3f);
    }
    /// <summary>
    /// 被攻撃、後退
    /// </summary>
    /// <param name="ATK">技の攻撃力</param>
    /// <param name="backDis">後退距離</param>
    public void TakeDamage(int ATK, int backDis)
    {
        iTween.MoveBy(this.gameObject, transform.InverseTransformDirection(PlayerState._instance.playerTransform.forward) * backDis, 0.3f);
        TakeDamage(ATK);
    }
    /// <summary>
    /// 被攻撃
    /// </summary>
    /// <param name="ATK">技の攻撃力</param>

    public override bool TakeDamage(int ATK, bool hitRecover = true)
    {
        if (nowState != ActionState.die)
        {
            if (nowState == ActionState.notFoundPlayer)
            {
                nowState = ActionState.foundPlayer;
            }
            if (base.TakeDamage(ATK))
            {
                hpSlider.value = ((float)HP) / ((float)HPMax);
                if (HP <= 0)
                {
                    this.Die();
                }
                else
                {
                    if (hitActionList.Length == 1)
                    {
                        anim.SetTrigger(hitActionList[0].actionTrigerName);
                    }
                    else
                    {
                        anim.SetTrigger(hitActionList[GetRandomEvent(hitActionList.GetProbs())].actionTrigerName);
                    }

                }
                return true;
            }
        }
        return false;
    }

    #endregion
    #region Die


    protected override void Die()
    {
        base.Die();
        HP = 0;
        nowState = ActionState.die;
        //ランダムの死亡動画
        if (dieActionList.Length == 1)
        {
            anim.SetTrigger(dieActionList[0].actionTrigerName);
        }
        else
        {
            anim.SetTrigger(dieActionList[GetRandomEvent(dieActionList.GetProbs())].actionTrigerName);
        }
        PlayerState._instance.KillEnemy(enemyID, EXP, level);

        Drop();
        StartCoroutine(AfterDie());
    }
    protected override void BugDie()
    {
        base.BugDie();
        charaController.enabled = false;
        StartCoroutine(AfterDie());
    }
    protected override IEnumerator AfterDie()
    {
        hpBarManagerUI.DestoryHpBar(hpBar);
        charaController.enabled = false;
        base.AfterDie();
        yield return 0;
    }
    void checkState()
    {
        if (HP <= 0)
        {
            nowState = ActionState.die;
        }
    }
    IEnumerator ChangeState(float time, ActionState state)
    {
        yield return new WaitForSeconds(time);
        nowState = state;
        yield return 0;
    }
    #endregion

    protected override void Drop()
    {
        base.Drop();
    }
    #endregion


}

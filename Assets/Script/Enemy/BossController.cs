using UnityEngine;
using System.Collections;

public class BossController : Enemy
{
    public int BossATK = 500;
    public int BossDEF = 500;

    public float attackDis = 1.25f;

    public float seeAngle = 50;
    public float rotSpeed = 1;

    public float attackDelay = 3f;

    private float attackTimer;
    private Rigidbody rigidBody;
    private GameObject skill1;
    private GameObject skill2;
    private ParticleSystem[] skill3WeapenFire;
    public GameObject skill3AreaFirePrefab;
    private Transform skill3Position;
    private ParticleSystem[] skill3WeapenLightning;
    public GameObject skill3AreaLightningPrefab;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        base.ATK = BossATK;
        base.DEF = BossDEF;

        skill1 = transform.Find("Skill1").gameObject;
        skill2 = transform.Find("Skill2").gameObject;

        skill3WeapenFire = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger1/Bip01 R Finger11/Skill3Fire").gameObject.GetComponentsInChildren<ParticleSystem>();
        skill3WeapenLightning = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger1/Bip01 R Finger11/Skill3Lightning").gameObject.GetComponentsInChildren<ParticleSystem>();
        skill3Position = transform.Find("Skill3AreaFirePosition").gameObject.transform;
        //skill3 = transform.Find("Skill3").gameObject;
        rigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nowState != ActionState.die)
        {
            if (nowState == ActionState.foundPlayer)
            {
                SetAction();
                Follow();
                Attack();
            }
        }
    }

    #region Follow
    float angle;
    Vector3 playerPos;
    protected override void Follow()
    {
        Debug.Log(playerPos);
        base.Follow();
        playerPos = PlayerController._instance.transform.position;
        angle = Vector3.Angle(playerPos - transform.position, transform.forward);
        if (nowAction == ActionType.run || nowAction == ActionType.idel)
        {
            playerPos.y = this.transform.position.y;
            if (angle > seeAngle * 0.5f || Vector3.Distance(playerPos, transform.position) > attackDis)
            {
                anim.SetFloat(hash.enemySpeedFloat, 5);

                if (angle > seeAngle * 0.5f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(playerPos - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 1 * Time.deltaTime);
                }
                if (Vector3.Distance(playerPos, transform.position) > attackDis)
                {

                    float dis = Vector3.Distance(playerPos, transform.position);

                    rigidBody.MovePosition(transform.position + transform.forward * (speed * (dis > 5 ? 1f : dis * 0.2f)) * Time.deltaTime);
                }
            }
            else
            {
                anim.SetFloat(hash.enemySpeedFloat, 0);

            }
        }


    }
    #endregion
    #region　Attack/Skill
    protected override void Attack(int attackWeight = 1)
    {
        if (attackTimer > 0f)
        {
            if (nowAction != ActionType.attack)
            {
                attackTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (Vector3.Distance(playerPos, transform.position) < attackDis && angle < seeAngle * 0.5f)
            {
                if (attackActionList.Length == 1)
                {
                    anim.SetTrigger(attackActionList[0].actionTrigerName);
                }
                else
                {
                    anim.SetTrigger(attackActionList[GetRandomEvent(attackActionList.GetProbs())].actionTrigerName);
                }



                attackTimer = attackDelay;
            }
            else
            {
                if (Vector3.Distance(playerPos, transform.position) > 10f && angle < seeAngle * 0.5f)
                {
                    anim.SetTrigger(attackActionList[3].actionTrigerName);
                }

                attackTimer = attackDelay * 2;

            }
        }

    }
    #region Skill
    public void Skill1()
    {
        skill1.gameObject.SetActive(true);
        base.Attack(3);
        StartCoroutine(StopEffect(1f, skill1));
    }
    public void SKill2()
    {
        skill2.gameObject.SetActive(true);
        base.Attack(3);
        StartCoroutine(StopEffect(1f, skill2));
    }

    #region skill3   -----  Fire
    public void Skill3WeapenFire()
    {
        for (int i = 0; i < skill3WeapenFire.Length; i++)
        {
            skill3WeapenFire[i].Play();
        }

        StartCoroutine(StopParticle(3f, skill3WeapenFire));
    }
    public void SKill3Boom()
    {
        skill2.gameObject.SetActive(true);
        if (angle < seeAngle && Vector3.Distance(playerPos, transform.position) < attackDis + 5f)
        {
            base.Attack(3);
        }
        StartCoroutine(StopEffect(1f, skill2));
    }


    public void Skill3AreaFire()
    {
        GameObject.Instantiate(skill3AreaFirePrefab, skill3Position.position, Quaternion.identity);
    }

    #endregion

    #region Skill3   ----- Lightning

    public void Skill3WeapenLightning()
    {
        for (int i = 0; i < skill3WeapenLightning.Length; i++)
        {
            skill3WeapenLightning[i].Play();
        }

        StartCoroutine(StopParticle(3f, skill3WeapenLightning));
    }


    public void Skill3AreaLightning()
    {
        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, 0, 0));
        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, -40, 0));
        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, -80, 0));
        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, -120, 0));

        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, 40, 0));
        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, 80, 0));
        GameObject.Instantiate(skill3AreaLightningPrefab, skill3Position.position, transform.rotation * Quaternion.Euler(0, 120, 0));

    }

    #endregion

    IEnumerator StopEffect(float timeSec, GameObject effect)
    {
        yield return new WaitForSeconds(timeSec);
        effect.SetActive(false);
    }
    IEnumerator StopParticle(float timeSec, ParticleSystem[] particles)
    {
        yield return new WaitForSeconds(timeSec);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }
    #endregion
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
}

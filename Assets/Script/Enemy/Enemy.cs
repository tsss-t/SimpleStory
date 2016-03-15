using UnityEngine;
using System.Collections;
public enum ActionType
{
    idel, attack, run, hit, die
}
public enum ActionState
{
    notFoundPlayer, foundPlayer, goBack, die, locked
}
public abstract class Enemy : MonoBehaviour
{
    #region Editor
    public int enemyID;//敵のID 

    public GameObject damageEffectPrefab;//流血

    #endregion
    public EnemyDropInfo dropInfo;

    public int EXP;
    public int level;

    public float speed;
    protected float speedDampTime = 0.3f;

    protected int HP;
    public int HPMax;
    protected int ATK;
    protected int DEF;
    protected float ACC;


    #region AnimationEvent
    public ActionEvent[] actionList;
    protected ActionEvent[] normalActionList;
    protected ActionEvent[] attackActionList;
    protected ActionEvent[] hitActionList;
    protected ActionEvent[] dieActionList;
    #endregion

    protected Animator anim;
    protected HashIDs hash;
    protected EnemyManager enemyManager;
    protected Vector3 playerPosition;
    public ActionState nowState;
    protected ActionType nowAction;

    #region Get/Set

    #endregion

    #region methord
    protected virtual void Start()
    {
        HP = HPMax;
        dropInfo = GameController._instance.GetEnemyInfo(this.enemyID);
        hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
        anim = this.GetComponent<Animator>();
        enemyManager = EnemyManager._instance;

        normalActionList = actionList.GetNormalActionEvents();
        attackActionList = actionList.GetAttackActionEvents();
        dieActionList = actionList.GetDieActionEvents();
        hitActionList = actionList.GetHitActionEvents();


        playerPosition = PlayerState._instance.playerTransform.position;


        Init();
    }
    protected void Init()
    {

    }
    protected virtual void Attack(int attackWeight = 1)
    {

        ACC = level - PlayerState._instance.level > 0 ? 0.7f :
                   (0.7 - (PlayerState._instance.level - level) * 0.15f <= 0 ? 0.05f :
                           ((0.7f - (PlayerState._instance.level - level) * 0.15f + 0.05f)));
        PlayerController._instance.TakeDamage(ATK, ACC);
        if (!PlayerState._instance.PlayerAliveNow)
        {
            nowState = ActionState.notFoundPlayer;
        }

    }
    protected virtual void Follow()
    {

    }
    protected virtual void Die()
    {

    }
    public virtual bool TakeDamage(int ATK,bool hitRecover=false)
    {
        if (nowState != ActionState.die)
        {
            //blood effect
            if (damageEffectPrefab != null)
            {
                GameObject.Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Have not set blooding effect");
            }
            ACC = PlayerState._instance.level - level > 0 ?
               0.7f + PlayerState._instance.DEX * 0.005f :
                   (0.7 - (level - PlayerState._instance.level) <= 0 ?
                       PlayerState._instance.DEX * 0.005f :
                           ((0.7f - (level - PlayerState._instance.level) * 0.15f + PlayerState._instance.DEX * 0.005f)));
            //debug test:
            ACC = 1;
            if (Random.value < ACC)//命中
            {
                HP -= (ATK - DEF >= 0 ? ATK - DEF : 1);
                return true;
            }
            //Debug.Log(ATK+"  damage:" +damage);
            //Debug.Log(HP / (float)HPMax);
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }



    protected virtual void BugDie()
    {
        HP = 0;
        nowState = ActionState.die;
        anim.SetTrigger(dieActionList[0].actionTrigerName);
    }
    protected virtual void Drop()
    {
        dropItem[] itemList = dropInfo.DropItemList.ToArray();
        Random.seed = System.DateTime.Now.Millisecond;
        int randomNum = Random.Range(0, 101);
        int tempNum = 0;
        int dropID = -1;
        for (int i = 0; i < itemList.Length; i++)
        {
            if (level > itemList[i].dropItemLV)
            {
                itemList[i].dropItemPre = (int)(itemList[i].dropItemPre + level * 0.25f > 10 ? 10 : itemList[i].dropItemPre + level * 0.25f);
            }
            else
            {
                itemList[i].dropItemPre = 0;
            }

            if (randomNum < tempNum + itemList[i].dropItemPre)
            {

                dropID = itemList[i].itemID;
                break;
            }
        }

        if (dropID != -1)
        {
            PlayerState._instance.GetItem(dropID);
            UIGiftManager._instance.CreatOneGift(this.gameObject);
        }

        int dropMoney = 0;

        Random.seed = System.DateTime.Now.Millisecond + 5;
        randomNum = Random.Range(0, 101);
        if (randomNum < 25)
        {
            Random.seed = System.DateTime.Now.Millisecond + 10;
            dropMoney = (int)(Mathf.Sqrt(level) * dropInfo.MoneyDropPre * 10 * Random.value);
            if (dropMoney > 0)
            {
                PlayerState._instance.GetMoney(dropMoney);

                for (int i = 0; i < dropMoney % 100; i++)
                {
                    UICoinManager._instance.CreatOneCoin(this.gameObject, 13);
                }
                dropMoney = dropMoney % 100;
                for (int i = 0; i < dropMoney % 10; i++)
                {
                    UICoinManager._instance.CreatOneCoin(this.gameObject, 9);

                }
                dropMoney = dropMoney % 10;
                for (int i = 0; i < dropMoney; i++)
                {
                    UICoinManager._instance.CreatOneCoin(this.gameObject, 7);
                }
            }
        }
    }
    protected virtual IEnumerator AfterDie()
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        for (float timer = 0; timer < 10f; timer += Time.deltaTime)
            yield return 0;

        while (transform.position.y - 1f > targetPos.y)
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
    #region
    /// <summary>
    /// ランダム方法
    /// </summary>
    /// <param name="probs">各事件に対する確率</param>
    /// <returns>第何番の事件</returns>
    protected int GetRandomEvent(int[] probs)
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
    /// <summary>
    /// 生きているかどうか
    /// </summary>
    /// <returns></returns>
    public bool isAlive()
    {
        return nowState != ActionState.die;
    }

    #endregion
    #region 外部API
    public virtual void InitEnemy(int enemyID, int level)
    {
        this.enemyID = enemyID;
        this.level = level;
        Init();
    }
    public int GetBossHP()
    {
        return this.HP;
    }
    public int GetBossMaxHP()
    {
        return this.HPMax;
    }
    #endregion

}

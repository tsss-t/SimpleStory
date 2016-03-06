using UnityEngine;
using System.Collections.Generic;



public class EnemyManager : MonoBehaviour
{
    #region para
    [System.Serializable]
    public struct EnemyStruct
    {
        public GameObject enemy;
        public int count;
        public Vector2 postion;
        public float width;
        public float height;
        public List<GameObject> enemyList;
    }

    #region Editor
    public int updateFreme = 18000;//1秒30freme
    public List<EnemyStruct> enemyAreaList;
    #endregion
    #region 外部
    public static EnemyManager _instance;

    public List<EnemyStruct> managedEnemyList;//イベント用
    #endregion
    Terrain tr;

    Vector3 buildPos;
    PlayerState playerstate;
    List<GameObject> canAttackEnemy;
    GameObject enemyContainer;
    #endregion
    #region start
    void Awake()
    {
        _instance = this;

    }

    // Use this for initialization
    void Start()
    {
        managedEnemyList = new List<EnemyStruct>();
        //tr = GameObject.FindGameObjectWithTag(Tags.terrain).GetComponent<Terrain>();
        playerstate = PlayerState._instance;
        canAttackEnemy = new List<GameObject>();
        buildPos = new Vector3(0, 0, 0);


        #region 保存したデータから、敵の位置データを貰う
        List<EnemyPositionData> enemyPositionDataList = GameController._instance.GetEnemyPosition(SceneInfomation._instance.floorNum);
        EnemyStruct enemyArea;
        if (enemyPositionDataList != null)
        {
            for (int i = 0; i < enemyPositionDataList.Count; i++)
            {
                enemyArea = new EnemyStruct();
                enemyArea.enemy = Resources.Load(GameController._instance.GetEnemyInfo(enemyPositionDataList[i].EnemyID).Name) as GameObject;
                enemyArea.count = enemyPositionDataList[i].EnemeyCount;
                enemyArea.postion = new Vector2(enemyPositionDataList[i].LeftUpPosition.x, enemyPositionDataList[i].LeftUpPosition.z);
                enemyArea.width = enemyPositionDataList[i].Width;
                enemyArea.height = enemyPositionDataList[i].Height;
                enemyArea.enemy.GetComponent<EnemyController>().level = enemyPositionDataList[i].EnemyLevel;
                enemyArea.enemy.GetComponent<EnemyController>().enemyID = enemyPositionDataList[i].EnemyID;
                enemyArea.enemyList = new List<GameObject>();
                this.enemyAreaList.Add(enemyArea);
            }
        }
        #endregion

        UpdateSence();
    }
    #endregion
    #region update
    // Update is called once per frame
    void Update()
    {
        //特定な時間を越えたら、シーンの敵をすべて再生
        if (Time.frameCount % updateFreme == 0)
        {
            UpdateSence();
        }

    }
    #endregion
    #region　シーン更新、設置した敵生成区域より、敵を生成する
    void UpdateSence()
    {
        for (int i = 0; i < enemyAreaList.Count; i++)
        {
            for (int j = enemyAreaList[i].enemyList.Count; j < enemyAreaList[i].count; j++)
            {
                enemyAreaList[i].enemyList.Add(MakeEnemey(enemyAreaList[i].enemy, enemyAreaList[i].postion, enemyAreaList[i].width, enemyAreaList[i].height));
            }
        }

    }
    #endregion

    #region 敵を生成する
    /// <summary>
    /// 敵生成区域情報より、敵を生成する
    /// </summary>
    /// <param name="prefab">敵のPrefab</param>
    /// <param name="postion">敵生成区域の中心座標</param>
    /// <param name="width">敵生成区域の大きさのwidth</param>
    /// <param name="height">敵生成区域の大きさのheight</param>
    /// <returns></returns>
    GameObject MakeEnemey(GameObject prefab, Vector3 postion, float width, float height)
    {
        if (enemyContainer == null)
        {
            enemyContainer = new GameObject("enemyContainer");
        }
        buildPos = new Vector3(
                width * Random.Range(0.0f, 1.0f) - width * 0.5f + postion.x,
                0,
                height * Random.Range(0.0f, 1.0f) - height * 0.5f + postion.y
                );
        GameObject gameObject = Instantiate(prefab, buildPos, Quaternion.Euler(0, Random.Range(-180, 180), 0)) as GameObject;

        gameObject.transform.parent = enemyContainer.transform;

        return gameObject;
    }
    #endregion
    #region 敵が死んだ後、配列から削除
    /// <summary>
    ///  敵が死んだ後、配列から削除
    /// </summary>
    /// <param name="enemy">削除必要な実体</param>
    /// <returns></returns>
    public bool destroyEnemy(GameObject enemy)
    {
        for (int i = 0; i < enemyAreaList.Count; i++)
        {
            if (isInArea(enemyAreaList[i], enemy.transform.position))
            {
                for (int j = 0; j < enemyAreaList[i].enemyList.Count; j++)
                {
                    if (enemyAreaList[i].enemyList[j] == enemy)
                    {
                        enemyAreaList[i].enemyList.Remove(enemyAreaList[i].enemyList[j]);
                        return true;
                    }
                }
            }
            else
            {
                continue;
            }
        }
        return false;
    }
    #endregion



    #region イベント用、敵生成及びプレイヤーに攻撃
    public void makeEnemyAttackPlayer(Vector3 position, int width, int height, int count, GameObject prefab)
    {
        EnemyStruct Estruct = new EnemyStruct();
        Estruct.count = count;
        Estruct.enemy = prefab;
        Estruct.height = height;
        Estruct.width = width;
        Estruct.postion = position;
        Estruct.enemyList = new List<GameObject>();
        for (int j = 0; j < Estruct.count; j++)
        {

            GameObject gameObject = MakeEnemey(prefab, position, width, height);
            gameObject.GetComponent<EnemyController>().Lock();
            gameObject.transform.LookAt(GameObject.FindGameObjectWithTag(Tags.player).transform.position);
            Estruct.enemyList.Add(gameObject);

        }
        managedEnemyList.Add(Estruct);
    }
    public void UnLockManagedEnemy()
    {
        for (int i = 0; i < managedEnemyList.Count; i++)
        {
            for (int j = 0; j < managedEnemyList[i].enemyList.Count; j++)
            {
                managedEnemyList[i].enemyList[j].GetComponent<EnemyController>().UnLock();
            }
        }
    }

    #endregion
    #region プレイヤー攻撃用
    #region 攻撃用、プレイヤーの位置情報により、攻撃可能な敵を返す
    /// <summary>
    /// 攻撃用、プレイヤーの位置情報により、攻撃可能な敵を返す
    /// </summary>
    /// <returns></returns>
    public GameObject[] getAttackEnemy()
    {
        canAttackEnemy = new List<GameObject>();
        for (int i = 0; i < enemyAreaList.Count; i++)
        {

            if (isInArea(enemyAreaList[i], playerstate.playerTransform.position))
            {
                for (int j = 0; j < enemyAreaList[i].enemyList.Count; j++)
                {
                    if (enemyAreaList[i].enemyList[j] != null && enemyAreaList[i].enemyList[j].GetComponent<EnemyController>().nowState != ActionState.die)
                    {
                        canAttackEnemy.Add(enemyAreaList[i].enemyList[j]);
                    }

                }
            }
        }
        for (int i = 0; i < managedEnemyList.Count; i++)
        {
            for (int j = 0; j < managedEnemyList[i].enemyList.Count; j++)
            {
                if (managedEnemyList[i].enemyList[j] != null && managedEnemyList[i].enemyList[j].GetComponent<EnemyController>().nowState != ActionState.die)
                {
                    canAttackEnemy.Add(managedEnemyList[i].enemyList[j]);
                }
            }
        }

        return canAttackEnemy.ToArray();
    }
    #endregion
    #region 攻撃判断用、位置は敵の活動範囲内にいるかとか
    /// <summary>
    /// 攻撃判断用、位置は敵の活動範囲内にいるかとか
    /// </summary>
    /// <param name="enemyStruct">敵の生成区域情報</param>
    /// <param name="position">判定点</param>
    /// <returns>いる時は、Trueを返す</returns>
    public bool isInArea(EnemyStruct enemyStruct, Vector3 position)
    {
        EnemyController enemyController = enemyStruct.enemy.GetComponent<EnemyController>();
        if (enemyStruct.postion.x - enemyStruct.width * 0.5 - enemyController.attackDis - enemyController.followDis - playerstate.GetAttackDis() < position.x &&
            enemyStruct.postion.x + enemyStruct.width * 0.5 + enemyController.attackDis + enemyController.followDis + playerstate.GetAttackDis() > position.x &&
            enemyStruct.postion.y + enemyStruct.height * 0.5 + enemyController.attackDis + enemyController.followDis + playerstate.GetAttackDis() > position.z &&
            enemyStruct.postion.y - enemyStruct.height * 0.5 - enemyController.attackDis - enemyController.followDis - playerstate.GetAttackDis() < position.z
        )
        {
            return true;
        }
        else
        {

            return false;
        }

    }
    #endregion
    #endregion
    #region 任務システム用、敵の名前を返す、任務メニューに表示
    public string getEnemyName(int enemyID)
    {
        for (int i = 0; i < enemyAreaList.Count; i++)
        {
            if (enemyAreaList[i].enemy.GetComponent<EnemyController>().enemyID == enemyID)
            {
                return enemyAreaList[i].enemy.GetComponent<EnemyController>().name;
            }
        }
        return "未知目標";
    }
    #endregion
}
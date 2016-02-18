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
        public Vector2 topLeft;
        public float width;
        public float height;
        public List<GameObject> enemyList;
    }
    public static EnemyManager _instance;
    public int updateFreme = 18000;//1秒30freme
    public EnemyStruct[] enemyList;
    public List<EnemyStruct> managedEnemyList;
    Terrain tr;

    Vector3 buildPos;
    PlayerState playerstate;
    List<GameObject> canAttackEnemy;
    #endregion
    #region start
    // Use this for initialization
    void Start()
    {
        _instance = this;
        managedEnemyList = new List<EnemyStruct>();
        //tr = GameObject.FindGameObjectWithTag(Tags.terrain).GetComponent<Terrain>();
        playerstate = PlayerState._instance;
        canAttackEnemy = new List<GameObject>();
        buildPos = new Vector3(0, 0, 0);

        UpdateSence();
    }
    #endregion
    #region update
    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % updateFreme == 0)
        {
            UpdateSence();
        }

    }
    #endregion
    #region　シーン更新
    void UpdateSence()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            for (int j = enemyList[i].enemyList.Count; j < enemyList[i].count; j++)
            {

                buildPos = new Vector3(
                enemyList[i].width * Random.Range(0.0f, 1.0f) + enemyList[i].topLeft.x,
                0,
                 enemyList[i].height * Random.Range(0.0f, 1.0f) + enemyList[i].topLeft.y
                );
                buildPos.y = 0;

                GameObject gameObject = Instantiate(enemyList[i].enemy, buildPos, Quaternion.Euler(0, Random.Range(-180, 180), 0)) as GameObject;

                enemyList[i].enemyList.Add(gameObject);
            }
        }

    }
    #endregion
    public void makeEnemyAttackPlayer(Vector3 position, int width, int height, int count, GameObject prefab)
    {
        EnemyStruct Estruct = new EnemyStruct();
        Estruct.count = count;
        Estruct.enemy = prefab;
        Estruct.height = height;
        Estruct.width = width;
        Estruct.topLeft = position;
        Estruct.enemyList = new List<GameObject>();
        for (int j = 0; j < Estruct.count; j++)
        {
            buildPos = new Vector3(
                Estruct.width * Random.Range(0.0f, 1.0f) + Estruct.topLeft.x,
                0,
                Estruct.height * Random.Range(0.0f, 1.0f) + Estruct.topLeft.y
                );
            buildPos.y = 0;

            GameObject gameObject = Instantiate(Estruct.enemy, buildPos, Quaternion.Euler(0, Random.Range(-180, 180), 0)) as GameObject;
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


    public GameObject[] getAttackEnemy()
    {
        canAttackEnemy = new List<GameObject>();
        for (int i = 0; i < enemyList.Length; i++)
        {

            if (isInArea(enemyList[i], playerstate.playerTransform.position))
            {
                for (int j = 0; j < enemyList[i].enemyList.Count; j++)
                {
                    if (enemyList[i].enemyList[j] != null)
                    {
                        canAttackEnemy.Add(enemyList[i].enemyList[j]);
                    }
                
                }
            }
        }
        for (int i = 0; i < managedEnemyList.Count; i++)
        {
            foreach (GameObject item in managedEnemyList[i].enemyList)
            {
                if (item != null)
                {
                    canAttackEnemy.Add(item);
                }
            }
        }

        return canAttackEnemy.ToArray();
    }
    public bool destroyEnemy(GameObject enemy)
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (isInArea(enemyList[i], enemy.transform.position))
            {
                for (int j = 0; j < enemyList[i].enemyList.Count; j++)
                {
                    if (enemyList[i].enemyList[j] == enemy)
                    {
                        enemyList[i].enemyList.Remove(enemyList[i].enemyList[j]);
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

    public bool isInArea(EnemyStruct enemyStruct, Vector3 position)
    {
        if (enemyStruct.topLeft.x - enemyStruct.enemy.GetComponent<EnemyController>().attackDis - enemyStruct.enemy.GetComponent<EnemyController>().followDis - playerstate.GetAttackDis() < position.x &&
    enemyStruct.topLeft.x + enemyStruct.width + enemyStruct.enemy.GetComponent<EnemyController>().attackDis + enemyStruct.enemy.GetComponent<EnemyController>().followDis + playerstate.GetAttackDis() > position.x &&
    enemyStruct.topLeft.y + enemyStruct.enemy.GetComponent<EnemyController>().attackDis + enemyStruct.enemy.GetComponent<EnemyController>().followDis + playerstate.GetAttackDis() > position.z &&
    enemyStruct.topLeft.y - enemyStruct.height - enemyStruct.enemy.GetComponent<EnemyController>().attackDis - enemyStruct.enemy.GetComponent<EnemyController>().followDis - playerstate.GetAttackDis() < position.z
    )
        {
            return true;
        }
        else
        {

            return false;
        }

    }
    public string getEnemyName(int enemyID)
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].enemy.GetComponent<EnemyController>().enemyID == enemyID)
            {
                return enemyList[i].enemy.GetComponent<EnemyController>().name;
            }
        }
        return "未知目標";
    }



}

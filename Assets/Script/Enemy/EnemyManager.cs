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
    public int updateFreme = 18000;//1秒30freme
    public EnemyStruct[] enemyList;

    Terrain tr;

    Vector3 buildPos;
    PlayerState playerstate;
    List<GameObject> canAttackEnemy;
    #endregion
    #region start
    // Use this for initialization
    void Start()
    {
        //tr = GameObject.FindGameObjectWithTag(Tags.terrain).GetComponent<Terrain>();
        playerstate = PlayerState.GamePlayerState;
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
        for (int enemyTypeI = 0; enemyTypeI < enemyList.Length; enemyTypeI++)
        {
            for (int enemyI = enemyList[enemyTypeI].enemyList.Count; enemyI < enemyList[enemyTypeI].count; enemyI++)
            {

                buildPos = new Vector3(
                enemyList[enemyTypeI].width * Random.Range(0.0f, 1.0f) + enemyList[enemyTypeI].topLeft.x,
                0,
                 enemyList[enemyTypeI].height * Random.Range(0.0f, 1.0f) + enemyList[enemyTypeI].topLeft.y
                );
                buildPos.y =0;

                GameObject gameObject = Instantiate(enemyList[enemyTypeI].enemy, buildPos, Quaternion.Euler(0, Random.Range(-180, 180), 0)) as GameObject;
                enemyList[enemyTypeI].enemyList.Add(gameObject);
            }
        }
    }
    #endregion


    public GameObject[] getAttackEnemy()
    {
        canAttackEnemy = new List<GameObject>();
        for (int i = 0; i < enemyList.Length; i++)
        {

            if (isInArea(enemyList[i], playerstate.playerTransform.position))
            {
                for (int j = 0; j < enemyList[i].enemyList.Count; j++)
                {
                    canAttackEnemy.Add(enemyList[i].enemyList[j]);
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

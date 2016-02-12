using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    GameObject player;
    AutoMove playerAutoMove;
    public GameObject tufei;
    AutoMove tufeiAutoMove;

    PlayerState playerState;

    EnemyManager enemyManager;

    TalkingManager talkingManager;

    public Transform[] PositionTarget;

    EventDelegate nextEvent;
    bool thisEventIsOver;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager).GetComponent<EnemyManager>();

        playerAutoMove = player.GetComponent<AutoMove>();
        tufeiAutoMove = tufei.GetComponent<AutoMove>();

        playerState = PlayerState.GamePlayerState;
        nextEvent = new EventDelegate();
        nextEvent.target = this;
        thisEventIsOver = false;
    }
    void Update()
    {
        if (thisEventIsOver)
        {
            if (nextEvent.methodName == "")
            {
                thisEventIsOver = false;
            }
            else
            {
                thisEventIsOver = false;
                nextEvent.Execute();
            }
        }
    }
    //移动到指定位置
    public void EventOneOne()
    {

        playerAutoMove.StartDestination(PositionTarget[0].position, PlayerState.PlayerAction.Locked);
        StartCoroutine(WaitToMoveEnd());
        nextEvent.methodName = "EventOneTwo";
    }
    //对话1
     void EventOneTwo()
    {
        talkingManager = GameObject.FindGameObjectWithTag(Tags.talkingManager).GetComponent<TalkingManager>();
        talkingManager.PlayEvent(1);
        nextEvent.methodName = "EventOneThree";
    }
    //交钱场景 去
     void EventOneThree()
    {
        playerAutoMove.StartDestination(PositionTarget[1].position, PlayerState.PlayerAction.Locked);
        StartCoroutine(WaitToMoveEnd(2f));
        nextEvent.methodName = "EventOneFour";
    }
    //交钱场景 回
     void EventOneFour()
    {
        playerAutoMove.StartDestination(PositionTarget[0].position, PlayerState.PlayerAction.Locked);
        StartCoroutine(WaitToMoveEnd());
        nextEvent.methodName = "EventTwoOne";
    }
     void EventTwoOne()
    {
        enemyManager.makeEnemyAttackPlayer(new Vector3(-80, -20, 0), 20, 30, 10, enemyPrefab);
        player.transform.LookAt(PositionTarget[1]);
        talkingManager = GameObject.FindGameObjectWithTag(Tags.talkingManager).GetComponent<TalkingManager>();
        talkingManager.PlayEvent(2);
        nextEvent.methodName = "EventTwoTwo";
    }
     void EventTwoTwo()
    {
        enemyManager.UnLockManagedEnemy();
        tufeiAutoMove.StartDestination(PositionTarget[2].position);
        playerState.ChangeAction(PlayerState.PlayerAction.Free);
    }

    public void EventThreeOne()
    {
        playerState.ChangeAction(PlayerState.PlayerAction.Locked);
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraMovement>().setTarget(tufei.transform);
        tufeiAutoMove.StartDestination(PositionTarget[3].position);
        StartCoroutine(Wait(3f));
        nextEvent.methodName = "EventThreeTwo";

    }
     void EventThreeTwo()
    {
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraMovement>().setTarget(player.transform);
        playerState.ChangeAction(PlayerState.PlayerAction.Free);

    }




    IEnumerator WaitToChangeAction(PlayerState.PlayerAction action)
    {
        while (!playerAutoMove.IsMoveOver())
        {
            yield return new WaitForSeconds(0.5f);
        }
        playerState.ChangeAction(action);
        thisEventIsOver = true;
    }
    IEnumerator WaitToMoveEnd()
    {
        while (!playerAutoMove.IsMoveOver())
        {
            yield return new WaitForSeconds(0.5f);
        }
        thisEventIsOver = true;
    }
    IEnumerator WaitToMoveEnd(float time)
    {
        while (!playerAutoMove.IsMoveOver())
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(time);
        thisEventIsOver = true;
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        thisEventIsOver = true;
    }
    public void SetEventOver()
    {
        thisEventIsOver = true;
    }


}

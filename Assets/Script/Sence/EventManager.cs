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
            if (nextEvent == null || nextEvent.methodName == "")
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
        enemyManager.makeEnemyAttackPlayer(new Vector3(-80, -20, 0), 20, 20, 10, enemyPrefab);
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
    public void EventTwoOneOne()
    {
        EventDelegate distorytufei = new EventDelegate(this, "DesTufei");

        EventDelegate cameraMove = new EventDelegate(this, "CameraMove");
        cameraMove.parameters[0] = new EventDelegate.Parameter(tufei.transform);
        cameraMove.parameters[1] = new EventDelegate.Parameter(PositionTarget[0].position);
        cameraMove.parameters[2] = new EventDelegate.Parameter(3f);
        cameraMove.parameters[3] = new EventDelegate.Parameter(distorytufei);

        Talk(3, cameraMove);
    }


    void MakeEnemyToAttakPlayer(Vector3 enemyPosition, int width, int height, int count, GameObject enemyPrefab, EventDelegate nextmethod)
    {
        enemyManager.makeEnemyAttackPlayer(enemyPosition, width, height, count, enemyPrefab);
        nextEvent = nextmethod;
    }
    void UnlockEnemy(EventDelegate nextmethod)
    {
        enemyManager.UnLockManagedEnemy();
        nextEvent = nextmethod;

    }
    void Talk(int talkEventID, EventDelegate nextmethod)
    {
        playerState.ChangeAction(PlayerState.PlayerAction.Locked);
        talkingManager = GameObject.FindGameObjectWithTag(Tags.talkingManager).GetComponent<TalkingManager>();
        talkingManager.PlayEvent(talkEventID);
        nextEvent = nextmethod;
    }

    void PlayerTalk(int talkEventID, EventDelegate nextmethod, Transform targetPosition)
    {
        player.transform.LookAt(targetPosition);
        talkingManager = GameObject.FindGameObjectWithTag(Tags.talkingManager).GetComponent<TalkingManager>();
        talkingManager.PlayEvent(talkEventID);
        nextEvent = nextmethod;
    }
    void MoveTo(Transform targetPosition, Vector3 targetMoveTo, float waitTime, EventDelegate nextmethod)
    {
        if (targetPosition.gameObject.tag.Equals(Tags.player))
        {
            playerAutoMove.StartDestination(targetMoveTo, PlayerState.PlayerAction.Locked);
            StartCoroutine(WaitToMoveEnd(waitTime));
        }
        else
        {
            targetPosition.GetComponent<AutoMove>().StartDestination(targetMoveTo);
        }
        nextEvent = nextmethod;
    }
    void CameraMove(Transform targetPosition, Vector3 targetMoveTo, float camaraWaitTime, EventDelegate nextmethod)
    {
        playerState.ChangeAction(PlayerState.PlayerAction.Locked);
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraMovement>().setTarget(targetPosition);
        targetPosition.GetComponent<AutoMove>().StartDestination(targetMoveTo);
        StartCoroutine(Wait(camaraWaitTime));
        EventDelegate camaraBack = new EventDelegate(this, "CamaraBack");
        camaraBack.parameters[0] = new EventDelegate.Parameter(nextmethod);
        nextEvent = camaraBack;
    }
    void CamaraBack(EventDelegate nextmethod)
    {
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraMovement>().setTarget(player.transform);
        playerState.ChangeAction(PlayerState.PlayerAction.Free);
        thisEventIsOver = true;
        nextEvent = nextmethod;
    }
    void DesTufei(EventDelegate nextmethod)
    {
        Debug.Log("DES");
        StartCoroutine(DestroyTufei());
        nextEvent = nextmethod;

    }
    IEnumerator DestroyTufei()
    {
        try
        {
            iTween.FadeTo(tufei.transform.Find("Merchant_body").gameObject, 0, 2f);
            iTween.FadeTo(tufei.transform.Find("Object017").gameObject, 0, 2f);
            iTween.FadeTo(tufei.transform.Find("Object018").gameObject, 0, 2f);
            iTween.FadeTo(tufei.transform.Find("Object019").gameObject, 0, 2f);
        }
        catch { }
        yield return new WaitForSeconds(3f);
        thisEventIsOver = true;
        Destroy(tufei);
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

using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public static EventManager _instance;
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
        _instance = this;
        player = GameObject.FindGameObjectWithTag(Tags.player);
        enemyManager = EnemyManager._instance;

        playerAutoMove = player.GetComponent<AutoMove>();
        if (tufei != null)
        {
            tufeiAutoMove = tufei.GetComponent<AutoMove>();
        }

        playerState = PlayerState._instance;
        nextEvent = new EventDelegate();
        nextEvent.target = this;
        thisEventIsOver = false;
        setGameObjectHide();
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
    #region Event---- name format:  Event_scene_eventID_eventStepID
    #region Town Evet
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
        talkingManager = TalkingManager._instance;
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
        enemyManager.makeEnemyAttackPlayer(new Vector3(-80, -20, 0), 20, 20, 6, enemyPrefab);
        player.transform.LookAt(PositionTarget[1]);
        talkingManager = TalkingManager._instance;
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

        GameController._instance.doneEvent(0);
    }
    #endregion
    #region firstFloor Event
    /// <summary>
    /// 賊が魔法陣に移動
    /// </summary>
    public void EventTwoOneOne()
    {
        EventDelegate distorytufei = new EventDelegate(this, "DesTufei");

        EventDelegate cameraMove = new EventDelegate(this, "CameraFollowObject");
        cameraMove.parameters[0] = new EventDelegate.Parameter(tufei.transform);
        cameraMove.parameters[1] = new EventDelegate.Parameter(PositionTarget[0].position);
        cameraMove.parameters[2] = new EventDelegate.Parameter(3f);
        cameraMove.parameters[3] = new EventDelegate.Parameter(distorytufei);

        Talk(3, cameraMove);
    }
    /// <summary>
    /// 地震イベント
    /// </summary>
    public void EventTowTowOne()
    {
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraShake>().isShake = true;
        EventDelegate makePlayerFree = new EventDelegate(this, "MakePlayerFree");
        Talk(4, makePlayerFree);
    }
    /// <summary>
    /// 魔法陣調査
    /// </summary>
    public void EventTowThreeOne()
    {
        EventDelegate makePlayerFree = new EventDelegate(this, "MakePlayerFree");
        Talk(5, makePlayerFree);
    }
    /// <summary>
    /// 入口から脱出禁止のイベント
    /// </summary>
    public void EventTowFourOne()
    {
        EventDelegate makePlayerFree = new EventDelegate(this, "MakePlayerFree");


        EventDelegate move = new EventDelegate(this, "CameraFollowObject");
        move.parameters[0] = new EventDelegate.Parameter(player.transform);
        move.parameters[1] = new EventDelegate.Parameter(PositionTarget[1].position);
        move.parameters[2] = new EventDelegate.Parameter(0f);
        move.parameters[3] = new EventDelegate.Parameter(makePlayerFree);
        Talk(6, move);
    }
    #endregion
    #region shopFloor Event 
    public void EventShopOneOne()
    {
        EventDelegate unlockPlayer = new EventDelegate(this, "MakePlayerFree");

        EventDelegate talkEvent9 = new EventDelegate(this, "Talk");
        talkEvent9.parameters[0] = new EventDelegate.Parameter(9);
        talkEvent9.parameters[1] = new EventDelegate.Parameter(unlockPlayer);

        EventDelegate helpShopping = new EventDelegate(this, "EventHelpShopping");
        helpShopping.parameters[0] = new EventDelegate.Parameter(talkEvent9);

        EventDelegate talkEvent8 = new EventDelegate(this, "Talk");
        talkEvent8.parameters[0] = new EventDelegate.Parameter(8);
        talkEvent8.parameters[1] = new EventDelegate.Parameter(helpShopping);

        EventDelegate moveToShangRen = new EventDelegate(this, "MoveTo");
        moveToShangRen.parameters[0] = new EventDelegate.Parameter(player.transform);
        moveToShangRen.parameters[1] = new EventDelegate.Parameter(PositionTarget[1].position);
        moveToShangRen.parameters[2] = new EventDelegate.Parameter(0f);
        moveToShangRen.parameters[3] = new EventDelegate.Parameter(talkEvent8);

        EventDelegate talkEvent7 = new EventDelegate(this, "Talk");
        talkEvent7.parameters[0] = new EventDelegate.Parameter(7);
        talkEvent7.parameters[1] = new EventDelegate.Parameter(moveToShangRen);

        EventDelegate cameraLookAt = new EventDelegate(this, "CameraLookAt");
        cameraLookAt.parameters[0] = new EventDelegate.Parameter(PositionTarget[0].transform);
        cameraLookAt.parameters[1] = new EventDelegate.Parameter(2f);
        cameraLookAt.parameters[2] = new EventDelegate.Parameter(talkEvent7);

        cameraLookAt.Execute();
        GameController._instance.doneEvent(2);
    }
    public void EventHelpShopping(EventDelegate nextMethod)
    {
        this.nextEvent = nextMethod;
        StartCoroutine(helpShopping());
    }
    #endregion
    #endregion

    #region Event API (Type )
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
        talkingManager = TalkingManager._instance;
        talkingManager.PlayEvent(talkEventID);
        nextEvent = nextmethod;
    }

    void PlayerTalk(int talkEventID, EventDelegate nextmethod, Transform targetPosition)
    {
        player.transform.LookAt(targetPosition);
        talkingManager = TalkingManager._instance;
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

    void CameraLookAt(Transform targetPosition,  float camaraWaitTime, EventDelegate nextmethod)
    {
        Debug.Log("??");
        playerState.ChangeAction(PlayerState.PlayerAction.Locked);
        GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraMovement>().setTarget(targetPosition);
        StartCoroutine(Wait(camaraWaitTime));
        EventDelegate camaraBack = new EventDelegate(this, "CamaraBack");
        camaraBack.parameters[0] = new EventDelegate.Parameter(nextmethod);
        nextEvent = camaraBack;
    }
    void CameraFollowObject(Transform targetPosition, Vector3 targetMoveTo, float camaraWaitTime, EventDelegate nextmethod)
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
        StartCoroutine(DestroyTufei());
        nextEvent = nextmethod;

    }
    void MakePlayerFree()
    {
        playerState.ChangeAction(PlayerState.PlayerAction.Free);
        thisEventIsOver = true;
        nextEvent = null;
    }

    #endregion

    #region IEnumerator

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

    #endregion
    #region SpecilEvent
    public void setGameObjectHide()
    {
        if(!GameController._instance.getEventIsDone(2))
        {
            foreach (GameObject item in NPCManager._instance.GetNPCDctionary().Values)
            {
                item.SetActive(false);
            }
        }
    }
    IEnumerator helpShopping()
    {
        UISceneFader._instance.changeFade();
        yield return new WaitForSeconds(2f);

        tufei.SetActive(false);
        foreach (GameObject item in NPCManager._instance.GetNPCDctionary().Values)
        {
            item.SetActive(true);
        }

        UISceneFader._instance.changeFade();
        yield return new WaitForSeconds(2f);

        thisEventIsOver = true;
    }

    #endregion
}

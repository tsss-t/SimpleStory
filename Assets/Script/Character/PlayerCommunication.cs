using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCommunication : MonoBehaviour
{
    #region Para
    public GameObject starTip;
    PlayerState playerState;
    List<GameObject> colliderList;
    GameObject targetObject;
    float targetObjectDistance;
    bool targetIsFront;
    float timer;
    #endregion
    #region Start/Update
    // Use this for initialization
    void Start()
    {
        timer = 0f;
        playerState = PlayerState._instance;
        colliderList = new List<GameObject>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (playerState.PlayerAliveNow)
        {
            if (Input.GetButtonDown("Comunication") && targetObject != null && timer > 0.5f&&playerState.GetActionInfoNow()==PlayerState.PlayerAction.Free )
            {
                switch(targetObject.tag)
                {
                    case Tags.worktop:
                        {
                            UIComposeManager._instance.OnCommunicationStart();
                            break;
                        }
                    case Tags.NPC:
                        {
                            playerState.DoQuest(QuestType.findNPC, targetObject.GetComponent<NPCInfomation>().Communication());
                            break;
                        }

                }
                timer = 0f;
            }
        }


        if (colliderList.Count != 0)
        {
            FindObject();
        }
    }
    #endregion
    #region Event (OnTriggerEnter,OnTriggerExit)
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == Tags.NPC || collider.gameObject.tag == Tags.worktop)
        {
            colliderList.Add(collider.gameObject);
            FindObject();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == Tags.NPC || collider.gameObject.tag == Tags.worktop)
        {
            colliderList.Remove(collider.gameObject);
            FindObject();
        }
    }
    #endregion
    #region FindObject 範囲内の　やり取りできる　オブジェクト　の中に、一番近いオブジェクトを探す
    /// <summary>
    /// 範囲内の　やり取りできる　オブジェクト　の中に、一番近いオブジェクトを探す
    /// </summary>
    void FindObject()
    {
        targetObjectDistance = 100f;
        targetIsFront = false;
        targetObject = null;
        for (int i = 0; i < colliderList.Count; i++)
        {
            Vector3 targetLocalPosition = this.transform.InverseTransformPoint(colliderList[i].transform.position);
            if (targetIsFront)
            {
                if (targetLocalPosition.z > 0.0f)
                {
                    if (Vector3.Distance(transform.position, targetLocalPosition) < targetObjectDistance)
                    {
                        targetObject = colliderList[i];
                        targetObjectDistance = Vector3.Distance(transform.position, targetLocalPosition);
                    }
                }
            }
            else
            {
                if (targetLocalPosition.z > 0.0f)
                {
                    targetObject = colliderList[i];
                    targetObjectDistance = Vector3.Distance(transform.position, targetLocalPosition);
                }
                else
                {
                    if (Vector3.Distance(transform.position, targetLocalPosition) < targetObjectDistance)
                    {
                        targetObject = colliderList[i];
                        targetObjectDistance = Vector3.Distance(transform.position, targetLocalPosition);
                    }
                }

            }
        }
        if (targetObject != null)
        {
            starTip.SetActive(true);

            //starTip.transform.position = targetObject.transform.Find("MarkTip").transform.position;
            starTip.GetComponent<UIFollowTarget>().target = targetObject.transform.Find("MarkTip").transform;
        }
        else
        {
            starTip.SetActive(false);
        }

    }
    #endregion
}

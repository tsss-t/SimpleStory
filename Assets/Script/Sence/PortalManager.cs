using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PortalManager : MonoBehaviour
{
    Dictionary<string, GameObject> colliderList;

    public static PortalManager _instans;

    public EventDelegate otherCharaInEvent;
    public GameObject portalEffect;

    public delegate void  PlayerStateChange( bool isIn);
    public event PlayerStateChange playerStateChange;

    // Use this for initialization
    void Start()
    {
        _instans = this;
        colliderList = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    GameObject go;
    void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger)
        {
            if (collider.gameObject.tag.Equals(Tags.player))
            {
                playerIn();
            }
            else if (!collider.gameObject.tag.Equals(Tags.player))
            {
                if (otherCharaInEvent != null)
                {
                    //otherCharaInEvent.Execute();
                }
            }
            colliderList.TryGetValue(collider.gameObject.name, out go);
            if (go == null)
            {

                go = Instantiate(portalEffect, collider.transform.position, Quaternion.Euler(-90f, 0f, 0f)) as GameObject;
                go.GetComponent<FollowTarget>().targetTransform = collider.transform;

                colliderList.Add(collider.gameObject.name, go);
            }

        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (!collider.isTrigger)
        {
            if (collider.gameObject.tag.Equals(Tags.player))
            {
                playerOut();
            }
            StartCoroutine(DestoryParticleSystem(colliderList[collider.gameObject.name]));

            colliderList.Remove(collider.gameObject.name);
        }

    }
    IEnumerator DestoryParticleSystem(GameObject go)
    {
        go.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2f);
        Destroy(go);
    }

    void playerIn()
    {
        playerStateChange(true);
    }
    void playerOut()
    {
        playerStateChange(false);
    }

}

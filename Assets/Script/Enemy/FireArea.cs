using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireArea : MonoBehaviour
{
    public float DestroyTime=5;

    private List<GameObject> inColliderList;
    private ParticleSystem[] particleSystems;
    // Use this for initialization
    void Start()
    {
        inColliderList = new List<GameObject>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(MakeDamage());

        StartCoroutine(AutoDestroy());
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == Tags.player && inColliderList.IndexOf(collider.gameObject) < 0)
        {
            inColliderList.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.tag == Tags.player && inColliderList.IndexOf(collider.gameObject) >= 0)
        {
            inColliderList.Remove(collider.gameObject);
        }
    }

    IEnumerator MakeDamage()
    {
        while (true)
        {
            if(inColliderList.Count>0)
            {
                PlayerController._instance.TakeDamage(200, 1);
            }
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator AutoDestroy()
    {


        yield return new WaitForSeconds(DestroyTime);
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }

        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);

    }
}

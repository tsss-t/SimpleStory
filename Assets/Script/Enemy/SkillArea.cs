using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class SkillArea : MonoBehaviour
{
    protected float attackDelay = 0.8f;
    protected float destroyTime = 5;
    protected int damage = 200;
    protected List<GameObject> inColliderList;
    protected ParticleSystem[] particleSystems;
    // Use this for initialization
    protected virtual void Start()
    {
        inColliderList = new List<GameObject>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();


        StartCoroutine(MakeDamage());

        StartCoroutine(AutoDestroy());
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == Tags.player && !collider.isTrigger && inColliderList.IndexOf(collider.gameObject) < 0)
        {
            inColliderList.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.tag == Tags.player && !collider.isTrigger && inColliderList.IndexOf(collider.gameObject) >= 0)
        {
            inColliderList.Remove(collider.gameObject);
        }
    }
    protected virtual IEnumerator MakeDamage()
    {
        while (true)
        {
            if (inColliderList.Count > 0)
            {
                PlayerController._instance.TakeDamage(damage, 1);
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }

    protected virtual IEnumerator AutoDestroy()
    {

        yield return new WaitForSeconds(destroyTime);
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
        //FADE OUTのため
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);

    }
}

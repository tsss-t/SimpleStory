using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LightningArea : MonoBehaviour
{
    public float speed = 5;
    public int angle = 10;

    public float DestroyTime = 10;
    public Vector3 forwardDirection;


    private Rigidbody rigidBody;
    private List<GameObject> inColliderList;
    private ParticleSystem[] particleSystems;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        inColliderList = new List<GameObject>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(MakeDamage());

        StartCoroutine(AutoDestroy());
        StartCoroutine(RandomDirection());

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        rigidBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 1 * Time.deltaTime);
        // Quaternion targetRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        // targetRot.
        //Quaternion.Lerp();

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
    Quaternion targetRot;
    IEnumerator RandomDirection()
    {
        while (true)
        {
            targetRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w) * Quaternion.Euler(0, angle * 0.5f - angle, 0);


            yield return new WaitForSeconds(2f);
        }

    }

    IEnumerator MakeDamage()
    {
        while (true)
        {
            if (inColliderList.Count > 0)
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

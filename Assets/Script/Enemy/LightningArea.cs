using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LightningArea : SkillArea
{
    public float DestroyTime = 5;
    public int Damage = 200;
    public float Speed = 5;
    public int Angle = 10;


    private Rigidbody rigidBody;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        base.destroyTime = DestroyTime;
        base.damage = Damage;
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(RandomDirection());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Move();
    }
    void Move()
    {
        rigidBody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 1 * Time.deltaTime);
    }
    Quaternion targetRot;
    IEnumerator RandomDirection()
    {
        while (true)
        {
            targetRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w) * Quaternion.Euler(0, Angle * 0.5f - Angle, 0);
            yield return new WaitForSeconds(2f);
        }
    }

    protected override IEnumerator MakeDamage()
    {
        yield return base.MakeDamage();
    }
    protected override IEnumerator AutoDestroy()
    {
        yield return base.AutoDestroy();
    }
}

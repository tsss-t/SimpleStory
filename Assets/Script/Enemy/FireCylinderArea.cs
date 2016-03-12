using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FireCylinderArea : SkillArea
{
    public float DestroyTime = 5;
    public int Damage = 200;
    public float Speed = 5;
    public float Angle = -360;


    private Rigidbody rigidBody;
    // Use this for initialization
    protected override void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        base.damage = Damage;
        base.destroyTime = DestroyTime;
        base.attackDelay = 0.2f;
        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Move();
    }
    Quaternion targetRot;
    void Move()
    {
        Angle = Mathf.Lerp(Angle, 0,0.5f* Time.deltaTime);

        rigidBody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);

        targetRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w) * Quaternion.Euler(0, Angle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot,0.8f * Time.deltaTime);
    }

}

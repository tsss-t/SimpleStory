using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireArea : SkillArea
{
    public float DestroyTime = 5;
    public int Damage = 200;

    // Use this for initialization
    protected override void Start()
    {
        base.destroyTime = DestroyTime;
        base.damage = Damage;
        base.Start();
    }


}

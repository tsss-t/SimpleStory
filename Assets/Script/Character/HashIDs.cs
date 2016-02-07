using UnityEngine;

public class HashIDs : MonoBehaviour
{
    public int EmptyState;
    public int NormalAttack1State;
    public int NormalAttack2State;
    public int NormalAttack3State;

    public int Skill1State;
    public int Skill2State;
    public int Skill3State;

    public int hitTrigger;
    public int dieTrigger;
    public int locomotionState;

    //攻撃
    public int AttackTrigger;
    public int Skill1Trigger;
    public int Skill2Trigger;
    public int Skill3Trigger;
    //移動
    public int speedFloat;

    public int playerInSightBool;
    public int shotFloat;
    public int aimWeightFloat;
    public int angularSpeedFloat;
    public int openBool;

    //ここからはenemy
    public int enemySpeedFloat;

    void Awake()
    {
        //状態
        EmptyState = Animator.StringToHash("Attack Layer.Empty");
        NormalAttack1State = Animator.StringToHash("Attack Layer.NormalAttack1");
        NormalAttack2State = Animator.StringToHash("Attack Layer.NormalAttack2");
        NormalAttack3State = Animator.StringToHash("Attack Layer.NormalAttack3");
        Skill1State = Animator.StringToHash("Attack Layer.Skill1");
        Skill2State = Animator.StringToHash("Attack Layer.Skill2");
        Skill3State = Animator.StringToHash("Attack Layer.Skill3");

        dieTrigger = Animator.StringToHash("Die");
        hitTrigger = Animator.StringToHash("Hit");

        AttackTrigger = Animator.StringToHash("NormalAttack");
        Skill1Trigger = Animator.StringToHash("Skill1");
        Skill2Trigger = Animator.StringToHash("Skill2");
        Skill3Trigger = Animator.StringToHash("Skill3");


        speedFloat = Animator.StringToHash("Speed");

        playerInSightBool = Animator.StringToHash("PlayerInSight");
        shotFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        openBool = Animator.StringToHash("Open");

        //ここからはenemy
        enemySpeedFloat = Animator.StringToHash("speed");
    }
}
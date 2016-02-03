using UnityEngine;

public class HashIDs : MonoBehaviour
{
    public int EmptyState;

    public int hitTrigger;
    public int dieTrigger;
    public int locomotionState;
    public int AttackBool;
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
        EmptyState = Animator.StringToHash("Attack Layer.Empty");

        dieTrigger = Animator.StringToHash("Die");
        hitTrigger = Animator.StringToHash("Hit");
        AttackBool = Animator.StringToHash("NormalAttack");
        speedFloat = Animator.StringToHash("Speed");

        playerInSightBool = Animator.StringToHash("PlayerInSight");
        shotFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        openBool = Animator.StringToHash("Open");

        //ここからはenemy
        enemySpeedFloat = Animator.StringToHash("speed");
    }
}
using UnityEngine;
using System.Collections;

public class NormalAttack : StateMachineBehaviour {
    private HashIDs hash;
    PlayerState playerState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //アニメーション開始　／　動作開始
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        playerState = PlayerState.GamePlayerState;
        hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
        animator.SetBool(hash.AttackBool, false);
        animator.SetFloat(hash.speedFloat, 0f);


        if (stateInfo.fullPathHash!=hash.EmptyState)
        {
            Attack( SkillType.basic);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    /// <summary>
    /// 攻撃によって、体力減少
    /// </summary>
    /// <param name="type">攻撃種類</param>
    /// <returns></returns>
    public bool Attack(SkillType type)
    {

        switch (type)
        {
            case SkillType.basic:
                {
                    if (playerState.energy >= 20)
                    {
                        playerState.energy -= 20;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
        }
        playerState.PlayerStateChanged(PlayerStateChangeType.energy);
        return true;
    }
}

using UnityEngine;
using System.Collections;

public class AttackAnimation : StateMachineBehaviour {
    private HashIDs hash;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //アニメーション開始　／　動作開始
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
        animator.SetFloat(hash.speedFloat, 0f);

        if (stateInfo.fullPathHash!=hash.EmptyState)
        {

            if(stateInfo.fullPathHash==hash.NormalAttack1State||
                stateInfo.fullPathHash == hash.NormalAttack2State|| 
                stateInfo.fullPathHash == hash.NormalAttack3State)
            {
                PlayerAttack._instance.UseEnergy(SkillType.basic,PosType.basic);
            }
            else
            {
                if (stateInfo.fullPathHash == hash.Skill1State)
                {
                    PlayerAttack._instance.UseEnergy(SkillType.skill, PosType.one);
                }
                else if (stateInfo.fullPathHash == hash.Skill2State)
                {
                    PlayerAttack._instance.UseEnergy(SkillType.skill, PosType.two);
                }
                else if (stateInfo.fullPathHash == hash.Skill3State)
                {
                    PlayerAttack._instance.UseEnergy(SkillType.skill, PosType.three);
                }

            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    hash = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<HashIDs>();
    //    animator.SetFloat(hash.speedFloat, 0f);

    //    if (stateInfo.fullPathHash == hash.Skill1State)
    //    {

    //    }
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

using UnityEngine;
using System.Collections;

public class AttackEffect : MonoBehaviour {
    public Renderer[] AttackRender;
    NcCurveAnimation[] AttackAnimation;
	// Use this for initialization
	void Start () {
        AttackRender = this.GetComponentsInChildren<Renderer>();
        AttackAnimation = this.GetComponentsInChildren<NcCurveAnimation>();
        foreach (Renderer ren in AttackRender)
        {
            ren.enabled = false;
        }
    }
	

    public void ShowAttack()
    {
        foreach(Renderer ren in AttackRender)
        {
            ren.enabled = true;
        }
        foreach (NcCurveAnimation animation in AttackAnimation)
        {
            animation.ResetAnimation();
        }
    }
}

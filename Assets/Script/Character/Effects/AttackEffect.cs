using UnityEngine;
using System.Collections;

public class AttackEffect : MonoBehaviour {
    Renderer[] AttackRender;
    NcCurveAnimation[] AttackAnimation;
    GameObject effectOffset;
    // Use this for initialization
    void Start () {
        AttackRender = this.GetComponentsInChildren<Renderer>();
        AttackAnimation = this.GetComponentsInChildren<NcCurveAnimation>();
        if (transform.Find("EffectOffset") != null)
        {
            effectOffset = transform.Find("EffectOffset").gameObject;
        }
        //foreach (Renderer ren in AttackRender)
        //{
        //    ren.enabled = false;
        //}
    }
	

    public void ShowAttack()
    {
        if (effectOffset != null)
        {
            effectOffset.SetActive(false);
            effectOffset.SetActive(true);
        }
        else
        {
            foreach (Renderer ren in AttackRender)
            {
                ren.enabled = true;
            }
            foreach (NcCurveAnimation animation in AttackAnimation)
            {
                animation.ResetAnimation();
            }
        }

    }
}

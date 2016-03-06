using UnityEngine;
using System.Collections;

public class UIHpBarManager : MonoBehaviour
{
    public GameObject HpBar;
    public static UIHpBarManager _instans;
    // Use this for initialization
    void Start()
    {
        _instans = this;
    }

    public GameObject CreateHpBar(GameObject enemy)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, HpBar); 
        go.GetComponent<UIFollowTarget>().target = enemy.transform;
        return go;
    }
    public void DestoryHpBar(GameObject Hpbar)
    {
        NGUITools.Destroy(Hpbar);
    }

}

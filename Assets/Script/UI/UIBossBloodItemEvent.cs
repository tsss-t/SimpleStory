using UnityEngine;
using System.Collections;

public class UIBossBloodItemEvent : MonoBehaviour
{


    UISlider UIBossHPbarSlider;
    UILabel UIBossNameLabel;
    UILabel UIHPLabel;
    string bossName="";
    int bossHP=0;
    // Use this for initialization
    void Start()
    {
        UIBossHPbarSlider = this.GetComponentInChildren<UISlider>();

        UIBossNameLabel = transform.Find("BossNameBG/BossNameLabel").GetComponent<UILabel>();
        UIHPLabel = transform.Find("BloodBarBack/BloodNumLabel").GetComponent<UILabel>();

        SetHp(bossHP, bossHP);
        SetBossName(bossName);

    }
    public void Init(string bossName, int bossHP)
    {
        this.bossName = bossName;
        this.bossHP = bossHP;
    }

    public void SetBossName(string name)
    {
        this.UIBossNameLabel.text = name;
    }

    public void SetHp(int Hp, int MaxHp)
    {
        UIBossHPbarSlider.value = Hp / (float)MaxHp;
        UIHPLabel.text = string.Format("{0}/{1}", Hp < 0 ? 0 : Hp, MaxHp);
    }

}

using UnityEngine;
using System.Collections;
public enum ItemType
{
    head, body, foot, necklace, ring, bracelet, wing,weapon, drug, key, quest, error
}
/// <summary>
/// アイテムの詳細情報、データベースのitemテーブル
/// </summary>
public class ItemInfo
{
    #region para
    /// <summary>
    /// itemテーブル中のID
    /// </summary>
    public int id;
    public ItemType type;
    public string adress;
    public string name;
    public int STR;//力
    public int DEX;//素早さ
    public int INT;//知恵
    public int CON;//体力
    public int LUK;//運
    public int HP;
    public int energy;
    public int needSTR;
    public int needDEX;
    public int needINT;
    public int needCON;
    public int needLevel;
    public int money;
    #endregion

    #region 初期化(装備・薬品)
    /// <summary>
    /// 装備の初期化方法
    /// </summary>
    /// <param name="id">アイテムのID</param>
    /// <param name="type">装備のタイプ、装備以外のタイプは入力禁止！</param>
    /// <param name="picAdress">装備の画像アドレス</param>
    /// <param name="name">装備の名前</param>
    /// <param name="STR">装備の攻撃力</param>
    /// <param name="DEX">装備の素早さ</param>
    /// <param name="INT">装備の知恵力</param>
    /// <param name="CON">装備の体力</param>
    /// <param name="LUK">装備の運</param>
    /// <param name="needSTR">装備必要な攻撃力</param>
    /// <param name="needDEX">装備必要な素早さ</param>
    /// <param name="needINT">装備必要な知恵力</param>
    /// <param name="needCON">装備必要な運</param>
    /// <param name="needLevel">装備必要なレベル</param>
    /// <param name="money">値段</param>
    public ItemInfo(int id, ItemType type, string picAdress, string name, int STR, int DEX, int INT, int CON, int LUK, int needSTR, int needDEX, int needINT, int needCON,int needLevel,int money)
    {
        if (type != ItemType.head &&
            type != ItemType.body &&
            type != ItemType.necklace &&
            type != ItemType.ring &&
            type != ItemType.bracelet &&
            type != ItemType.weapon &&
            type != ItemType.wing)
        {
            Debug.LogWarning("Bag data Error!");
            this.id = -1;
            type = ItemType.error;
        }
        else
        {
            this.id = id;
            this.type = type;
            this.adress = picAdress;
            this.name = name;
            this.STR = STR;
            this.DEX = DEX;
            this.INT = INT;
            this.CON = CON;
            this.LUK = LUK;
            this.needSTR = needSTR;
            this.needINT = needINT;
            this.needDEX = needDEX;
            this.needCON = needCON;
            this.needLevel = needLevel;
            this.money = money;
        }

    }

    /// <summary>
    /// 使用品（薬品）の初期化方法
    /// </summary>
    /// <param name="id">アイテムのID</param>
    /// <param name="picAress">アイテムの画像アドレス</param>
    /// <param name="name">アイテムの名前</param>
    /// <param name="HP">HP回復点数</param>
    /// <param name="energy">体力回復点数</param>
    /// <param name="money">値段</param>
    public ItemInfo(int id, string picAress, string name, int HP, int energy,int money)
    {
        this.id = id;
        this.adress = picAress;
        this.name = name;
        this.type = ItemType.drug;
        this.HP = HP;
        this.energy = energy;
        this.money = money;
    }
    #endregion
}

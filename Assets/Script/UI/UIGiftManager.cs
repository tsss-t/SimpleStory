using UnityEngine;
using System.Collections;

public class UIGiftManager : MonoBehaviour
{


    public GameObject giftPrefab;
    public static UIGiftManager _instance;

    void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatOneGift(GameObject enemy)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, giftPrefab);

        go.GetComponent<UIGiftAnimation>().target = enemy.transform;
    }
    public void DestoryGift(GameObject gift)
    {
        NGUITools.Destroy(gift);
    }


}

using UnityEngine;
using System.Collections;

public class UICoinManager : MonoBehaviour
{

    public GameObject coinPrefab;
    public static UICoinManager _instans;

    void Awake()
    {
        _instans = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatOneCoin(GameObject enemy, int size)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, coinPrefab);
        UICoinAnimation animation = go.GetComponent<UICoinAnimation>();
        animation.target = enemy.transform;
        animation.sizeScale = size;

    }
    public void DestoryCoin(GameObject coin)
    {
        NGUITools.Destroy(coin);
    }


}
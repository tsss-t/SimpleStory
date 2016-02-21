using UnityEngine;
using System.Collections;

public class SceneInfomation : MonoBehaviour {

    public int floorNum=-1000;
    public static SceneInfomation _instance;
    // Use this for initialization
    void Start () {
        _instance = this;
    }
}

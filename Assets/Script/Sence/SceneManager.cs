using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public int floorNum=-1000;
    public static SceneManager _instance;
    // Use this for initialization
    void Start () {
        _instance = this;
    }
}

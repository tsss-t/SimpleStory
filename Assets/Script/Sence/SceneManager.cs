using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public int floorNum=-1000;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(floorNum == -1000)
        {
            Debug.LogError("You must set the floorNumber in SceneManager");
        }
	}
}

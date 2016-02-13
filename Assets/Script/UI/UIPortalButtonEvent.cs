using UnityEngine;
using System.Collections;

public class UIPortalButtonEvent : MonoBehaviour {
    public int foolNumber;

	// Use this for initialization
	void Awake () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void OnButtonClick()
    {
        Debug.Log("GO TO FLOOR "+foolNumber);
    }

}

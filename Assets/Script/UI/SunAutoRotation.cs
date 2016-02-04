using UnityEngine;
using System.Collections;

public class SunAutoRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if( Time.frameCount%6==0)
        {
            transform.Rotate(0, 0.05f, 0);
        }
	}
}

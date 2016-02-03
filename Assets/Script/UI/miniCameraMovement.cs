using UnityEngine;
using System.Collections;

public class miniCameraMovement : MonoBehaviour {

    public Transform playerPosition;
    public float x;
    public float y;
    public float z;
	// Use this for initialization
	void Start () {
        playerPosition = GameObject.FindGameObjectWithTag(Tags.player).transform;
	}
	
	// Update is called once per frame
	void Update () {
        x = playerPosition.position.x;
        y = this.transform.position.y;
        z= playerPosition.position.z;

        this.transform.position=new Vector3(x, y, z);
        
    }
}

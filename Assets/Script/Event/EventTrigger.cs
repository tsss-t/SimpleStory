using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

    public EventDelegate eventDele;
    public bool Once=true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger && collider.gameObject.tag.Equals(Tags.player))
        {
            eventDele.Execute();
            if(Once)
            {
                this.enabled = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    
}

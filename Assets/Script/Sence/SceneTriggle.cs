using UnityEngine;
using System.Collections;

public class SceneTriggle : MonoBehaviour {

    public EventDelegate eventDele;

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
            this.enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}

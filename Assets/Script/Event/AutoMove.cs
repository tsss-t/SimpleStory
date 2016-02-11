using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {

    private NavMeshAgent agent;

    public Transform target;
	// Use this for initialization
	void Start () {
        agent = this.GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if(agent.enabled)
        {
            if(agent.remainingDistance<1)
            {
                agent.Stop();
                agent.enabled = false;
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            SetDestination(target.position);
        }
	}
    public void SetDestination(Vector3 targetPos)
    {
        agent.enabled = true;
        agent.SetDestination(targetPos);
    }
}

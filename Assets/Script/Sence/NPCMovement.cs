using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    NavMeshAgent NPCAgent;
    Animation NPCanimation;
    // Use this for initialization
    void Start()
    {
        NPCAgent = this.GetComponent<NavMeshAgent>();
        NPCanimation = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

        if (NPCAgent.enabled)
        {
            transform.rotation = Quaternion.LookRotation(NPCAgent.velocity);
            NPCanimation.CrossFade("Walk");
        }
        else
        {
            if (!NPCanimation.IsPlaying("Idle"))
            {
                NPCanimation.Play("Idle");
            }
        }
    }
}

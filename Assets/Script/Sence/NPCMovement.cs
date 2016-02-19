using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    NavMeshAgent NPCAgent;
    Animator anim;
    // Use this for initialization
    void Start()
    {
        NPCAgent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (NPCAgent.enabled)
        {
            transform.rotation = Quaternion.LookRotation(NPCAgent.velocity);
            anim.SetBool("Walk",true);
        }
        else
        {
            if (!anim.GetBool("Walk"))
            {
                anim.SetBool("Walk",false);
            }
        }
    }
}

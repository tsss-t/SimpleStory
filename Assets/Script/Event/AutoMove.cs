using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour
{
    PlayerState.PlayerAction endAction;
    private NavMeshAgent agent;
    PlayerState playerState;
    AudioSource audioStepSound;
    // Use this for initialization
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        playerState = PlayerState._instance;
        if (this.gameObject.tag.Equals(Tags.player))
        {
            audioStepSound = this.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {
            if (agent.remainingDistance < 1)
            {
                if (this.gameObject.tag.Equals(Tags.player))
                {
                    if (endAction != PlayerState.PlayerAction.NULL)
                    {
                        playerState.ChangeAction(endAction);
                    }
                    else
                    {
                        playerState.ChangeAction(PlayerState.PlayerAction.Free);
                    }
                    if (audioStepSound.isPlaying)
                    {
                        audioStepSound.Stop();
                    }
                }
                agent.Stop();

                agent.enabled = false;
            }
        }
    }
    public void StartDestination(Vector3 targetPos)
    {
        if (this.gameObject.tag.Equals(Tags.player))
        {
            this.endAction = PlayerState.PlayerAction.NULL;
            playerState.ChangeAction(PlayerState.PlayerAction.AutoMoving);
            if (!audioStepSound.isPlaying)
            {
                audioStepSound.Play();
            }

        }
        agent.enabled = true;
        agent.SetDestination(targetPos);



    }
    public void StartDestination(Vector3 targetPos, PlayerState.PlayerAction endAction)
    {
        if (this.gameObject.tag.Equals(Tags.player))
        {
            this.endAction = endAction;
            playerState.ChangeAction(PlayerState.PlayerAction.AutoMoving);
            if (!audioStepSound.isPlaying)
            {
                audioStepSound.Play();
            }
        }
        agent.enabled = true;
        agent.SetDestination(targetPos);

    }
    public bool IsMoveOver()
    {
        return !agent.enabled;
    }
}

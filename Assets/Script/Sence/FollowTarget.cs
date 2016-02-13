using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
    public Transform targetTransform;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            this.gameObject.transform.position=new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

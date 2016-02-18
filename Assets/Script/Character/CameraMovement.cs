using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float smooth = 1.5f;
    private Transform player;
    public Transform target;
    private Vector3 relCameraPos;
    private float relCameraPosMag;
    private Vector3 newPos;

    void Awake()
    {
        Transform StartPosition = this.transform;
        switch (GameController._instance.GetLastChangeSceneType())
        {
            case EntryType.Up:
                {
                    StartPosition = GameObject.FindGameObjectWithTag(Tags.UpPosition).transform;
                    break;
                }
            case EntryType.Down:
                {
                    StartPosition = GameObject.FindGameObjectWithTag(Tags.DownPosition).transform;
                    break;
                }
            case EntryType.Portal:
                {
                    StartPosition = GameObject.FindGameObjectWithTag(Tags.PortalPosition).transform;
                    break;
                }
        }


        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        target = player ;

        //相机偏移
        //relCameraPos = transform.position - target.position;
        relCameraPos = new Vector3(0,9,-4);

        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        this.transform.position = StartPosition.position + relCameraPos;


    }
    public void setTarget(Transform target)
    {
        this.target = target;
    }
    void FixedUpdate()
    {

        Vector3 standardPos = target.position + relCameraPos;
        Vector3 abovePos = target.position + Vector3.up * relCameraPosMag;
        Vector3[] checkPoints = new Vector3[5];
        checkPoints[0] = standardPos;
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
        checkPoints[4] = abovePos;


        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (ViewingPosCheck(checkPoints[i]))
            {
                break;
            }
        }
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
        SmoothLookAt();
    }
    bool ViewingPosCheck(Vector3 checkPos)
    {
        RaycastHit hit;
        Physics.Raycast(checkPos, target.position - checkPos, out hit, relCameraPosMag);
        if (hit.transform != target)
        {
            return false;
        }
        else
        {
            newPos = checkPos;
            return true;
        }
    }
    void SmoothLookAt()
    {
        Vector3 relPlayerPosition = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
    }
}
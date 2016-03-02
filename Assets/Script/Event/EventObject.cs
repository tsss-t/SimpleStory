using UnityEngine;
using System.Collections;
public enum ObjectType
{
    Destroy, Create
}
public class EventObject : MonoBehaviour
{
    public int eventID;
    public ObjectType objectType = ObjectType.Destroy;
    public bool UpdateAtStart = true;
    // Use this for initialization
    void Start()
    {
        UpdateState();
    }

    // Update is called once per frame
    void Update()
    {
        if(!UpdateAtStart)
        {
            UpdateState();
        }
    }

    void UpdateState()
    {
        try
        {
            //すでにやったイベント
            if (GameController._instance.getEventIsDone(eventID))
            {
                if (objectType == ObjectType.Destroy)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.gameObject.SetActive(true);
                }
            }
            //まだやっていないイベント
            else
            {
                if (objectType == ObjectType.Destroy)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        catch
        {
            Debug.Log("EventID Set Erorr");
        }
    }
}

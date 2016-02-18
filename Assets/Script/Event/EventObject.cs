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
    private bool isDone;
    // Use this for initialization
    void Start()
    {
        try
        {
            if (GameController._instance.getEventIsDone(eventID))
            {
                isDone = true;
                this.gameObject.SetActive(false);
            }
            else
            {
                isDone = false;
            }
        }
        catch
        {
            Debug.Log("EventID Set Erorr");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone)
        {
            if (objectType == ObjectType.Destroy)
            {
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
    }
}

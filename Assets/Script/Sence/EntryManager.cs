using UnityEngine;
using System.Collections;

public enum EntryType {
    Up,Down,Portal
}


public class EntryManager : MonoBehaviour
{
    public EntryType entryType;
    public int goToFloorNum=-1000;
    // Use this for initialization
    void Awake()
    {
        goToFloorNum = goToFloorNum ==- 1000?SceneManager._instance.floorNum:goToFloorNum;
    }    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(Tags.player)&&collider.isTrigger==false)
        {
            Debug.Log("IN!");
            GameController._instance.SetLastChangeSceneType(entryType);
            UISceneManager._instance.Show(Application.LoadLevelAsync(goToFloorNum));

        }
    }
}

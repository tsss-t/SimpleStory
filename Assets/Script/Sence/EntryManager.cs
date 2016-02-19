using UnityEngine;
using System.Collections;

public enum EntryType {
    Up,Down,Portal
}


public class EntryManager : MonoBehaviour
{
    public static EntryManager _instance;
    public EntryType entryType;
    public int goToFloorNum=-1000;

    public delegate void OnPlayerInEntry(AsyncOperation ao);
    public event OnPlayerInEntry onPlayerInEntry;

    // Use this for initialization
    void Awake()
    {
        _instance = this;
        goToFloorNum = goToFloorNum ==- 1000?SceneManager._instance.floorNum:goToFloorNum;
    }    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(Tags.player)&&collider.isTrigger==false)
        {
            GameController._instance.SetLastChangeSceneType(entryType);
            //FOR DEBUG
            onPlayerInEntry(Application.LoadLevelAsync(goToFloorNum));
            //Application.LoadLevelAsync(goToFloorNum);
        }
    }
}

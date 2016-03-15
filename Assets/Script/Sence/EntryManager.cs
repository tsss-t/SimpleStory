using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum EntryType
{
    Up, Down, Portal
}


public class EntryManager : MonoBehaviour
{
    public static EntryManager _instance;
    public EntryType entryType;
    public int goToFloorNum = -1000;

    public delegate void OnPlayerInEntry(AsyncOperation ao);
    public event OnPlayerInEntry onPlayerInEntry;

    // Use this for initialization
    void Awake()
    {
        _instance = this;

    }    // Update is called once per frame
    void Start()
    {
        if (entryType == EntryType.Up)
        {
            goToFloorNum = goToFloorNum ==(int) SceneFloorInfo.RandomMapFloor ? SceneInfomation._instance.FloorNumber + 1 : goToFloorNum;

        }
        else
        {
            goToFloorNum = goToFloorNum == (int)SceneFloorInfo.RandomMapFloor ? SceneInfomation._instance.FloorNumber - 1 : goToFloorNum;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(Tags.player) && collider.isTrigger == false)
        {
            GameController._instance.SetLastChangeSceneType(entryType);
            GameController._instance.SetGoingToFloor(goToFloorNum);
            //FOR DEBUG

            if (goToFloorNum == (int)SceneFloorInfo.Town)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Town));
            }
            else if (goToFloorNum == (int)SceneFloorInfo.FirstFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.FirstFloor));

            }
            else if (goToFloorNum == (int)SceneFloorInfo.LastFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.LastFloor));

            }
            else if (goToFloorNum == (int)SceneFloorInfo.ShopFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.ShopFloor));

            }
            else if(goToFloorNum==(int)SceneFloorInfo.BossFloor)
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.BossFloor));
            }
            else
            {
                UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.RandomMapFloor));
            }
            //SceneManager.LoadSceneAsync(goToFloorNum);
        }
    }
}

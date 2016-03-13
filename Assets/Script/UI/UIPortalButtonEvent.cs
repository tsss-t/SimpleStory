using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIPortalButtonEvent : MonoBehaviour
{
    public int floorNumber;
    UISceneManager LoadingBar;

    // Use this for initialization
    void Awake()
    {
        LoadingBar = UISceneManager._instance;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnButtonClick()
    {

        if (floorNumber == -1)
        {
            GameController._instance.SetGoingToFloor(-1);
            LoadingBar.Show(SceneManager.LoadSceneAsync(SceneName.FirstFloor));
        }
        if (floorNumber == -10)
        {
            LoadingBar.Show(SceneManager.LoadSceneAsync(SceneName.LastFloor));
            GameController._instance.SetGoingToFloor(-10);

        }
        GameController._instance.SetLastChangeSceneType(EntryType.Portal);
    }

}

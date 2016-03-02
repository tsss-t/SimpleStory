using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIStartMenuManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnNewButtonClick()
    {
        GameController._instance.newGame();
        UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Opening));
    }
    public void OnContinueButtonClick()
    {
        GameController._instance.Load();
        int floorNumber = GameController._instance.GetGoingToFloor();
        if (floorNumber == 0)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Town));
        }
        else if (floorNumber == -1)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.FirstFloor));

        }
        else if (floorNumber == -100)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.LastFloor));

        }
        else if (floorNumber == -101)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.ShopFloor));

        }
        else
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.RandomMapFloor));
        }
    }
    public void OnLoadButtonClick()
    {
        GameController._instance.Load();
        int floorNumber = GameController._instance.GetGoingToFloor();
        if (floorNumber == 0)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Town));
        }
        else if (floorNumber == -1)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.FirstFloor));

        }
        else if (floorNumber == -100)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.LastFloor));

        }
        else if(floorNumber==-101)
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.ShopFloor));

        }
        else
        {
            UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.RandomMapFloor));
        }

    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}

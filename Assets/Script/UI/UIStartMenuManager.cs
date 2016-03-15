using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIStartMenuManager : MonoBehaviour
{
    GameObject LoadContainer;
    // Use this for initialization
    void Start()
    {
        LoadContainer = transform.Find("SystemPanel").gameObject;
        LoadContainer.SetActive(false);
    }


    public void OnNewButtonClick()
    {
        GameController._instance.newGame();
        UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Opening));
    }
    public void OnContinueButtonClick()
    {
        LoadContainer.SetActive(true);
    }
    public void OnLoadButtonClick()
    {
        LoadContainer.SetActive(true);
    }
    public void OnLoadButtonCloseClick()
    {
        LoadContainer.SetActive(false);

    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}

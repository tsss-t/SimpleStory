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
        UISceneManager._instance.Show(SceneManager.LoadSceneAsync(SceneName.Opening));
    }
    public void OnContinueButtonClick()
    {

    }
    public void OnLoadButtonClick()
    {

    }
    public void OnExitButtonClick()
    {

    }
}

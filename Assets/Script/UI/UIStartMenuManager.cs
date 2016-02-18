using UnityEngine;
using System.Collections;

public class UIStartMenuManager : MonoBehaviour
{
    UISceneManager sceneManagerUI;
    // Use this for initialization
    void Start()
    {
        sceneManagerUI = UISceneManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnNewButtonClick()
    {
        sceneManagerUI.Show(Application.LoadLevelAsync(1));
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

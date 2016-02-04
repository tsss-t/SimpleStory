using UnityEngine;
using System.Collections;

public class UIStartMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNewButtonClick()
    {
        Application.LoadLevel(1);
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

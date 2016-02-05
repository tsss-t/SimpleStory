using UnityEngine;
using System.Collections;

public class UISceneManager : MonoBehaviour {

    public static UISceneManager _instance;
    private GameObject BG;
    private UISlider progressBar;
    private bool isAsyn = false;
    private AsyncOperation ao = null;

	// Use this for initialization
	void Awake () {
        _instance = this;
        BG = transform.Find("BG").gameObject;
        progressBar = BG.transform.Find("ProgressBar").GetComponent<UISlider>();

        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
	
        if(isAsyn)
        {
            progressBar.value = ao.progress;
            if(ao.progress==1)
            {
                Hide();
            }
        }
	}

    public void Show(AsyncOperation ao)
    {
        gameObject.SetActive(true);
        BG.SetActive(true);
        isAsyn = true;
        this.ao = ao;
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}

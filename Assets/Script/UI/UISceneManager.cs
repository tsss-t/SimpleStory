using UnityEngine;
using System.Collections;

public class UISceneManager : MonoBehaviour
{

    public static UISceneManager _instance;
    private GameObject BG;
    private UISlider progressBar;
    private bool isAsyn = false;
    private AsyncOperation ao = null;

    // Use this for initialization
    void Start()
    {
        _instance = this;
        if (EntryManager._instance != null)
        {
            EntryManager._instance.onPlayerInEntry += Show;
        }
        BG = transform.Find("BG").gameObject;
        progressBar = BG.transform.Find("ProgressBar").GetComponent<UISlider>();

        BG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (isAsyn)
        {
            progressBar.value = ao.progress;
            if (ao.progress == 1)
            {
                Hide();
            }
        }
    }
    public void Show(AsyncOperation ao)
    {

        BG.SetActive(true);
        isAsyn = true;
        this.ao = ao;
    }
    void Hide()
    {
        BG.SetActive(false);
    }
    void OnDestroy()
    {
        if (EntryManager._instance != null)
        {
            EntryManager._instance.onPlayerInEntry -= Show;

        }
    }
}

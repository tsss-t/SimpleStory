using UnityEngine;
using System.Collections;

public class EntryManager : MonoBehaviour
{

    public int goToFloorNum=-1000;
    private UISceneManager sceneManagerUI;
    // Use this for initialization
    void Awake()
    {
        sceneManagerUI = GameObject.FindGameObjectWithTag(Tags.UISceneLoading).GetComponent<UISceneManager>();
        goToFloorNum = goToFloorNum ==- 1000? GameObject.FindGameObjectWithTag(Tags.sceneManager).GetComponent<SceneManager>().floorNum:goToFloorNum;
    }    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(Tags.player))
        {
            sceneManagerUI.Show(Application.LoadLevelAsync(goToFloorNum));
        }
    }
}

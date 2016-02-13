using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public TextAsset playerStateData;
    public TextAsset playerTypeData;
    public TextAsset bagData;
    public TextAsset itemListData;
    public TextAsset playerQuestData;
    public TextAsset questListData;
    public TextAsset NPCData;
    public TextAsset skillData;
    public TextAsset openingData;
    public TextAsset portalData;
    public string gameDataKey;

    public static GameManager _instans;
    // Use this for initialization
    void Awake () {
        _instans = this;
        if(GameObject.FindGameObjectWithTag(Tags.player)!=null)
        {
            PlayerState.GamePlayerState.playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;

        }
        gameDataKey = SystemInfo.deviceUniqueIdentifier;
    }

    #region Path

    //获取路径//
    public static string GetDataPath()
    {
        // Your game has read+write access to /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/Documents
        // Application.dataPath returns ar/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data             
        // Strip "/Data" from path
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            // Strip application name
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents";
        }
        else
            //    return Application.dataPath + "/Resources";
            return Application.dataPath;
    }
    #endregion

}

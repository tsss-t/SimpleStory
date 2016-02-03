using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationInfo : MonoBehaviour {

    public EventDelegate OnPanelShow;
    public EventDelegate OnActionShow;
    public EventDelegate OnCommunicationShow;

    public void CommunicationStart()
    {
        //TODO:各種のコミュニケーション形式
        OnCommunicationShow.Execute();
        OnActionShow.Execute();
        OnPanelShow.Execute();
    }

}

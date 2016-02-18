using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalkingManager : MonoBehaviour
{
    public static TalkingManager _instance;
    List<TalkText> talkList;

    GameObject talkContainer;
    UISprite icon;
    UILabel talk;
    int index;
    // Use this for initialization
    void Start()
    {
        _instance = this;
        talkContainer = transform.Find("TalkContainer").gameObject;
        talkList = new List<TalkText>();
        icon = talkContainer.transform.Find("IconContainer/Icon").GetComponent<UISprite>();
        talk = talkContainer.transform.Find("TalkContainer/Talk").GetComponent<UILabel>();

        talkContainer.SetActive(false);
    }
    void Update()
    {
        if (talkContainer.activeSelf)
        {
            if (Input.GetButtonUp("Comunication"))
            {
                index++;
                thispage = true;
            }
        }
    }
    bool thispage;
    public void PlayEvent(int eventID)
    {
        talkContainer.SetActive(true);
        talkList = GameController._instance.LoadText(eventID);
        thispage = true;
        StartCoroutine(startEvent());
    }
    IEnumerator startEvent()
    {
        for (index = 0; index < talkList.Count;)
        {
            if (thispage)
            {
                if (talkList[index].textType == TextType.text)
                {
                    icon.spriteName = talkList[index].iconName;
                    talk.text = string.Format("【{0}】\n {1}", talkList[index].speakerName, talkList[index].textInfo);
                }
                thispage = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
        EventManager._instance.SetEventOver();
        talkContainer.SetActive(false);
    }
}

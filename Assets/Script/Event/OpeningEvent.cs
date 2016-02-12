using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OpeningEvent : MonoBehaviour {
    List<TalkText> textList;

    UISceneManager loadingUI;

    UILabel text;
    UITweener textTweener;


    UILabel textArea;
    UITweener textAreaTweener;

    UISprite picture;
    UITweener pictureTweener;

    UITweener playText;
	// Use this for initialization
	void Start () {
        loadingUI = GameObject.FindGameObjectWithTag(Tags.UISceneLoading).GetComponent<UISceneManager>();

        text = transform.Find("Panel/Text").GetComponent<UILabel>();
        textTweener = text.gameObject.GetComponent<UITweener>();

        textArea =transform.Find("Panel/TextArea").GetComponent<UILabel>();
        textAreaTweener = textArea.gameObject.GetComponent<UITweener>();

        picture =transform.Find("Panel/Picture").GetComponent<UISprite>();
        pictureTweener = picture.gameObject.GetComponent<UITweener>();

        textList = GameController._instance.LoadText(0);

        textTweener.PlayReverse();
        textAreaTweener.PlayReverse();
        pictureTweener.PlayReverse();
        //picture.spriteName = "手紙";
        StartCoroutine(Play());
    }
	
    IEnumerator Play()
    {
        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < textList.Count; i++)
        {
            switch (textList[i].textType)
            {
                case TextType.text:
                    text.text = textList[i].textInfo;
                    playText = textTweener;
                    break;
                case TextType.textArea:
                    textArea.text = textList[i].textInfo;
                    playText = textAreaTweener;
                    break;
                case TextType.picture:
                    if (!textList[i].textInfo.Equals(""))
                    {
                        picture.spriteName = textList[i].textInfo;
                    }
                    break;
                default:
                    break;
            }
            switch (textList[i].textEffect)
            {
                case TextEffect.foi:
                    playText.PlayForward();
                    yield return new WaitForSeconds(textTweener.duration);

                    if(textTweener.gameObject.name.Equals(text.gameObject.name))
                    {
                        yield return new WaitForSeconds(textTweener.duration * 3);
                    }
                    else
                    {
                        yield return new WaitForSeconds(textTweener.duration * 7);
                    }


                    playText.PlayReverse();
                    yield return new WaitForSeconds(textTweener.duration);

                    break;
                case TextEffect.pictureIn:
                    pictureTweener.PlayForward();
                    yield return new WaitForSeconds(textTweener.duration);
                    break;
                case TextEffect.pictureOut:
                    pictureTweener.PlayReverse();
                    yield return new WaitForSeconds(textTweener.duration);
                    break;
                default:
                    break;
            }
        }
        yield return new WaitForSeconds(3.0f);
        loadingUI.Show(Application.LoadLevelAsync(2));
    }
}

using UnityEngine;
using System.Collections;

public class UIComposeButtonEvent : MonoBehaviour
{


    ItemCompose itemCompose;

    GameObject ComposeResult;
    GameObject ComposeClip1;
    GameObject ComposeClip2;
    GameObject ComposeClip3;
    GameObject ComposeClip4;

    UILabel ComposeResultCount;
    UILabel ComposeClipCount1;
    UILabel ComposeClipCount2;
    UILabel ComposeClipCount3;
    UILabel ComposeClipCount4;

    UISprite ComposeResultSprite;
    UISprite ComposeClipSprite1;
    UISprite ComposeClipSprite2;
    UISprite ComposeClipSprite3;
    UISprite ComposeClipSprite4;


    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        ComposeResult = transform.Find("ComposeResult").gameObject;
        ComposeResultCount = ComposeResult.transform.Find("Count/CountLabel").GetComponent<UILabel>();
        ComposeResultSprite = ComposeResult.transform.Find("Icon").GetComponent<UISprite>();
        if (itemCompose.ResultItem.itemCount == 1)
        {
            ComposeResult.transform.Find("Count").gameObject.SetActive(false);
        }

        ComposeClip1 = transform.Find("ComposeClip1").gameObject;
        ComposeClipCount1 = ComposeClip1.transform.Find("Count/CountLabel").GetComponent<UILabel>();
        ComposeClipSprite1 = ComposeClip1.transform.Find("Icon").GetComponent<UISprite>();

        if (itemCompose.NeedItem[0].itemCount == 1)
        {
            ComposeClip1.transform.Find("Count").gameObject.SetActive(false);
        }




        switch (itemCompose.NeedItem.Count)
        {
            case 2:
                {

                    ComposeClip2 = transform.Find("ComposeClip2").gameObject;
                    ComposeClipCount2 = ComposeClip2.transform.Find("Count/CountLabel").GetComponent<UILabel>();
                    ComposeClipSprite2 = ComposeClip2.transform.Find("Icon").GetComponent<UISprite>();
                    if (itemCompose.NeedItem[1].itemCount == 1)
                    {
                        ComposeClip2.transform.Find("Count").gameObject.SetActive(false);
                    }

                    break;
                }
            case 3:
                {
                    ComposeClip3 = transform.Find("ComposeClip3").gameObject;
                    ComposeClipCount3 = ComposeClip3.transform.Find("Count/CountLabel").GetComponent<UILabel>();
                    ComposeClipSprite3 = ComposeClip3.transform.Find("Icon").GetComponent<UISprite>();
                    if (itemCompose.NeedItem[2].itemCount == 1)
                    {
                        ComposeClip3.transform.Find("Count").gameObject.SetActive(false);
                    }
                    break;
                }

            case 4:
                {
                    ComposeClip4 = transform.Find("ComposeClip4").gameObject;
                    ComposeClipCount4 = ComposeClip4.transform.Find("Count/CountLabel").GetComponent<UILabel>();
                    ComposeClipSprite4 = ComposeClip4.transform.Find("Icon").GetComponent<UISprite>();
                    if (itemCompose.NeedItem[3].itemCount == 1)
                    {
                        ComposeClip4.transform.Find("Count").gameObject.SetActive(false);
                    }

                    break;
                }
            default:
                {
                    Debug.LogError(string.Format("Item ID: {0} Item Compose Data Error", itemCompose.ResultItem.itemID));
                    NGUITools.Destroy(this);
                    break;
                }

        }
        UpdateButton();
    }
    public void UpdateButton()
    {
        SetLabelCount(ComposeResultCount, itemCompose.ResultItem.itemID, itemCompose.ResultItem.itemCount, true);
        ComposeResultSprite.spriteName = ItemList.getItem(itemCompose.ResultItem.itemID).adress;

        SetLabelCount(ComposeClipCount1, itemCompose.NeedItem[0].itemID, itemCompose.NeedItem[0].itemCount, false);
        ComposeClipSprite1.spriteName = ItemList.getItem(itemCompose.NeedItem[0].itemID).adress;


        switch (itemCompose.NeedItem.Count)
        {
            case 2:
                {
                    SetLabelCount(ComposeClipCount2, itemCompose.NeedItem[1].itemID, itemCompose.NeedItem[1].itemCount, false);
                    ComposeClipSprite2.spriteName = ItemList.getItem(itemCompose.NeedItem[1].itemID).adress;

                    break;
                }
            case 3:
                {
                    SetLabelCount(ComposeClipCount3, itemCompose.NeedItem[2].itemID, itemCompose.NeedItem[2].itemCount, false);
                    ComposeClipSprite3.spriteName = ItemList.getItem(itemCompose.NeedItem[2].itemID).adress;

                    break;
                }

            case 4:
                {
                    SetLabelCount(ComposeClipCount4, itemCompose.NeedItem[3].itemID, itemCompose.NeedItem[3].itemCount, false);
                    ComposeClipSprite4.spriteName = ItemList.getItem(itemCompose.NeedItem[3].itemID).adress;

                    break;
                }
            default:
                {
                    Debug.LogError(string.Format("Item ID: {0} Item Compose Data Error", itemCompose.ResultItem.itemID));
                    NGUITools.Destroy(this);
                    break;
                }
        }
    }


    void OnClick()
    {

        UIComposeManager._instance.OnComposeItemClick(this);

    }

    void SetLabelCount(UILabel label, int itemID, int count, bool isResult)
    {
        string color;

        if (isResult)
        {
            color = "000000";
        }
        else
        {
            if (PlayerState._instance.GetPlayerBag().GetItemCount(itemID) >= count)
            {
                color = "00FF4B";
            }
            else
            {
                color = "FF0000";
            }
        }
        label.text = string.Format("[{0}]{1}[-]", color, count.ToString());

    }

    public void SetItemCompose(ItemCompose itemCompose)
    {
        this.itemCompose = itemCompose;
    }
    public ItemCompose getItemCompose()
    {
        return this.itemCompose;
    }



}

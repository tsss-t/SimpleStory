using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPortalManager : MonoBehaviour
{
    public GameObject protalPrefab;
    public GameObject protalPrefabOn;
    bool isShowPanel;
    UITweener tweener;


    GameObject portalGrid;
    // Use this for initialization
    void Start()
    {
        isShowPanel = false;
        tweener = transform.Find("PortalContainer").GetComponent<UITweener>();
        portalGrid = transform.Find("PortalContainer/Scroll View/Items").gameObject;
        if (PortalManager._instans != null)
        {
            PortalManager._instans.playerStateChange += PlayerPositionChange;
        }

        LoadData();
        tweener.gameObject.SetActive(false);
    }
    GameObject go;
    void LoadData()
    {
        foreach (KeyValuePair<int, bool> item in GameController._instance.getPortalList())
        {
            if (item.Key == SceneManager._instance.floorNum)
            {
                go = NGUITools.AddChild(portalGrid, protalPrefabOn);
                go.transform.Find("mark").GetComponent<UISprite>().spriteName = "pic_星星";
            }
            else
            {
                if (item.Value)
                {
                    go = NGUITools.AddChild(portalGrid, protalPrefabOn);
                    go.transform.Find("mark").GetComponent<UISprite>().spriteName = "";
                }
                else
                {
                    go = NGUITools.AddChild(portalGrid, protalPrefab);
                    go.transform.Find("mark").GetComponent<UISprite>().spriteName = "";
                    go.transform.Find("BG").GetComponent<UIButton>().enabled = false;
                }

            }
            go.transform.Find("FloorNumber").GetComponent<UILabel>().text = (-item.Key).ToString();
            go.GetComponent<UIPortalButtonEvent>().floorNumber = -item.Key;
            portalGrid.GetComponent<UIGrid>().enabled = true;
        }
    }

    void PlayerPositionChange(bool isIn)
    {
        if (isIn)
        {

        }
        else
        {
            OnCloseButtonClick();
        }
    }



    public void OnCloseButtonClick()
    {
        Hide();
    }
    public void OnOpenButtonClick()
    {
        Show();
    }
    public void OnToggleButtonClick()
    {
        if (isShowPanel)
        {
            Hide();
        }
        else
        {
            Show();
        }

    }



    void Show()
    {
        if (!isShowPanel)
        {
            tweener.gameObject.SetActive(true);
            UIController._instance.CloseAllWindows();
            StartCoroutine(ShowPanel());
        }
    }
    void Hide()
    {
        if (isShowPanel)
        {
            StartCoroutine(HidePanel());
        }
    }
    #region Panel CUTIN/OUT
    IEnumerator HidePanel()
    {

        tweener.PlayForward();
        yield return new WaitForSeconds(tweener.duration);
        tweener.gameObject.SetActive(false);
        isShowPanel = false;
    }
    IEnumerator ShowPanel()
    {
        tweener.PlayReverse();
        yield return new WaitForSeconds(tweener.duration);
        isShowPanel = true;
    }
    #endregion


    void OnDestroy()
    {
        if (PortalManager._instans != null)
        {
            PortalManager._instans.playerStateChange -= PlayerPositionChange;
        }
    }
}

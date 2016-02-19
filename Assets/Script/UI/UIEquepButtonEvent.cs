using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIEquepButtonEvent : MonoBehaviour
{
    #region para
    UIItemInfoPanel ItemInfoPanel;
    PlayerState playerState;
    Item item;
    #endregion
    #region start
    // Use this for initialization
    void Awake()
    {
        ItemInfoPanel = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("EquepMenu").Find("ItemInfoPanel").GetComponent<UIItemInfoPanel>();
        playerState = PlayerState._instance;
        OnEquepChanged();
        playerState.OnPlayerStateChanged += OnStateChanged;
    }
    #endregion
    #region delegate
    void OnStateChanged(PlayerStateChangeType type)
    {
        if (type == PlayerStateChangeType.equep)
        {
            OnEquepChanged();
        }
    }
    void OnEquepChanged()
    {
        switch (gameObject.name)
        {
            case "EquepHead":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.head];
                    break;
                }
            case "EquepNeck":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.necklace];
                    break;
                }
            case "EquepCoad":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.body];
                    break;
                }
            case "EquepFoot":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.foot];
                    break;
                }
            case "EquepBra":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.bracelet];
                    break;
                }
            case "EquepRing":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.ring];
                    break;
                }
            case "EquepWing":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.wing];
                    break;
                }
            case "EquepWeapon":
                {
                    item = playerState.GetPlayerEquep().dictionaryEquep[ItemType.weapon];
                    break;
                }
        }
        if (item == null)
        {
            GetComponent<UISprite>().spriteName = "bg_道具";
            GetComponent<UIButton>().normalSprite = GetComponent<UISprite>().spriteName;
        }
        else
        {
            GetComponent<UISprite>().spriteName = item.info.adress;
            GetComponent<UIButton>().normalSprite = GetComponent<UISprite>().spriteName;
        }
    }
    #endregion

    #region UI Event
    void OnClick()
    {
        ItemInfoPanel.ShowInfo(this.item, ItemFrom.Equep);
    }
    #endregion

    void OnDestroy()
    {
        playerState.OnPlayerStateChanged -= OnStateChanged;
    }
}

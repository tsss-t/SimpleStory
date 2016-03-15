using UnityEngine;
using System.Collections;

public enum SystemItemType
{
    save1, save2, save3, save4, load1, load2, load3, load4, none, exit
}

public class UISystemItemEvent : MonoBehaviour
{
    #region Editor
    public SystemItemType ItemType;
    #endregion
    // Use this for initialization
    void Start()
    {

    }
    void OnClick()
    {
        if (UISystemManager._instance == null)
        {
            switch (ItemType)
            {

                case SystemItemType.load1:
                    GameController._instance.Load(1);
                    break;
                case SystemItemType.load2:
                    GameController._instance.Load(2);

                    break;
                case SystemItemType.load3:
                    GameController._instance.Load(3);

                    break;
                case SystemItemType.load4:
                    GameController._instance.Load(4);

                    break;
            }
        }
        else
        {
            UISystemManager._instance.OnSaveLoadDataButtonClick(ItemType);
        }
    }
}

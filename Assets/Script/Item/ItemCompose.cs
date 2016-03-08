using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct itemSet
{
    public int itemID;
    public int itemCount;
}

public class ItemCompose
{
    itemSet resultItem;
    List<itemSet> needItem;

    public itemSet ResultItem
    {
        get
        {
            return resultItem;
        }

        set
        {
            resultItem = value;
        }
    }

    public List<itemSet> NeedItem
    {
        get
        {
            return needItem;
        }

        set
        {
            needItem = value;
        }
    }

    public ItemCompose(int itemResultID, int itemResultCount, int[] itemNeedID, int[] itemNeedCount)
    {
        if (itemNeedID.Length != itemNeedCount.Length)
        {
            Debug.LogError("合成データ　エラー");
        }
        else
        {
            needItem = new List<itemSet>();
            this.resultItem.itemID = itemResultID;
            this.resultItem.itemCount = itemResultCount;

            for (int i = 0; i < itemNeedID.Length; i++)
            {
                needItem.Add(new itemSet() { itemID = itemNeedID[i], itemCount = itemNeedCount[i] });
            }
        }

    }
}

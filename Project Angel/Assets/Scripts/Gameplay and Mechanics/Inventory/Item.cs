using System;
using UnityEngine;

public class Item : ScriptableObject, IComparable<Item>
{
    public string itemName;
    public string itemDescription;
    public int itemID;
    public Sprite itemIcon;
    public int worth;

    public int CompareTo(Item compareItem)
    {
        // A null value means that this object is greater.
        if (compareItem == null)
            return -1;

        else
            return this.itemID.CompareTo(compareItem.itemID);
    }

}

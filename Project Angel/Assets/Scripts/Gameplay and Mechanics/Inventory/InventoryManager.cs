using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    public Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();

    public void AddItem(Item item)
    {

        if (itemCounts.ContainsKey(item))
            itemCounts[item]++;
        else
            itemCounts.Add(item, 1);

    }

    public void RemoveITem(Item item)
    {
        if (itemCounts.ContainsKey(item))
        {
            itemCounts[item]--;
            if (itemCounts[item] == 0)
                itemCounts.Remove(item);
        }
    }

    public Dictionary<Item, int> GetItemsByType<T>() where T : Item
    {

        Dictionary<Item, int> itemTypeCount = new Dictionary<Item, int>();

        List<Item> items = new List<Item>(itemCounts.Keys);

        foreach(Item i in items)
        {
            if(i is T)
                itemTypeCount.Add(i, itemCounts[i]);
        }

        return itemTypeCount;

    }

    private void Awake()
    {
        Instance = this;
    }

}

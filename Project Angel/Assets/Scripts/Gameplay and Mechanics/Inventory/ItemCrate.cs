using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrate : Interactable
{
    public Item item;

    protected override void DoInteract()
    {
        InventoryManager.Instance.AddItem(item);
        print("Item Grabbed");
        Destroy(this);
    }

}

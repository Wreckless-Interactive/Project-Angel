using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Restoreable", menuName = "Wreckless/Item/Restoreable")]
public class RestorableItem : Item
{
   
    public enum RestoreType { Health, Mana, Revive};
    public RestoreType restoreType;
    public int restoreValue;

}

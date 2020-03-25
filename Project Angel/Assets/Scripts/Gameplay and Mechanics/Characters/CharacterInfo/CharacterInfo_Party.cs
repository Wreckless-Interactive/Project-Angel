using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Party Character", menuName = "Wreckless/Characters/Party")]
public class CharacterInfo_Party : CharacterInfo
{
    [Header("Party Info")]
    public int baseMaxMana;
}

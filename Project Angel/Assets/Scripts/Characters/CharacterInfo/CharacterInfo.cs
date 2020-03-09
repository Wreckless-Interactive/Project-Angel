using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : ScriptableObject
{

    [Header("Basic Character Info")]
    public string characterName;

    [Header("Character Stats")]
    public int basePhysicalAttack;
    public int basePhysicalDefense;
    public int baseMagicalAttack;
    public int baseMagicalDefense;
    public int baseMaxHealth;
    public int baseSpeed;

    public enum ElementType { Water, Fire, Ice, Lightning, Earth, Death, Life};
    [Header("Elemental")]
    public ElementType elemental;

}

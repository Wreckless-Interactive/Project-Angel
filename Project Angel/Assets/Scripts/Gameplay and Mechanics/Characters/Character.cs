using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Wreckless/Characters/Character")]
public class Character : ScriptableObject
{

    public BattleCharacter bCharacter;
    public WorldCharacter wCharacter;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalManager : MonoBehaviour
{

    public static ElementalManager Instance;

    public enum ElementAdvantage
    {
        Strong,
        Weak,
        Neutral
    };

    private Dictionary<CharacterInfo.ElementType, List<CharacterInfo.ElementType>> StrongVs;
    private Dictionary<CharacterInfo.ElementType, List<CharacterInfo.ElementType>> WeakVs;

    private void Start()
    {

        StrongVs = new Dictionary<CharacterInfo.ElementType, List<CharacterInfo.ElementType>>();
        WeakVs = new Dictionary<CharacterInfo.ElementType, List<CharacterInfo.ElementType>>();

        StrongVs.Add(CharacterInfo.ElementType.Water, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Ice });
        StrongVs.Add(CharacterInfo.ElementType.Fire, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Lightning });
        StrongVs.Add(CharacterInfo.ElementType.Ice, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Water });
        StrongVs.Add(CharacterInfo.ElementType.Lightning, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Fire });
        StrongVs.Add(CharacterInfo.ElementType.Earth, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Earth });
        StrongVs.Add(CharacterInfo.ElementType.Death, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Death });
        StrongVs.Add(CharacterInfo.ElementType.Life, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Life });

        WeakVs.Add(CharacterInfo.ElementType.Water, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Lightning });
        WeakVs.Add(CharacterInfo.ElementType.Fire, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Water });
        WeakVs.Add(CharacterInfo.ElementType.Ice, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Fire });
        WeakVs.Add(CharacterInfo.ElementType.Lightning, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Ice });
        WeakVs.Add(CharacterInfo.ElementType.Earth, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Fire, CharacterInfo.ElementType.Water });
        WeakVs.Add(CharacterInfo.ElementType.Death, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Life });
        WeakVs.Add(CharacterInfo.ElementType.Life, new List<CharacterInfo.ElementType>() { CharacterInfo.ElementType.Death });

    }

    public ElementAdvantage CompareElements(CharacterInfo.ElementType sourceType, CharacterInfo.ElementType targetType)
    {
        if (StrongVs.ContainsKey(sourceType) && StrongVs[sourceType].Contains(targetType))
            return ElementAdvantage.Strong;

        if (WeakVs.ContainsKey(sourceType) && WeakVs[sourceType].Contains(targetType))
            return ElementAdvantage.Weak;

        // If no element match ups found, means they're neutral
        return ElementAdvantage.Neutral;
    }


    private void Awake()
    {
        Instance = this;
    }

}

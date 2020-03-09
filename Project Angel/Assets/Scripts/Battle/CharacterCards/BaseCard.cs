using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour, IDestroyable
{

    [Header("Character Info")]
    public TextMeshProUGUI nameTxt;
    public Transform statusEffectPanel;

    public GameObject statusEffectPrefab;

    public Dictionary<string, GameObject> statusEffects = new Dictionary<string, GameObject>();

    protected BattleCharacter character;

    public virtual void UpdateCard() {}

    public virtual void InitCard(BattleCharacter character)
    {
        this.character = character;
    }

    public BattleCharacter GetCharacter()
    {
        return character;
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        if (!statusEffects.ContainsKey(effect.GetType().Name))
        {
            Transform obj = Instantiate(statusEffectPrefab, statusEffectPanel).transform;
            obj.GetComponent<Image>().sprite = effect.GetSprite();
            statusEffects.Add(effect.GetType().Name, obj.gameObject);
        }
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        if (statusEffects.ContainsKey(effect.GetType().Name))
        {
            var obj = statusEffects[effect.GetType().Name];
            statusEffects.Remove(effect.GetType().Name);
            Destroy(obj);
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}

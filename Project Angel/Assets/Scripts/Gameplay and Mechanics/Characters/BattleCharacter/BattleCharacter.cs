using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class BattleCharacter : BaseCharacter
{

    public CharacterInfo info;

    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            _health = Mathf.Clamp(_health, 0, info.baseMaxHealth);

            if (_health <= 0)
                Die();


        }
    }

    public bool IsGuarding { get; set; }

    private List<Type> statusEffects = new List<Type>();
    private Dictionary<Type, int> effectTimer = new Dictionary<Type, int>();

    public Vector3 startingPoint;

    public virtual void TakeDamage(BattleCharacter opponent)
    {

        int dmg = opponent.info.basePhysicalAttack;

        switch(ElementalManager.Instance.CompareElements(info.elemental, opponent.info.elemental))
        {
            case ElementalManager.ElementAdvantage.Neutral:
                break;
            case ElementalManager.ElementAdvantage.Weak:
                //50% less damage
                dmg -= Mathf.CeilToInt((float)dmg / 5);
                break;
            case ElementalManager.ElementAdvantage.Strong:
                //50% more damage;
                dmg += Mathf.CeilToInt((float)dmg / 5);
                break;
        }


        if (IsGuarding)
        {
            dmg = Mathf.RoundToInt((float)dmg / 2f);
            IsGuarding = false;
        }

        Health -= dmg;

        BattleHUD.Instance.UpdateCharacterCard(this);

    }

    public void TakeDamage(int dmg)
    {

        Health -= dmg;

        BattleHUD.Instance.UpdateCharacterCard(this);
    }

    public void AddStatusEffect<T>() where T : StatusEffect, new()
    {
        var effect = new T();
        effect.Init();
        statusEffects.Add(typeof(T));

        if (!effectTimer.ContainsKey(typeof(T)))
            effectTimer.Add(typeof(T), effect.GetTurnTimer());

        BattleHUD.Instance.AddCardEffects(this, effect);
    }

    public void RemoveStatusEffect<T>() where T : StatusEffect
    {
        if (statusEffects.Contains(typeof(T)))
        {
            BattleHUD.Instance.RemoveCardEffects(this, Activator.CreateInstance(typeof(T)) as StatusEffect);
            statusEffects.Remove(typeof(T));
            effectTimer.Remove(typeof(T));
        }
    }

    private void RemoveStatusEffect(Type type)
    {

        if (statusEffects.Contains(type))
        {
            BattleHUD.Instance.RemoveCardEffects(this, Activator.CreateInstance(type) as StatusEffect);
            statusEffects.Remove(type);
            effectTimer.Remove(type);
        }
    }

    public void RemoveStatusEffecMultiplet<T>() where T : StatusEffect
    {
        foreach (Type type in statusEffects)
        {
            if (type.IsSubclassOf(typeof(T)))
            {
                BattleHUD.Instance.RemoveCardEffects(this, Activator.CreateInstance(type) as StatusEffect);
                statusEffects.Remove(typeof(T));
                effectTimer.Remove(typeof(T));
            }
        }
    }

    public virtual void ProcessEffects()
    {

        foreach (var effectType in statusEffects.ToArray())
        {
            var effect = Activator.CreateInstance(effectType) as StatusEffect;
            effect.Process(this);

            effectTimer[effectType]--;

            if (effectTimer[effectType] == 0)
            {
                RemoveStatusEffect(effectType);
            }

        }

    }

    protected abstract void Die();

    public virtual void InitCharacter()
    {
        Health = info.baseMaxHealth;
        startingPoint = transform.position;
    }

    private StatusEffect GetStatusEffect(Type compareTo)
    {

        foreach (Type type in statusEffects)
        {
            if (type.Name.Equals(compareTo.Name))
                return Activator.CreateInstance(type) as StatusEffect;
        }

        return null;

    }

}

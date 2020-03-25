using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter_Party : BattleCharacter
{

    public enum Stances { Neutral = 0, Life = 1, Death = 2, Elemental = 3};
    public Stances currentStance = Stances.Neutral;

    public bool IsKnockedOut;

    public int TP;
    private int tpTimer;

    private int _mana;
    public int Mana
    {
        get { return _mana; }
        set
        {
            _mana = value;
            _mana = Mathf.Clamp(_mana, 0, (info as CharacterInfo_Party).baseMaxMana);
        }
    }

    public void SetStance(Stances stance)
    {

        switch (stance)
        {
            case Stances.Neutral: //Neutral
                currentStance = Stances.Neutral;
                RemoveStatusEffecMultiplet<StanceEffects>();
                break;
            case Stances.Life: //Life
                RemoveStatusEffecMultiplet<StanceEffects>();
                AddStatusEffect<WardingStance>();
                currentStance = Stances.Life;
                break;
            case Stances.Death: //Death
                RemoveStatusEffecMultiplet<StanceEffects>();
                AddStatusEffect<StrengthStance>();
                AddStatusEffect<FortitudeStance>();
                currentStance = Stances.Death;
                break;
            case Stances.Elemental: //Elemental
                RemoveStatusEffecMultiplet<StanceEffects>();
                AddStatusEffect<WardingStance>();
                AddStatusEffect<PowerStance>();
                currentStance = Stances.Elemental;
                break;
        }

    }

    public override void ProcessEffects()
    {
        base.ProcessEffects();

        if (TP < 6)
        {
            tpTimer++;

            if (tpTimer == 2)
            {
                tpTimer = 0;
                TP++;
            }
        }

    }

    protected override void Die()
    {
        IsKnockedOut = true;
    }

    public override void InitCharacter()
    {
        base.InitCharacter();
        Mana = (info as CharacterInfo_Party).baseMaxMana;
    }

}

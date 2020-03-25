using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{

    protected Sprite statusIcon;
    protected int maxTurnTimer;

    public abstract void Init();

    public abstract void Process(BattleCharacter character);

    public int GetTurnTimer()
    {
        return maxTurnTimer;
    }

    public Sprite GetSprite()
    {
        return statusIcon;
    }

}

public class TestStatus: StatusEffect
{

    public override void Init()
    {
        statusIcon = Resources.Load<Sprite>("Icons/Status Effects/katana");
        maxTurnTimer = 4;
    }

    public override void Process(BattleCharacter character)
    {
        Debug.Log("Effect");
    }
}

public class StanceEffects : StatusEffect
{
    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Process(BattleCharacter character)
    {
        throw new System.NotImplementedException();
    }
}

public class StrengthStance : StanceEffects
{
    public override void Init()
    {
        statusIcon = Resources.Load<Sprite>("Icons/Status Effects/katana");
        maxTurnTimer = -1;
    }

    public override void Process(BattleCharacter character)
    {
        Debug.Log("Attack Damage Up");
    }
}

public class FortitudeStance : StanceEffects
{
    public override void Init()
    {
        statusIcon = Resources.Load<Sprite>("Icons/Status Effects/armor-vest");
        maxTurnTimer = -1;
    }

    public override void Process(BattleCharacter character)
    {
        Debug.Log("Attack Defense Up");
    }
}

public class PowerStance : StanceEffects
{
    public override void Init()
    {
        statusIcon = Resources.Load<Sprite>("Icons/Status Effects/fairy-wand");
        maxTurnTimer = -1;
    }

    public override void Process(BattleCharacter character)
    {
        Debug.Log("Magic Attack Up");
    }
}

public class WardingStance : StanceEffects
{
    public override void Init()
    {
        statusIcon = Resources.Load<Sprite>("Icons/Status Effects/StoneWall");
        maxTurnTimer = -1;
    }

    public override void Process(BattleCharacter character)
    {
        Debug.Log("Magic Defense Up");
    }
}

public class Poison : StatusEffect
{
    public override void Init()
    {
        statusIcon = Resources.Load<Sprite>("Icons/Status Effects/poison");
        maxTurnTimer = 6;
    }

    public override void Process(BattleCharacter character)
    {
        character.TakeDamage(10);
    }
}

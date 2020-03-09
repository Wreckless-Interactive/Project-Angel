using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter_Enemy : BattleCharacter
{

    [Space(5)]
    public Transform characterCardPivot;

    protected override void Die()
    {
        BattleManager.Instance.RemoveCharacter(this);
        Destroy(this.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAI : MonoBehaviour
{

    private BattleManager bm => BattleManager.Instance;

    public void DoTurn()
    {

        List<BattleCharacter> party = bm.GetPartyList();

        BattleCharacter character = party[Random.Range(0, party.Count)];

        bm.SetSelectedCharacter(character);
        StartCoroutine(bm.DealDamage());

    }

}

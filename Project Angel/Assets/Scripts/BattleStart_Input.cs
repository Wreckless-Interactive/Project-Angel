using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart_Input : MonoBehaviour
{
    public List<BattleCharacter> party = new List<BattleCharacter>();
    public List<BattleCharacter> enemies = new List<BattleCharacter>();

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            BattleManager.Instance.InitBattle(party, enemies);

    }
}

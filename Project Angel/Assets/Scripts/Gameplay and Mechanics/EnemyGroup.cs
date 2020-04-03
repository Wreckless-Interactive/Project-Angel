using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

    public List<WorldCharacter_Enemy> enemies = new List<WorldCharacter_Enemy>();

    private PlayerController Player => FindObjectOfType<PlayerController>();

    private bool enteredBattle = false;

    private List<BattleCharacter> enemyBChars = new List<BattleCharacter>();
    public List<BattleCharacter> party = new List<BattleCharacter>();

    public bool CanChase { get; private set; }

    private void Start()
    {
        foreach (WorldCharacter_Enemy enemy in enemies)
        {
            enemyBChars.Add(enemy.battleCharacter);
        }
    }

    private void Update()
    {

        if (enteredBattle)
            return;

        float distFromPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if(distFromPlayer <= 20f)
        {
            CanChase = true;
        }
        else if(distFromPlayer > 20f)
        {
            CanChase = true;
        }


    }

    public void AddEnemy(WorldCharacter_Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.InitCharacter(Player.transform, this);
    }

    public void StartBattle()
    {
        if (!enteredBattle)
        {
            CanChase = false;
            enteredBattle = true;
            BattleManager.Instance.InitBattle(party, enemyBChars);
        }
    }

}

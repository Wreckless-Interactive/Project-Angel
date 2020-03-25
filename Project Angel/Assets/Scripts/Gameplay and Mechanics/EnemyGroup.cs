using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

    public List<WorldCharacter_Enemy> enemies = new List<WorldCharacter_Enemy>();

    private PlayerController Player => FindObjectOfType<PlayerController>();

    private bool checkPlayerDist;
    private bool enteredBattle = false;

    private void Update()
    {

        if (enteredBattle)
            return;

        float distFromCenter = Vector3.Distance(Player.transform.position, transform.position);

        checkPlayerDist = distFromCenter < 10f;

        if (!checkPlayerDist)
            return;

        foreach(WorldCharacter_Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, Player.transform.position);

            if (dist <= 2)
            {
                enteredBattle = true;
            }
        }

    }

    public void AddEnemy(WorldCharacter_Enemy enemy)
    {
        enemies.Add(enemy);
    }

    private void OnDrawGizmos()
    {

        if (!checkPlayerDist)
            return;

        foreach (WorldCharacter_Enemy enemy in enemies)
        {
            Gizmos.DrawLine(enemy.transform.position, Player.transform.position);
        }

    }

}

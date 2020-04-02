using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    [Header("Enemies")]
    public List<Character> levelEnemies;
    public Vector3 halfExtentCheck;

    public void SpawnInEnemies(List<Transform> groupPoints)
    {

        foreach(Transform point in groupPoints)
        {

            int groupAmount = Random.Range(0, 4);

            for(int i = 0; i <= groupAmount; i++)
            {

                Vector3 randomPos = Vector3.zero;

                int c = 0;

                do
                {
                    randomPos = new Vector3(Random.Range(-5, 6), 0, Random.Range(-5, 6));
                    if (c == 20)
                    {
                        print("Infinite loop");
                        break;
                    }

                }
                while (Physics.CheckBox(point.position + randomPos, halfExtentCheck, Quaternion.identity));

                int enemyIndex = Random.Range(0, levelEnemies.Count);

                GameObject enemy = Instantiate(levelEnemies[enemyIndex].wCharacter.gameObject, Vector3.zero, Quaternion.identity, point);
                enemy.transform.localPosition = randomPos;

                point.GetComponent<EnemyGroup>().AddEnemy(enemy.GetComponent<WorldCharacter_Enemy>());

            }
        }

    }


    private void Awake()
    {
        Instance = this;
    }

}

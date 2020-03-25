using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public List<Transform> points;

    private void Start()
    {

        LevelManager.Instance.SpawnInEnemies(points);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy normalMonsterPrefab;

    private EnemyRoad enemyRoad;
    private Road startRoad;
    private Road endRoad;

    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        enemyRoad = GetComponent<EnemyRoad>();
        startRoad = enemyRoad.GetRoad(0);
        endRoad = enemyRoad.GetRoad(3);

        enemySpawner.SpawnEnemy(normalMonsterPrefab, startRoad);
    }
}

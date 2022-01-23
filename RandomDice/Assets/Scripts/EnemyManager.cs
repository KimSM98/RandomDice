using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type;

public class EnemyManager : MonoBehaviour
{
    public float spawnDelay = 1f;
    
    private EnemyRoad enemyRoad;
    private Road startRoad;
    private Road endRoad;

    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        initEnemyRoad();

        SpawnEnemy();
    }

    private void initEnemyRoad()
    {
        enemyRoad = GetComponent<EnemyRoad>();
        startRoad = enemyRoad.GetRoad(0);
        endRoad = enemyRoad.GetRoad(3);
    }

    private void SpawnEnemy()
    {
        int typeNum = Random.Range(0, 3);
        enemySpawner.SpawnEnemy((EMonsterType)typeNum, startRoad);
    }
}

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

        StartCoroutine(EnemySpawning());
    }

    #region Initialization
    private void initEnemyRoad()
    {
        enemyRoad = GetComponent<EnemyRoad>();
        startRoad = enemyRoad.GetRoad(0);
        endRoad = enemyRoad.GetRoad(3);
    } 
    #endregion

    IEnumerator EnemySpawning()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        int typeNum = Random.Range(0, 3);
        enemySpawner.SpawnEnemy((EMonsterType)typeNum, startRoad);
    }

}

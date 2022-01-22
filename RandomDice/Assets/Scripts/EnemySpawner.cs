using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy SpawnEnemy(Enemy enemyPrefab, Road startRoad)
    {
        Vector2 spawnPos = startRoad.transform.position;
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.SetCurrentRoad(startRoad);

        return enemy;
    }
}

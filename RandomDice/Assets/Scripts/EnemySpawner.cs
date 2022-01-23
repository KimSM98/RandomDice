using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public MonsterStatus baseStatus;
    public MonsterType[] monsterTypes;

    private List<Enemy> pendingEnemies;
    private List<Enemy> activeEnemies;

    private void Start()
    {
        
    }

    public Enemy SpawnEnemy(EMonsterType monsterType, Road startRoad)
    {
        Vector2 spawnPos = startRoad.transform.position;
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        enemy.Init(baseStatus, monsterTypes[(int)monsterType]);
        enemy.SetCurrentRoad(startRoad);

        return enemy;
    }
}

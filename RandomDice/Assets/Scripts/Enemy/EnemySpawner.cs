using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public EnemyStatus baseStatus;
    public MonsterType[] monsterTypes;

    private List<Enemy> pendingEnemies;
    private List<Enemy> activeEnemies;

    private void Start()
    {
        InitEnemyLists();
    }

    public Enemy SpawnEnemy(EMonsterType monsterType, Road startRoad)
    {
        Vector2 spawnPos = startRoad.transform.position;
        Enemy enemy;
        if(pendingEnemies.Count > 0)
        {
            enemy = GetEnemyFromPending();
        }
        else
        {
            enemy = IntstantiateEnemy();
        }
        activeEnemies.Add(enemy);

        enemy.Init(baseStatus, monsterTypes[(int)monsterType], spawnPos);
        enemy.SetCurrentRoad(startRoad);

        return enemy;
    }

    private Enemy GetEnemyFromPending()
    {
        int lastIdx = pendingEnemies.Count - 1;
        Enemy lastEnemyInPending = pendingEnemies[lastIdx];
        lastEnemyInPending.gameObject.SetActive(true);

        pendingEnemies.RemoveAt(lastIdx);

        return lastEnemyInPending;
    }
    
    private Enemy IntstantiateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.InitSpawner(this);
        return enemy;
    }

    #region Initialization
    private void InitEnemyLists()
    {
        pendingEnemies = new List<Enemy>();
        activeEnemies = new List<Enemy>();

        for (int i = 0; i < 10; i++)
        {
            Enemy enemy = IntstantiateEnemy();
            enemy.gameObject.SetActive(false);
            pendingEnemies.Add(enemy);
        }
    }
    #endregion

    public void MoveToPendingList(Enemy enemy)
    {
        pendingEnemies.Add(enemy);
        activeEnemies.Remove(enemy);

        GetComponent<EnemyManager>().RemoveFromEnemyList(enemy);
    }

}

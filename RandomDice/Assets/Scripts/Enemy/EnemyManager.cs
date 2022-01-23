using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type;

public class EnemyManager : MonoBehaviour
{
    public EnemyStatus baseStatus;
    public float spawnDelay = 1f;

    private EnemyRoad enemyRoad;
    private Road startRoad;
    private Road endRoad;

    private EnemySpawner enemySpawner;

    private List<Enemy> enemies;
    [SerializeField]
    private Enemy leadingEnemy;
    [SerializeField]
    private Enemy mostHPEnemy;

    private void Start()
    {
        enemies = new List<Enemy>();
        enemySpawner = GetComponent<EnemySpawner>();
        initEnemyRoad();

        StartCoroutine(EnemySpawning());
    }

    private void Update()
    {
        if (enemies.Count < 1) return;

        UpdateLeadingEnemy();
        UpdateMostHpEnemy();
    }

    #region Initialization
    private void initEnemyRoad()
    {
        enemyRoad = GetComponent<EnemyRoad>();
        startRoad = enemyRoad.GetRoad(0);
        endRoad = enemyRoad.GetRoad(3);
    }
    #endregion

    #region Setter/Getter
    public Enemy GetLeadingTarget()
    {
        return leadingEnemy;
    }

    public Enemy GetMostHPTarget()
    {
        return mostHPEnemy;
    } 

    public Enemy GetRandomTarget()
    {
        if (enemies.Count < 1) return null;

        int enemyNum = Random.Range(0, enemies.Count);
        return enemies[enemyNum];
    }
    #endregion

    #region UpdateTarget
    private void UpdateLeadingEnemy()
    {
        float leadingDist = 0f;
        foreach (Enemy e in enemies)
        {
            float eDist = e.GetCurrentDistanceTraveled();

            if (leadingDist > eDist) continue;

            leadingEnemy = e;
            leadingDist = eDist;
        }
    }

    private void UpdateMostHpEnemy()
    {
        float mostHP = 0f;
        foreach (Enemy e in enemies)
        {
            float eHp = e.GetHP();

            if (mostHP > eHp) continue;

            mostHPEnemy = e;
            mostHP = eHp;
        }
    } 
    #endregion

    public void RemoveFromEnemyList(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    #region SpawnEnemy
    IEnumerator EnemySpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int typeNum = Random.Range(0, 3);
        Enemy enemy = enemySpawner.SpawnEnemy((EMonsterType)typeNum, startRoad);

        // 추가할 때 가장 앞, HP 등 비교
        enemies.Add(enemy);

    } 
    #endregion

}

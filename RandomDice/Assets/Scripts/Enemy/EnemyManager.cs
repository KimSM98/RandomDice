using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type;

public class EnemyManager : MonoBehaviour
{
    public EnemyStatus baseStatus;
    public float spawnDelay = 1f;

    #region Components
    private EnemyRoad enemyRoad;
    private Road startRoad;
    private EnemySpawner enemySpawner;
    private int numOfMonsterType;
    [SerializeField]
    private PlayerStatus playerStatus;
    #endregion

    private List<Enemy> enemies;
    private Enemy leadingEnemy;
    private Enemy mostHPEnemy;

    [SerializeField]
    private bool isSpawning;

    private void Start()
    {
        isSpawning = false;

        enemies = new List<Enemy>();
        
        enemySpawner = GetComponent<EnemySpawner>();
        numOfMonsterType = enemySpawner.GetNumOfMonsterType();

        playerStatus = GetComponentInParent<PlayerStatus>();
        initEnemyRoad();

        GameManager.instance.AddEnemyManager(this);

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
    }
    #endregion

    #region Setter/Getter
    public void SetSpawnCondition(bool val)
    {
        isSpawning = val;
    }

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

    public void RemoveFromEnemies(Enemy enemy)
    {
        enemySpawner.MoveToPendingList(enemy);
        enemies.Remove(enemy);
    }

    #region SpawnEnemy
    public void StartEnemySpawning()
    {
        StartCoroutine(EnemySpawning());
    }

    public IEnumerator EnemySpawning()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int typeNum = Random.Range(0, numOfMonsterType);
        Enemy enemy = enemySpawner.SpawnEnemy((EMonsterType)typeNum, startRoad);
        enemy.SetEnemyManager(this);
        enemy.SetPlayerStatus(playerStatus);

        enemies.Add(enemy);
    } 
    #endregion

}

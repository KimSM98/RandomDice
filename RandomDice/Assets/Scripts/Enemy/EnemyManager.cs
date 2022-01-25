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
    private Road endRoad;
    private EnemySpawner enemySpawner;
    private PlayerStatus playerStatus;
    #endregion

    private List<Enemy> enemies;
    private Enemy leadingEnemy;
    private Enemy mostHPEnemy;

    [SerializeField]
    private bool spawnPause;


    private void Start()
    {
        spawnPause = false;

        enemies = new List<Enemy>();
        enemySpawner = GetComponent<EnemySpawner>();
        playerStatus = GetComponentInParent<PlayerStatus>();
        initEnemyRoad();

        GameManager.instance.AddEnemyManager(this);

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
    public void SetSpawnPause(bool val)
    {
        spawnPause = val;
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
        Debug.Log("EnemySpawn 시작");
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
        enemy.SetEnemyManager(this);
        enemy.SetPlayerStatus(playerStatus);

        enemies.Add(enemy);
    } 
    #endregion

}

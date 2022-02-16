using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyStatus enemyBaseStatus; // 향후 이것으로 모든 Enemy의 스탯 변경. GameManage로 가도 될 듯
    public MonsterType[] monsterTypes;

    [SerializeField]
    private float spawnDelay = 1f;
    private Road startRoad;
    private Vector2 spawnPos;
    private bool isSpawning = false;
    
    private PlayerStatus targetPlayerStatus;

    private void Start()
    {
        GameManager.instance.AddEnemySpawner(this);

        startRoad = GetComponent<EnemyRoad>().GetRoad(0);
        spawnPos = startRoad.transform.position;
        targetPlayerStatus = GetComponentInParent<PlayerStatus>();
    }

    #region Spawning
    public void SpawnEnemy()
    {
        GameObject enemyObj = ObjectPool.instance.GetObject("Enemy", spawnPos);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        InitEnemy(enemy);
    }

    private void InitEnemy(Enemy enemy)
    {
        enemy.Init(enemyBaseStatus, RandomMonsterType(), startRoad, targetPlayerStatus);
        GetComponent<EnemyManager>().InitEnemyByEnemyManager(enemy);
    }

    private MonsterType RandomMonsterType() 
    {
        int randomType = Random.Range(0, monsterTypes.Length);

        return monsterTypes[randomType];
    }
    
    public void StartEnemySpawning()
    {
        StartCoroutine(EnemySpawing());
    }

    IEnumerator EnemySpawing()
    {
        while(isSpawning)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy();
        }
    }
    #endregion

    #region Setter
    public void SetSpawnCondition(bool condition)
    {
        isSpawning = condition;
    }
    #endregion

}

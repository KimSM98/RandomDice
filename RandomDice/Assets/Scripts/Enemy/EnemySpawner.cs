using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation.Effects;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy spawing")]
    public EnemyStatus enemyBaseStatus; // 향후 이것으로 모든 Enemy의 스탯 변경. GameManage로 가도 될 듯
    public MonsterType[] monsterTypes;
    
    [Header("Boss spawing")]
    public EnemyStatus bossStatus;
    public MonsterType[] bossTypes;
    public GameObject gatheringPosObject;
    private Vector2 gatheringPos;
    private float totalHPOfEnemies = 0f;

    // Spawning
    private bool isSpawning = false;
    [SerializeField]
    private float spawnDelay = 1f;
    private Vector2 spawnPos;
    private Waypoint startWaypoint;
    private PlayerStatus targetPlayerStatus;

    private void Start()
    {
        GameManager.instance.AddEnemySpawner(this);

        startWaypoint = GetComponent<WaypointSystem>().GetWaypoint(0);
        targetPlayerStatus = GetComponentInParent<PlayerStatus>();
        
        spawnPos = startWaypoint.transform.position;
        gatheringPos = gatheringPosObject.transform.position;
    }

    #region Enemy Spawning
    public void StartEnemySpawning()
    {
        StartCoroutine(EnemySpawing());
    }

    IEnumerator EnemySpawing()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnDelay);
            if (isSpawning) // 중간에 isSpawning이 변하는 경우
                SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Enemy enemy = GetEnemyObjectFromPool(spawnPos);
        InitEnemyByType(enemy, enemyBaseStatus, monsterTypes);
        
        enemy.StartMoving();
    } 
    #endregion

    #region BossSpawning
    public void StartBossSpawning()
    {
        StartCoroutine(BossSpawing());
    }

    IEnumerator BossSpawing()
    {
        List<Enemy> enemies = GetComponent<EnemyTargeter>().GetActiveEnemies();

        totalHPOfEnemies = CalculateTotalHPOfEnemies(enemies);

        // Enemy gathering animation
        yield return StartCoroutine(GatheringEnemies(enemies));

        // 게임 상의 Enemy 비활성화
        DeactivateAllEnemiesInList(enemies);

        StartCoroutine(SpawnBoss());
    }

    IEnumerator GatheringEnemies(List<Enemy> enemyList)
    {
        List<GameObject> enemyObjList = new List<GameObject>();
        foreach (Enemy enemy in enemyList)
        {
            enemy.SetMoving(false);
            enemyObjList.Add(enemy.gameObject);
        }

        yield return StartCoroutine(AnimationEffect.GatheringObjects(enemyObjList, gatheringPos, 1.25f));
    }

    private float CalculateTotalHPOfEnemies(List<Enemy> enemyList)
    {
        float totalHP = 0f;

        foreach (Enemy enemy in enemyList)//이거 빼기
        {
            totalHP += enemy.GetHP();
        }

        return totalHP;
    }

    private IEnumerator SpawnBoss()
    {
        Enemy boss = GetEnemyObjectFromPool(gatheringPos);
        InitEnemyByType(boss, bossStatus, bossTypes);
        boss.AddHP(totalHPOfEnemies); // Additional HP of Boss
        boss.SetIsBossEnemy(true);
        boss.SetMoving(false);

        // 보스가 Gathering 위치에 생성된 것을 보여주기 위해 잠깐 멈춘다.
        yield return new WaitForSeconds(0.3f);

        yield return AnimationEffect.MoveTo(boss.transform, spawnPos, 1.5f);

        boss.StartMoving();

        // Entry to GameManager
        GameManager.instance.AddBoss(boss);
    } 
    #endregion

    

    #region Enemy Initialization
    private void InitEnemyByType(Enemy enemyToInit, EnemyStatus statusType, MonsterType[] Monstertypes)
    {
        enemyToInit.Init(statusType, RandomMonsterType(Monstertypes), startWaypoint, targetPlayerStatus);
        GetComponent<EnemyTargeter>().RegisterEnemyToList(enemyToInit);
    }

    private MonsterType RandomMonsterType(MonsterType[] types) 
    {
        int randomType = Random.Range(0, types.Length);

        return types[randomType];
    }
    #endregion

    #region Setter
    public void SetSpawnCondition(bool condition)
    {
        isSpawning = condition;
    }
    #endregion
    
    private Enemy GetEnemyObjectFromPool(Vector2 pos)
    {
        GameObject enemyObj = ObjectPool.instance.GetObject("Enemy", pos);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        return enemy;
    }

    private void DeactivateAllEnemiesInList(List<Enemy> enemyList)
    {
        int enemiesSize = enemyList.Count;
        for (int i = 0; i < enemiesSize; i++)
        {
            enemyList[0].DeactivateEnemy(); // List에서 제거되기 때문에 계속 0번에 접근한다.
        }
    }
}

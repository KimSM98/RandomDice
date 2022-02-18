using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isSpawning = false;
    [SerializeField]
    private float spawnDelay = 1f;
    private Vector2 spawnPos;

    private Road startRoad;
    
    private PlayerStatus targetPlayerStatus;

    float totalHPOfEnemies = 0f;

    private void Start()
    {
        GameManager.instance.AddEnemySpawner(this);

        startRoad = GetComponent<EnemyRoad>().GetRoad(0);
        targetPlayerStatus = GetComponentInParent<PlayerStatus>();

        spawnPos = startRoad.transform.position;
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
    } 
    #endregion

    #region BossSpawning
    public void StartBossSpawning()
    {
        StartCoroutine(BossSpawing());
    }

    IEnumerator BossSpawing()
    {
        StopDiceAttack();

        List<Enemy> enemies = GetComponent<EnemyManager>().GetEnemies();

        // Enemy gathering animation
        yield return StartCoroutine(GatheringEnemies(enemies));
        totalHPOfEnemies = CalculateTotalHPOfEnemies(enemies);

        // 게임 상의 Enemy 비활성화
        DeactivateAllEnemiesInList(enemies);

        yield return StartCoroutine(SpawnBoss());

        StartDiceAttack();
    }

    private IEnumerator SpawnBoss()
    {
        Enemy boss = GetEnemyObjectFromPool(gatheringPos);
        InitEnemyByType(boss, bossStatus, bossTypes);
        boss.AddHP(totalHPOfEnemies); // Additional HP of Boss
        boss.SetIsBossEnemy(true);
        boss.SetIsMove(false);

        // Entry to GameManager
        GameManager.instance.AddBoss(boss);

        // 잠깐 멈춤
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(LerpMovement(boss.gameObject, spawnPos, 1.5f));

        boss.SetIsMove(true);
    } 
    #endregion

    #region Movement
    IEnumerator LerpMovement(GameObject obj, Vector2 moveTo, float lerpDuration)
    {
        float t = 0f;

        while (t / lerpDuration < 1f)
        {
            t += Time.deltaTime;

            obj.transform.position = Vector2.Lerp(obj.transform.position, moveTo, t / lerpDuration);
            yield return null;
        }
    }
    IEnumerator GatheringEnemies(List<Enemy> enemyList)
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.SetIsMove(false);
        }

        float t = 0f;
        float lerpDuration = 1.25f;
        while (t / lerpDuration < 1f)
        {
            t += Time.deltaTime;

            foreach (Enemy enemy in enemyList)
            {
                enemy.transform.position = Vector3.Lerp(enemy.transform.position, gatheringPos, t / lerpDuration);
            }

            yield return null;
        }
    }
    #endregion

    #region Enemy Initialization
    private void InitEnemyByType(Enemy enemyToInit, EnemyStatus statusType, MonsterType[] Monstertypes)
    {
        enemyToInit.Init(statusType, RandomMonsterType(Monstertypes), startRoad, targetPlayerStatus);
        GetComponent<EnemyManager>().InitEnemyByEnemyManager(enemyToInit);
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

    #region Dice Attack Condition
    private void StartDiceAttack()
    {
        DiceManager diceManager = transform.parent.GetComponentInChildren<DiceManager>();
        diceManager.ActiveAttack(true);
    }

    private void StopDiceAttack()
    {
        DiceManager diceManager = transform.parent.GetComponentInChildren<DiceManager>();
        diceManager.ActiveAttack(false);
        diceManager.ResetAllDiceTarget();
    } 
    #endregion
    
    private Enemy GetEnemyObjectFromPool(Vector2 pos)
    {
        GameObject enemyObj = ObjectPool.instance.GetObject("Enemy", pos);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        return enemy;
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

    private void DeactivateAllEnemiesInList(List<Enemy> enemyList)
    {
        int enemiesSize = enemyList.Count;
        for (int i = 0; i < enemiesSize; i++)
        {
            enemyList[0].DeactivateEnemy(); // List에서 제거되기 때문에 계속 0번에 접근한다.
        }
    }

}

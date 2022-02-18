using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverUI;

    [SerializeField]
    private List<PlayerStatus> playerStatuses;
    [SerializeField]
    private List<EnemySpawner> enemySpawners;
    [SerializeField]
    private List<Enemy> bosses;

    [SerializeField]
    private float bossSpawnTime = 20f;
    [SerializeField]
    private float bossSpawnTimer = 0f;

    enum InGameState 
    { 
        MonsterWave,
        BossRaid
    }
    InGameState currentInGameState;

    #region Unity Methods
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;

        playerStatuses = new List<PlayerStatus>();
        enemySpawners = new List<EnemySpawner>();
        bosses = new List<Enemy>();
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }
    #endregion

    #region Game Loop Coroutines
    private IEnumerator GameLoop()
    {
        Debug.Log("Game Start");
        yield return StartCoroutine(GameReady());
        yield return StartCoroutine(GamePlaying());
    }

    private IEnumerator GameReady()
    {
        while (playerStatuses.Count < 1 && enemySpawners.Count < 1)
        {
            yield return null;
        }
        Debug.Log("All player setting completed");
    }

    private IEnumerator GamePlaying()
    {
        Debug.Log("Game Playing");

        currentInGameState = InGameState.MonsterWave;

        ActiveEnemySpawn();

        // 게임 실행중
        while (!OnePlayerLeft())
        {
            if (currentInGameState == InGameState.MonsterWave)
                bossSpawnTimer += Time.deltaTime;

            if (bossSpawnTimer > bossSpawnTime)
            {
                bossSpawnTimer = 0f;
                yield return StartCoroutine(BossRaid());
            }

            yield return null;
        }

        GameOver();
    }
    #endregion

    #region Boss Raid
    IEnumerator BossRaid()
    {
        Debug.Log("On boss raid situation");

        currentInGameState = InGameState.BossRaid;

        DeactivateEnemySpawn();
        SpawnBoss();

        yield return WaitBossSpawing();

        while (IsBossAlive())
        {
            yield return null;
        }

        // End of boss raid
        ActiveEnemySpawn();
        currentInGameState = InGameState.MonsterWave;
    }
    private void SpawnBoss()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.StartBossSpawning();
        }
    }

    IEnumerator WaitBossSpawing()
    {
        while (bosses.Count < playerStatuses.Count)
        {
            yield return null;
        }
    }

    private bool IsBossAlive()
    {
        if (bosses.Count > 0) return true;

        return false;
    } 
    #endregion

    #region Enemy Spawn Activation
    private void ActiveEnemySpawn()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SetSpawnCondition(true);
            spawner.StartEnemySpawning();
        }
    }

    private void DeactivateEnemySpawn()
    {
        Debug.Log("Enemy spawn 중지");
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SetSpawnCondition(false);
        }
    } 
    #endregion

    #region Game Conditions
    private void GameOver()
    {
        gameOverUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private bool OnePlayerLeft()
    {
        bool isAnyoneDead = false;
        foreach (PlayerStatus status in playerStatuses)
        {
            isAnyoneDead = status.isPlayerDead();
        }

        return isAnyoneDead;
    }
    #endregion

    #region List Method
    public void AddPlayerStatus(PlayerStatus status)
    {
        playerStatuses.Add(status);
    }

    public void AddEnemySpawner(EnemySpawner spawner)
    {
        enemySpawners.Add(spawner);
    }

    public void AddBoss(Enemy boss)
    {
        bosses.Add(boss);
    }

    public void RemoveBoss(Enemy boss)
    {
        bosses.Remove(boss);
    }
    #endregion

}

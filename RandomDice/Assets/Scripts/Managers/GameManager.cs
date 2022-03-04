using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private int numOfPlayers = 1;
    public GameObject gameOverUI;

    [SerializeField]
    private List<PlayerStatus> playerStatuses;
    private List<EnemySpawner> enemySpawners;
    [SerializeField]
    private List<DiceManager> diceManagers;

    [SerializeField]
    private List<Enemy> bosses;

    #region Boss Spawn Timer
    [Header("Boss Spawn Timer")]
    public TimerDisplay timerDisplay;
    [SerializeField]
    private int bossSpawnTime = 20;
    private TMPController timerTMPController; 
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;

        InitLists();
    }


    private void Start()
    {
        StartCoroutine(GameLoop());

        timerDisplay.SetTimeToCount((int)bossSpawnTime);
    }

    #endregion

    #region Game Loop Coroutines
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameReady());
        yield return StartCoroutine(GamePlaying());

        GameOver();
    }

    private IEnumerator GameReady()
    {
        while (playerStatuses.Count < numOfPlayers && enemySpawners.Count < numOfPlayers)
        {
            yield return null;
        }
    }

    private IEnumerator GamePlaying()
    {
        ActiveEnemySpawn();
        
        StartCoroutine(BossRaidLoop());

        while (!IsPlayerAlive())
        {
            yield return null;
        }
    }

    #endregion

    #region Boss Raid Loop
    IEnumerator BossRaidLoop()
    {
        timerTMPController = timerDisplay.GetComponent<TMPController>();
        string bossAppearStr = "보스 출현";

        while (!IsPlayerAlive())
        {
            // 보스 출현 시간 카운트
            yield return StartCoroutine(timerDisplay.RunTimer());
            timerTMPController.SetText(bossAppearStr);
            // 보스 출현
            yield return StartCoroutine(BossRaid());
            // 보스 출현 시간 리셋
            timerDisplay.ResetTimer();
        }
    }

    IEnumerator BossRaid()
    {
        StopDiceAttack();
        DeactivateEnemySpawn();

        // 이미 발사된 총알이 남아 있는 경우를 방지하기 위해 텀을 주고 보스를 생성한다.
        // 보스를 생성하는 과정에도 총알이 타겟을 쫓아가는 경우를 방지하기 위함이다.
        yield return new WaitForSeconds(0.5f);

        SpawnBoss();
        // 보스가 완전히 생성될 때까지 기다린다.
        yield return WaitBossSpawing(); 
        
        StartDiceAttack();

        while (IsBossAlive())
        {
            yield return null;
        }

        // End of boss raid
        ActiveEnemySpawn();
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
        // 플레이어 수만큼 보스가 생성될 때까지 기다린다.
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

    #region Enemy Spawn Control
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsPlayerAlive()
    {
        bool isAnyoneDead = false;
        foreach (PlayerStatus status in playerStatuses)
        {
            isAnyoneDead = status.isPlayerDead();
            if (isAnyoneDead) break;
        }

        return isAnyoneDead;
    }
    #endregion

    #region List Method
    private void InitLists()
    {
        playerStatuses = new List<PlayerStatus>();
        enemySpawners = new List<EnemySpawner>();
        diceManagers = new List<DiceManager>();
        bosses = new List<Enemy>();
    }

    public void AddPlayerStatus(PlayerStatus status)
    {
        playerStatuses.Add(status);
    }

    public void AddEnemySpawner(EnemySpawner spawner)
    {
        enemySpawners.Add(spawner);
    }

    public void AddDiceManager(DiceManager manager)
    {
        diceManagers.Add(manager);
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

    #region Dice Attack Control
    private void StartDiceAttack()
    {
        foreach (DiceManager manager in diceManagers)
        {
            manager.ActiveAttack(true);
        }
    }

    private void StopDiceAttack()
    {
        foreach (DiceManager manager in diceManagers)
        {
            manager.ActiveAttack(false);
            manager.ResetAllDiceTarget();
        }
    }
    #endregion

}

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
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }
    #endregion

    private IEnumerator GameLoop()
    {
        Debug.Log("Game Start");
        yield return StartCoroutine(GameReady());
        yield return StartCoroutine(GamePlaying());
    }

    private IEnumerator GameReady()
    {
        while(playerStatuses.Count < 1 && enemySpawners.Count < 1)
        {
            yield return null;
        }
        Debug.Log("All player setting completed");
    }

    private IEnumerator GamePlaying()
    {
        Debug.Log("Game Playing");
        ActiveEnemySpawn();

        // 게임 실행중
        while (!OnePlayerLeft())
        {
            yield return null;
        }

        GameOver();
    }

    private void ActiveEnemySpawn()
    {
        foreach(EnemySpawner spawner in enemySpawners)
        {
            spawner.SetSpawnCondition(true);
            spawner.StartEnemySpawning();
        }        
    }

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

    #region Add Components
    public void AddPlayerStatus(PlayerStatus status)
    {
        playerStatuses.Add(status);
    }

    public void AddEnemySpawner(EnemySpawner spawner)
    {
        enemySpawners.Add(spawner);
    }
    #endregion

}

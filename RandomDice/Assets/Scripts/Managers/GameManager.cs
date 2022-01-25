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
    private List<EnemyManager> enemyManagers;

    private BulletManager bulletManager;

    private void Awake()
    {
        Time.timeScale = 1f;

        instance = this;

        bulletManager = GetComponent<BulletManager>();

        playerStatuses = new List<PlayerStatus>();
        enemyManagers = new List<EnemyManager>();
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        Debug.Log("Game Start");
        yield return StartCoroutine(GamePlaying());
    }

    private IEnumerator GamePlaying()
    {
        Debug.Log("Game Playing");
        //foreach (EnemyManager manager in enemyManagers)
        //{
        //    manager.SetSpawnPause(false);
        //    //StartCoroutine(manager.EnemySpawning());
        //}

        // 게임 실행중
        while (!OnePlayerLeft())
        {
            yield return null;
        }

        GameOver();
    }

    private void GameOver()
    {
        gameOverUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
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

    public BulletManager GetBulletManager()
    {
        return bulletManager;
    }

    #region Add Components
    public void AddPlayerStatus(PlayerStatus status)
    {
        playerStatuses.Add(status);
    }

    public void AddEnemyManager(EnemyManager manager)
    {
        enemyManagers.Add(manager);
    }
    #endregion

    public void RestartGame()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public List<PlayerStatus> playerStatuses;
    //public List<EnemyManager> enemyManagers;

    private BulletManager bulletManager;

    private void Awake()
    {
        instance = this;

        bulletManager = GetComponent<BulletManager>();
    }

    //private void Start()
    //{
    //    Debug.Log("GameManager");
    //    //StartCoroutine(GameLoop());
    //}

    //private IEnumerator GameLoop()
    //{
    //    Debug.Log("Game Start");
    //    yield return StartCoroutine(GamePlaying());
    //}

    //private IEnumerator GamePlaying()
    //{
    //    foreach(EnemyManager manager in enemyManagers)
    //    {
    //        manager.SetSpawnPause(true);
    //    }

    //    while(OnePlayerLeft())
    //    {
    //        Debug.Log("GameOver");
    //        yield return null;
    //    }

    //}

    //private bool OnePlayerLeft()
    //{
    //    bool isAnyoneDead = false;
    //    foreach(PlayerStatus status in playerStatuses)
    //    {
    //        isAnyoneDead = status.isPlayerDead();
    //    }

    //    return isAnyoneDead;
    //}

    public BulletManager GetBulletManager()
    {
        return bulletManager;
    }

    //#region Add Components
    //public void AddPlayerStatus(PlayerStatus status)
    //{
    //    playerStatuses.Add(status);
    //}

    //public void AddEnemyManager(EnemyManager manager)
    //{
    //    enemyManagers.Add(manager);
    //} 
    //#endregion
}

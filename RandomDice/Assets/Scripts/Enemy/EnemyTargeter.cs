using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{    
    private List<Enemy> activeEnemies;
    private Enemy leadingEnemy;
    private Enemy mostHPEnemy;

    private void Start()
    {
        activeEnemies = new List<Enemy>();
    }

    private void Update()
    {
        if (activeEnemies.Count < 1) return;

        UpdateLeadingEnemy();
        UpdateMostHpEnemy();
    }

    #region Setter/Getter

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
        if (activeEnemies.Count < 1) return null;

        int enemyNum = Random.Range(0, activeEnemies.Count);
        return activeEnemies[enemyNum];
    }

    public List<Enemy> GetEnemies()
    {
        return activeEnemies;
    }
    #endregion

    #region UpdateTarget
    private void UpdateLeadingEnemy()
    {
        float leadingDist = 0f;
        foreach (Enemy e in activeEnemies)
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
        foreach (Enemy e in activeEnemies)
        {
            float eHP = e.GetHP();

            if (mostHP > eHP) continue;

            mostHPEnemy = e;
            mostHP = eHP;
        }
    } 
    #endregion

    public void RemoveFromList(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }
    public void AddEnemyToList(Enemy enemySpawned)
    {
        activeEnemies.Add(enemySpawned);
    }
    
    // Enemy가 마지막 Road에 도달하거나 죽었을 때 EnemyManager의 리스트에서 빼기 위한 것이다.
    public void RegisterEnemyToList(Enemy enemy)
    {
        enemy.SetRegisteredList(this);
        AddEnemyToList(enemy);
    }
}

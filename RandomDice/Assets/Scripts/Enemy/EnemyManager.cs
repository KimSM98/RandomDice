using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{    
    // Enemy Targeting
    private List<Enemy> enemies;
    private Enemy leadingEnemy;
    private Enemy mostHPEnemy;

    private void Start()
    {
        enemies = new List<Enemy>();
    }

    private void Update()
    {
        if (enemies.Count < 1) return;

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
            float eHP = e.GetHP();

            if (mostHP > eHP) continue;

            mostHPEnemy = e;
            mostHP = eHP;
        }
    } 
    #endregion

    public void RemoveFromList(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
    public void AddEnemyToList(Enemy enemySpawned)
    {
        enemies.Add(enemySpawned);
    }
    
    public void InitEnemyByEnemyManager(Enemy enemy)
    {
        enemy.SetEnemyManager(this);
        AddEnemyToList(enemy);
    }
}

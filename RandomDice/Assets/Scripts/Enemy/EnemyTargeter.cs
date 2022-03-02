using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{    
    private List<Enemy> activeEnemies;
    [SerializeField]
    private Enemy leadingEnemy;
    [SerializeField]
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

    #region Get Enemy Target
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
    #endregion

    #region UpdateTarget
    private void UpdateLeadingEnemy()
    {
        float leadingDist = 0f;
        foreach (Enemy e in activeEnemies)
        {
            float eDist = e.GetDistanceTraveled();

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

    #region List Methods
    public List<Enemy> GetActiveEnemies()
    {
        return activeEnemies;
    }
    public void RemoveFromList(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }
    public void AddEnemyToList(Enemy enemySpawned)
    {
        activeEnemies.Add(enemySpawned);
    }

    public void RegisterEnemyToList(Enemy enemy)
    {
        // Enemy의 역할이 끝났을 때 리스트에서 제거하기 위한 것이다.
        enemy.SetRegisteredList(this); 
        AddEnemyToList(enemy);
    } 
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type;

public class EnemySpawner : MonoBehaviour
{
    public EnemyStatus baseStatus;
    public MonsterType[] monsterTypes;

    public Enemy SpawnEnemy(EMonsterType monsterType, Road startRoad)
    {
        Vector2 spawnPos = startRoad.transform.position;

        GameObject enemyObj = ObjectPool.instance.GetObject("Enemy", spawnPos);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemy.Init(baseStatus, monsterTypes[(int)monsterType], spawnPos);
        enemy.SetCurrentRoad(startRoad);

        return enemy;
    }

    public int GetNumOfMonsterType()
    {
        return monsterTypes.Length;
    }
}

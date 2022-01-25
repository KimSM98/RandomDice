using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    #region Components
    private DiceSpawner diceSpawner;
    private DiceSpawnPosition spawnPos;
    private EnemyManager enemyManager;
    private PlayerStatus playerStatus;
    #endregion

    private List<Dice> activeDices;

    private void Start()
    {
        activeDices = new List<Dice>();
        diceSpawner = GetComponent<DiceSpawner>();
        spawnPos = diceSpawner.diceSpawnPos;

        enemyManager = transform.parent.GetComponentInChildren<EnemyManager>();
        playerStatus = GetComponentInParent<PlayerStatus>();

    }

    private void Update()
    {
        if (activeDices.Count < 1) return;

        UpdateAttackTarget();
    }

    public void SpawnDice()
    {
        if (!playerStatus.CanSpawnDice()) return;
        if(activeDices.Count >= 15)
        {
            Debug.Log("소환 가능한 다이스의 최대 개수에 도달했습니다.");
            return;
        }

        Dice dice = diceSpawner.SpawnDice();
        activeDices.Add(dice);

        playerStatus.SpawnDice();
    }

    public List<Dice> GetDiceList()
    {
        return activeDices;
    }

    public void RemoveActiveDice(Dice dice)
    {
        spawnPos.MoveToEmptyBoard(dice.GetBoardInfo());

        activeDices.Remove(dice);
    }

    private void UpdateAttackTarget()
    {
        foreach(Dice dice in activeDices)
        {
            DiceType.TargetType targetType = dice.GetDiceTargetType();

            Enemy target = null;
            switch (targetType)
            {
                case DiceType.TargetType.InFront:
                    target = enemyManager.GetLeadingTarget();
                    break;

                case DiceType.TargetType.HPDescendingOrder:
                    target = enemyManager.GetMostHPTarget();
                    break;

                case DiceType.TargetType.Random:
                    target = enemyManager.GetRandomTarget();
                    break;
            }

            if (target == null) return;

            dice.SetTarget(target);
        }
    }
}

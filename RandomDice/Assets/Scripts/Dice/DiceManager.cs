using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<Dice> activeDices;

    private DiceSpawner diceSpawner;
    private DiceSpawnPosition spawnPos;

    private void Start()
    {
        diceSpawner = GetComponent<DiceSpawner>();
        spawnPos = diceSpawner.spawnPos;
    }

    public void SpawnDice()
    {
        if(activeDices.Count >= 15)
        {
            Debug.Log("소환 가능한 다이스의 최대 개수에 도달했습니다.");
            return;
        }

        Dice dice = diceSpawner.SpawnDice();
        activeDices.Add(dice);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public Dice dicePrefab;
    public DiceType[] diceTypes;
    public DiceSpawnPosition diceSpawnPos;

    public Dice SpawnDice()
    {
        BoardInfo boardInfo = diceSpawnPos.GetRandomBoard();
        Dice dice = Instantiate(dicePrefab, boardInfo.boardPos, Quaternion.identity);

        dice.SetBoardInfo(boardInfo);

        // Random types
        DiceType diceType = GetRandDiceType();
        dice.InitDice(diceType);

        return dice;
    }

    public DiceType GetRandDiceType()
    {
        int randNum = Random.Range(0, diceTypes.Length);
        return diceTypes[randNum];
    }

}

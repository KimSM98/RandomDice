using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public Dice dicePrefab;
    public DiceType[] diceTypes;
    public SpawnPosition spawnPos;

    public Dice SpawnDice()
    {
        BoardInfo boardInfo = spawnPos.GetRandomBoard();
        Dice dice = Instantiate(dicePrefab, boardInfo.boardPos, Quaternion.identity);

        dice.SetBoardInfo(boardInfo);
        
        // Random types
        int randNum = Random.Range(0, diceTypes.Length);
        dice.InitDice(diceTypes[randNum]);

        return dice;
    }

}

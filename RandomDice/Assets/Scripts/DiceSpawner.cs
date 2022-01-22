using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public Dice dicePrefab;
    public DiceType[] diceTypes;
    public SpawnPosition spawnPos;

    private int numOfDice;

    private void Start()
    {
        numOfDice = 0;
    }

    public Dice SpawnDice()
    {
        // Random Position
        Vector2 randPos = spawnPos.GetRandomEmptyPos();
        
        Dice dice = Instantiate(dicePrefab, randPos, Quaternion.identity);

        // Random types
        int randNum = Random.Range(0, diceTypes.Length);
        dice.InitDice(diceTypes[randNum]);

        numOfDice++;

        return dice;
    }

}

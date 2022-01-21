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

    public void SpawnDice()
    {
        if(numOfDice >= 15)
        {
            Debug.Log("소환 가능한 다이스의 최대 개수에 도달했습니다.");
            return;
        }
        
        // Random Position
        Vector2 randPos = spawnPos.GetRandomEmptyPos();
        
        Dice dice = Instantiate(dicePrefab, randPos, Quaternion.identity);

        // Random types
        int randNum = Random.Range(0, diceTypes.Length);
        dice.InitDice(diceTypes[randNum]);

        numOfDice++;
    }

}

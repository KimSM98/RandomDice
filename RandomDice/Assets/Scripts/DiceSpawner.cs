using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public Dice dicePrefab;
    public DiceType[] diceTypes;

    public void SpawnDice()
    {
        Dice dice = Instantiate(dicePrefab);
        dice.InitDice(diceTypes[0]);
    }

}

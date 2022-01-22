using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<Dice> activeDice;

    private DiceSpawner diceSpawner;

    private void Start()
    {
        diceSpawner = GetComponent<DiceSpawner>();
    }

    public void SpawnDice()
    {
        if(activeDice.Count >= 15)
        {
            Debug.Log("소환 가능한 다이스의 최대 개수에 도달했습니다.");
            return;
        }

        Dice dice = diceSpawner.SpawnDice();
        activeDice.Add(dice);
    }
}

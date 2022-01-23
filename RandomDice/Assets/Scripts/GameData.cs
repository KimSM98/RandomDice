using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField]
    private int sp;
    [SerializeField]
    private int hp;
    
    public void AddSp(int val)
    {
        sp += val;
        Debug.Log("SP " + sp);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        // IF hp <= 0
        Debug.Log("HP " + hp);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    protected int sp = 100;
    [SerializeField]
    protected int hp = 3;

    public virtual void AddSp(int val)
    {
        sp += val;
        Debug.Log("SP " + sp);
    }

    public virtual void TakeDamage(int damage)
    {
        hp = Mathf.Max(0, hp -= damage);
        // IF hp <= 0
    }
}

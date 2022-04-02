using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    protected int sp = 100;
    [SerializeField]
    protected int spawnDiceSP = 10;
    [SerializeField]
    protected int life = 3;

    private bool isDead = false;

    protected virtual void Start()
    {
        GameManager.instance.AddPlayerStatus(this);
    }

    public virtual void AddSp(int val)
    {
        sp += val;
    }

    public virtual void TakeDamage(int damage)
    {
        life = Mathf.Max(0, life -= damage);

        if (life == 0) isDead = true;
    }

    public virtual void SpawnDice()
    {
        sp -= spawnDiceSP;
        spawnDiceSP += 10;
    }

    public bool CanSpawnDice()
    {
        if (sp - spawnDiceSP >= 0) return true;

        return false;
    }

    public bool isPlayerDead()
    {
        return isDead;
    }
}

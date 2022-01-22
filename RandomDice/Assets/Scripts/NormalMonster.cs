using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Enemy
{
    private void Start()
    {
        InitStatus();

        InitCanvas();
        InitHpText();
    }

    private void InitStatus()
    {
        hp = normalMonsterStatus.hp;
        moveSpeed = normalMonsterStatus.moveSpeed;
        attackPower = normalMonsterStatus.attackPower;
    }
}

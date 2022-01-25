using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStatus : PlayerStatus
{
    UserUIManager userUI;

    protected override void Start()
    {
        base.Start();
        userUI = GetComponent<UserUIManager>();

        userUI.Inin(sp, spawnDiceSP);
    }

    public override void AddSp(int val)
    {
        base.AddSp(val);
        userUI.UpdateCurrentSPText(sp);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        for(int i = 0; i < damage; i++)
            userUI.OffLifeUI();
    }

    public override void SpawnDice()
    {
        base.SpawnDice();
        userUI.UpdateCurrentSPText(sp);
        userUI.UpdateSpawnDiceSP(spawnDiceSP);
    }
}

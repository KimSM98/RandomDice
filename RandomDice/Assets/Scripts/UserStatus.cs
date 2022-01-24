using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStatus : PlayerStatus
{
    UserUIManager userUI;

    private void Start()
    {
        userUI = GetComponent<UserUIManager>();

        userUI.Inin(sp);
    }

    public override void AddSp(int val)
    {
        base.AddSp(val);
        userUI.SetSPText(sp);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        userUI.OffLifeUI(hp);
    }
}

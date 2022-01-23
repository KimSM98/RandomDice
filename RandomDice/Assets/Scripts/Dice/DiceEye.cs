using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullet 생성

public class DiceEye : MonoBehaviour
{
    private Enemy target;
    private DiceType type;

    private BulletManager bulletManager;
    private void Start()
    {
        bulletManager = GameManager.instance.GetBulletManager();

        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (target == null) return;
        if(target.IsDead())
        {
            target = null;
        }
    }

    public void Init(DiceType _type)
    {
        target = null;

        type = _type;
        SetColor(type.diceEyeColor);
    }

    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    private void CreateBullet()
    {
        Bullet bullet = bulletManager.CreateBullet(transform.position, target, type);
        bullet.SetShootingDiceEye(this);
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            if(target != null)
                CreateBullet();

            yield return new WaitForSeconds(type.attackSpeed);
        }
    }
}

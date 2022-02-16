using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEye : MonoBehaviour
{
    [SerializeField]
    private Enemy target;
    private DiceType type;

    #region Components
    private SpriteRenderer spriteRenderer;
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
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
        spriteRenderer.color = color;
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    private void SpawnBullet()
    {
        GameObject bulletObj = ObjectPool.instance.GetObject("Bullet", transform.position);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(target, type);
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            if(target != null)
                SpawnBullet();

            yield return new WaitForSeconds(type.attackSpeed);
        }
    }
}

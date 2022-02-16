using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy target;
    private DiceType type;
    [SerializeField]
    private float moveSpeed = 1f;

    private SpriteRenderer sprRenderer;
    private BulletAnimation bulletAnimation;

    private bool isShooting = false;
    private Vector2 startPos;
    private float t;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        bulletAnimation = GetComponent<BulletAnimation>();
    }

    private void Update()
    {
        if (isShooting)
        {
            Move();
        }
    }

    public void Init(Enemy _target, DiceType _type)
    {
        isShooting = true;
        startPos = transform.position;
        t = 0f;

        target = _target;

        type = _type;
        sprRenderer.color = type.diceEyeColor;
    }

    private void Move()
    {
        float dist = Vector2.Distance(startPos, target.transform.position);
        float ratioOfDist = moveSpeed / dist;
        t += ratioOfDist * Time.deltaTime;

        Vector2 destination = target.transform.position;
        transform.position = Vector2.Lerp(startPos, destination, t);

        
        if (t >= 1f || target.IsDead())
        {
            BulletExplosion();
        }
    }

    #region Explosion
    private void BulletExplosion()
    {
        isShooting = false;
        
        if (!target.IsDead())
            target.TakeDamage(type.attackPower);

        PlayExplosionAnim();
    }

    private void PlayExplosionAnim()
    {
        StartCoroutine(ExplosionAnim());
    }

    private IEnumerator ExplosionAnim()
    {
        //GameObject damageObj = ObjectPool.instance.GetObject("DamageEffect", transform.position);
        //DamageEffect damageEffect = damageObj.GetComponent<DamageEffect>();
        //damageEffect.DisplayDamage(type.attackPower);

        yield return StartCoroutine(bulletAnimation.PlayExplosionAnim());
        
        ResetBullet();
    }
    #endregion


    private void ResetBullet()
    {
        gameObject.SetActive(false);
        bulletAnimation.InitSprite();
        ObjectPool.instance.ReturnToPool("Bullet", this.gameObject);
    }

}

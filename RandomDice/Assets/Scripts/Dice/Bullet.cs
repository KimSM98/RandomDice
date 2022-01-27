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
    private Sprite defaultSprite;

    private bool isShooting = false;
    private Vector2 initPos;
    private float t;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = sprRenderer.sprite;
        bulletAnimation = GetComponent<BulletAnimation>();
    }

    private void Update()
    {
        if (isShooting)
        {
            Move();
        }
    }

    public void Init(Vector2 pos, Enemy _target, DiceType _type)
    {
        isShooting = true;
        transform.position = pos;
        initPos = pos;
        t = 0f;

        target = _target;

        type = _type;
        sprRenderer.color = type.diceEyeColor;
    }

    private void Move()
    {
        float dist = Vector2.Distance(initPos, target.transform.position);
        float ratioOfDist = moveSpeed / dist;
        t += ratioOfDist * Time.deltaTime;

        Vector2 destination = target.transform.position;
        transform.position = Vector2.Lerp(initPos, destination, t);

        
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
        yield return StartCoroutine(bulletAnimation.PlayExplosionAnim());
        ResetBullet();
    } 
    #endregion

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        sprRenderer.sprite = defaultSprite;
        GameManager.instance.GetBulletManager().MoveToPendingList(this);
    }

}

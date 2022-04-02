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
    private AnimationPlayer animPlayer;
    private LerpMovement lerpMovement;

    private bool isShooting = false;

    // 외부에서 Init하기 때문에 Awake를 사용한다.
    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        animPlayer = GetComponent<AnimationPlayer>();

        lerpMovement = GetComponent<LerpMovement>();
    }
    public void Init(Enemy _target, DiceType _type)
    {
        isShooting = true;

        target = _target;

        type = _type;
        sprRenderer.color = type.diceEyeColor;

        StartMoving();
    }

    #region Movement
    private void StartMoving()
    {
        StartCoroutine(BulletExplosionChecker());
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        lerpMovement.SetMoving(true);

        yield return StartCoroutine(lerpMovement.MoveLerp(target.transform, 1f, moveSpeed));

        isShooting = false;
    } 
    #endregion

    #region Explosion
    IEnumerator BulletExplosionChecker()
    {
        while(isShooting && !target.IsDead())
        {
            yield return null;
        }

        lerpMovement.SetMoving(false);
        BulletExplosion();
    }
    
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
        yield return StartCoroutine(animPlayer.PlayAnimation());
        
        ResetBullet();
    }
    #endregion

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        animPlayer.InitSprite();
        ObjectPool.instance.ReturnToPool("Bullet", this.gameObject);
    }

}

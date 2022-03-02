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
        lerpMovement.SetMovementType(LerpMovement.MovementType.UniformMotion);
        //lerpMovement.UseRatioOfDistanceToSpeedWeight();
    }


    public void Init(Enemy _target, DiceType _type)
    {
        isShooting = true;

        target = _target;

        type = _type;
        sprRenderer.color = type.diceEyeColor;

        StartMoving();
    }

    private void StartMoving()
    {
        //float dist = Vector2.Distance(startPos, target.transform.position);
        //float ratioOfDist = moveSpeed / dist;
        //t += ratioOfDist * Time.deltaTime;

        //Vector2 destination = target.transform.position;
        //transform.position = Vector2.Lerp(startPos, destination, t);

        //if (t >= 1f || target.IsDead())
        //{
        //    BulletExplosion();
        //}
        StartCoroutine(BulletExplosionChecker());
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        lerpMovement.SetMoving(true);

        //yield return StartCoroutine(lerpMovement.UniformMotion(transform.position, target.transform.position, 1f, moveSpeed));
        yield return StartCoroutine(lerpMovement.MoveLerp(target.transform, 1f, moveSpeed));

        isShooting = false;
    }


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

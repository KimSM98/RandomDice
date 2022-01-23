using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private DiceEye shootingDiceEye;
    [SerializeField]
    private Enemy target;
    private DiceType type;
    [SerializeField]
    private float moveSpeed = 1f;

    private SpriteRenderer sprRenderer;
    private BulletAnimation bulletAnimation;

    private bool isShooting = false;
    private Vector2 initPos;
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

    public void Init(Vector2 pos, Enemy _target, DiceType _type)
    {
        isShooting = true;
        initPos = pos;
        t = 0f;

        target = _target;

        type = _type;
        sprRenderer.color = type.diceEyeColor;
    }

    public void SetShootingDiceEye(DiceEye startFrom)
    {
        shootingDiceEye = startFrom;
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
            t = 0f;
            //StartCoroutine(PlayExplosionAnim());
            //PlayExplosionAnim();

            ResetBullet();
            if (!target.IsDead())
                target.TakeDamage(type.attackPower);
        }
    }

    void PlayExplosionAnim()
    {
        //Debug.Log("코루틴 시작");
        StartCoroutine(bulletAnimation.PlayExplosionAnim());
        //ResetBullet();
    }
    //private IEnumerator PlayExplosionAnim()
    //{
    //    yield return StartCoroutine(bulletAnimation.PlayExplosionAnim());
    //    Debug.Log("Play끝");
    //    //ResetBullet();
    //}

    public void ResetBullet()
    {
        GameManager.instance.GetBulletManager().MoveToPendingList(this);
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Text hpText;

    #region Status with MonsterType
    private float hp;
    private float moveSpeed;
    [SerializeField]
    private int attackPower; // Player HP에 영향 
    
    #endregion
    
    #region Components
    private EnemyTargeter currentlyRegisteredList;
    private PlayerStatus playerStatus;

    private WaypointFollower waypointFollower;
    private MovingDistanceTracker distanceTracker;
    private SpriteRenderer spriteRenderer;
    #endregion
    
    private bool isDead;

    [SerializeField]
    private Vector2 damageEffectOffset;

    // Boss Field
    [SerializeField]
    private bool isBossEnemy = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        distanceTracker = GetComponent<MovingDistanceTracker>();
        waypointFollower = GetComponent<WaypointFollower>();
    }

    #region Initialization
    public void Init(EnemyStatus status, MonsterType type, Waypoint startWaypoint, PlayerStatus targetPlayer)
    {
        hp = 0f;
        isDead = false;

        // Stats
        InitBaseStatus(status);
        InitStatusByType(type);
        InitSpriteByType(type.sprite);

        // Movement
        SetCurrentWaypoint(startWaypoint);
        SetTargetPlayerStatus(targetPlayer);

        UpdateHPText();
    }

    private void InitBaseStatus(EnemyStatus status)
    {
        hp = status.hp;
        moveSpeed = status.moveSpeed;
        attackPower = status.attackPower;
    }

    // Monster Type마다 스탯 가중치를 준다.
    private void InitStatusByType(MonsterType type)
    {
        hp *= type.HpMult;
        moveSpeed *= type.moveSpeedMult;
        attackPower *= type.attackPowerMult;
    }

    private void InitSpriteByType(Sprite spr)
    {
        spriteRenderer.sprite = spr;
    }
    #endregion
    
    public void AddHP(float hpAmount)
    {
        hp += hpAmount;
        UpdateHPText();
    }

    public void UpdateHPText()
    {
        hpText.text = hp.ToString();
    }

    #region Setter/Getter
    public void SetMoving(bool movingCondition)
    {
        if (waypointFollower != null)
            waypointFollower.SetMoving(movingCondition);

        distanceTracker.SetRunningCondition(false);
    }

    // Enemy가 비활성화되면 EnemyTargeter 리스트에서 지우기 위해서 설정
    // Enemy가 등록되어 있는 리스트
    public void SetRegisteredList(EnemyTargeter currentTargeter)
    {
        if (currentlyRegisteredList != null) return;
        currentlyRegisteredList = currentTargeter;
    }

    public void SetTargetPlayerStatus(PlayerStatus status)
    {
        if (playerStatus != null) return;
        playerStatus = status;
    }

    private void SetCurrentWaypoint(Waypoint wp)
    {
        waypointFollower.SetCurrentPoint(wp);
    }

    public void SetIsBossEnemy(bool val)
    {
        isBossEnemy = val;
    }

    public float GetDistanceTraveled()
    {
        return distanceTracker.GetCurrentDistancePassed();
    } 

    public float GetHP()
    {
        return hp;
    }

    public bool IsDead()
    {
        return isDead;
    }
    #endregion

    #region Movement
    public void StartMoving()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        SetMoving(true);

        distanceTracker.StartTrackingFromPoint();
        
        while (waypointFollower.GetNextPoint() != null)
        {
            Vector2 startPoint = waypointFollower.GetCurrentPoint().transform.position;
            distanceTracker.SetStartintPoint(startPoint);

            yield return StartCoroutine(waypointFollower.MoveToNextPoint(moveSpeed));

            distanceTracker.SaveDistancePassed();
        }

        // 끝에 도달
        if (waypointFollower.GetMovementCondition())
        {
            isDead = true; // 이름 바꿀 것
            playerStatus.TakeDamage(attackPower);
            DeactivateEnemy();
        }
    }
    #endregion

    public void DeactivateEnemy()
    {
        SetMoving(false);

        currentlyRegisteredList.RemoveFromList(this);
        ObjectPool.instance.ReturnToPool("Enemy", this.gameObject);
        
        if(isBossEnemy)
        {
            GameManager.instance.RemoveBoss(this);
            isBossEnemy = false;
        }

        distanceTracker.ResetDistance();

        gameObject.SetActive(false);
    }

    #region Damage
    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(0, hp -= damage);
        UpdateHPText();

        if (hp <= 0)
        {
            isDead = true;
            DeactivateEnemy();

            playerStatus.AddSp(10);
        }

        CallDamageEffect(damage);
    }

    private void CallDamageEffect(float damage)
    {
        Vector2 damageEffectPos = transform.position;
        damageEffectPos += damageEffectOffset;

        GameObject damageObj = ObjectPool.instance.GetObject("DamageEffect", damageEffectPos);
        DamageEffect damageEffect = damageObj.GetComponent<DamageEffect>();
        damageEffect.DisplayDamage(damage);
    } 
    #endregion
}

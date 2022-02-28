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

    #region Movement
    private Road currentRoad;
    private Road nextRoad;
    private float totalDistanceTraveled = 0f;
    private float distanceTraveled = 0f;
    #endregion

    #region Components
    private EnemyTargeter currentlyRegisteredList;
    private PlayerStatus playerStatus;
    private LerpMovement movementComp;
    #endregion

    #region Dead 
    private bool isDead;
    private Vector2 deadPos;
    #endregion

    [SerializeField]
    private Vector2 damageEffectOffset;

    private bool isMoving = true;
    // Boss property
    [SerializeField]
    private bool isBossEnemy = false;

    #region Initialization
    public void Init(EnemyStatus status, MonsterType type, Road startRoad, PlayerStatus targetPlayer)
    {
        hp = 0f;
        isDead = false;
        //InitDistance();

        InitBaseStatus(status);
        InitStatusByType(type);

        SetCurrentRoad(startRoad);
        SetTargetPlayerStatus(targetPlayer);

        UpdateHPText();

        movementComp = GetComponent<LerpMovement>();
        //StartMovement();
    }

    private void InitBaseStatus(EnemyStatus status)
    {
        hp = status.hp;
        moveSpeed = status.moveSpeed;
        attackPower = status.attackPower;
    }

    private void InitStatusByType(MonsterType type)
    {
        InitSprite(type.sprite);
        hp *= type.HpMult;
        moveSpeed *= type.moveSpeedMult;
        attackPower *= type.attackPowerMult;
    }

    private void InitSprite(Sprite spr)
    {
        GetComponent<SpriteRenderer>().sprite = spr;
    }

    private void InitDistance()
    {
        totalDistanceTraveled = 0f;
        distanceTraveled = 0f;
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
        isMoving = movingCondition;

        if (movementComp != null) 
            movementComp.SetMoving(movingCondition);
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

    public void SetCurrentRoad(Road road)
    {
        currentRoad = road;
        nextRoad = currentRoad.GetNextRoad();
    }

    public void SetIsBossEnemy(bool val)
    {
        isBossEnemy = val;
    }

    public float GetCurrentDistanceTraveled()
    {
        return totalDistanceTraveled + distanceTraveled;
    } 

    public float GetHP()
    {
        return hp;
    }

    public Vector2 GetDeadPos()
    {
        return deadPos;
    }

    public bool IsDead()
    {
        return isDead;
    }
    #endregion

    #region Movement
    public void MoveToNextRoad()
    {
        SetMoving(true);

        StartCoroutine(TrackDistanceCoroutine());
        StartCoroutine(MoveToNextRoadCoroutine());
    }

    private IEnumerator MoveToNextRoadCoroutine()
    {
        while(nextRoad != null && isMoving)
        {
            Vector2 nextPos = currentRoad.GetNextRoad().transform.position;
            float distanceBetweenRoads = Vector2.Distance(currentRoad.transform.position, nextPos);
            float ratioOfDistance = moveSpeed / distanceBetweenRoads;

            yield return StartCoroutine(movementComp.UniformMotion(currentRoad.transform.position, nextPos, 1f, ratioOfDistance));

            AddTotalDistance();
            SetCurrentRoad(currentRoad.GetNextRoad());
        }

        // Reached to end point
        if(nextRoad == null)
        {
            isMoving = false;
            isDead = true;
            playerStatus.TakeDamage(attackPower);
            DeactivateEnemy();
        }
    }

    private IEnumerator TrackDistanceCoroutine()
    {
        InitDistance();

        while(isMoving)
        {
            TrackDistance();

            yield return null;
        }
    }

    private void TrackDistance()
    {
        if (currentRoad == null) return;
        distanceTraveled = Vector2.Distance(currentRoad.transform.position, this.transform.position);
    }

    public void DeactivateEnemy()
    {
        currentlyRegisteredList.RemoveFromList(this);
        ObjectPool.instance.ReturnToPool("Enemy", this.gameObject);
        
        if(isBossEnemy)
        {
            GameManager.instance.RemoveBoss(this);
            isBossEnemy = false;
        }

        isMoving = false;

        gameObject.SetActive(false);
    }

    private void AddTotalDistance()
    {
        totalDistanceTraveled += distanceTraveled;
        distanceTraveled = 0f;
    }
    #endregion

    #region Damage
    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(0, hp -= damage);
        UpdateHPText();

        if (hp <= 0)
        {
            isDead = true;
            deadPos = transform.position;
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

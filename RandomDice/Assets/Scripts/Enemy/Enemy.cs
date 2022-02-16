﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Canvas canvas;
    public Text hpText;

    #region Status with MonsterType
    private float hp;
    private float moveSpeed;
    private int attackPower; // Player HP에 영향 
    #endregion

    #region Movement
    private Road currentRoad;
    private Road nextRoad;
    private float totalDistanceTraveled = 0f;
    private float distanceTraveled = 0f;
    private float t; 
    #endregion

    #region Components
    private EnemyManager enemyManager;
    private PlayerStatus playerStatus;
    #endregion

    #region Dead 
    private bool isDead;
    private Vector2 deadPos;
    #endregion

    [SerializeField]
    private Vector2 damageEffectOffset;

    private void Update()
    {
        MoveToNextRoad();
    }

    #region Initialization
    public void Init(EnemyStatus status, MonsterType type, Road startRoad, PlayerStatus targetPlayer)
    {
        hp = 0f;
        t = 0f;
        isDead = false;
        InitDistance();

        InitStatus(status);
        InitStatusByType(type);
        SetCurrentRoad(startRoad);
        SetTargetPlayerStatus(targetPlayer);

        InitCanvas();
        UpdateHpText();
    }

    private void InitStatus(EnemyStatus status)
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

    public void InitCanvas()
    {
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        canvasTransform.position = transform.position;
    }

    public void UpdateHpText()
    {
        hpText.text = hp.ToString();
    }
    #endregion

    #region Setter/Getter
    // Enemy가 비활성화되면 EnemyManager 리스트에서 지우기 위해서 설정
    public void SetEnemyManager(EnemyManager manager)
    {
        if (enemyManager != null) return;
        enemyManager = manager;
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
        if (nextRoad == null) // 끝에 도달 또는 Road가 세팅되지 않음
        {
            isDead = true;
            playerStatus.TakeDamage(attackPower);
            DeactivateEnemy();
            return;
        }

        Vector2 nextPos = currentRoad.GetNextRoad().transform.position;

        // Road 길이에 따라 비율 조정
        float distance = Vector2.Distance(currentRoad.transform.position, nextPos);
        float ratioOfDistance = moveSpeed / distance;
        t += ratioOfDistance * Time.deltaTime;

        transform.position = Vector2.Lerp(currentRoad.transform.position, nextPos, t);

        // 이동한 거리 업데이트
        distanceTraveled = Vector2.Distance(currentRoad.transform.position, this.transform.position);

        if (t >= 1f)
        {
            t = 0f;
            AddTotalDistance();

            SetCurrentRoad(currentRoad.GetNextRoad());
        }
    }

    private void DeactivateEnemy()
    {
        enemyManager.RemoveFromList(this);
        ObjectPool.instance.ReturnToPool("Enemy", this.gameObject); // 220211
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
        UpdateHpText();

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

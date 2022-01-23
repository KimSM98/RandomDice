using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Canvas canvas;
    public Text hpText;

    public float hp;
    public float moveSpeed;
    public float attackPower; // Player HP에 영향
    
    public Road currentRoad;
    private Road nextRoad;
    private float totalDistanceTraveled = 0f;
    private float distanceTraveled = 0f;

    private float t = 0f;

    private EnemySpawner spawner;

    private void Update()
    {
        MoveToNextRoad();
    }

    #region Initialization
    public void Init(EnemyStatus status, MonsterType type, Vector2 spawnPos)
    {
        InitStatus(status);
        InitType(type);
        InitPos(spawnPos);
        InitDistance();

        InitCanvas();
        InitHpText();
    }

    private void InitPos(Vector2 spawnPos)
    {
        transform.position = spawnPos;
    }

    // 최초로 생성될 때만
    public void InitSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    private void InitType(MonsterType type)
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

    private void InitStatus(EnemyStatus status)
    {
        hp = status.hp;
        moveSpeed = status.moveSpeed;
        attackPower = status.attackPower;
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

    public void InitHpText()
    {
        hpText.text = hp.ToString();
    }
    #endregion

    #region Setter/Getter
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
    #endregion

    public void MoveToNextRoad()
    {
        //Road nextRoad = currentRoad.GetNextRoad();
        if (nextRoad == null) // 끝에 도달 또는 Road가 세팅되지 않음
        {
            spawner.MoveToPendingList(this);
            gameObject.SetActive(false);
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
            AddTotalDist();

            SetCurrentRoad(currentRoad.GetNextRoad());
        }
    }

    private void AddTotalDist()
    {
        totalDistanceTraveled += distanceTraveled;
        distanceTraveled = 0f;
    }
}

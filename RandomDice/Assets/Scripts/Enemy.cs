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
    private float t = 0f;

    private void Update()
    {
        MoveToNextRoad();
    }

    #region Initialization
    public void Init(MonsterStatus status, MonsterType type)
    {
        InitStatus(status);
        InitType(type);

        InitCanvas();
        InitHpText();
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

    private void InitStatus(MonsterStatus status)
    {
        hp = status.hp;
        moveSpeed = status.moveSpeed;
        attackPower = status.attackPower;
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

    public void SetCurrentRoad(Road road)
    {
        currentRoad = road;
    }

    public void MoveToNextRoad()
    {
        Road nextRoad = currentRoad.GetNextRoad();
        if (nextRoad == null) return;

        Vector2 nextPos = currentRoad.GetNextRoad().transform.position;

        // Road 길이에 따라 비율 조정
        float distance = Vector2.Distance(currentRoad.transform.position, nextPos);
        float ratioOfDistance = moveSpeed / distance;
        t += ratioOfDistance * Time.deltaTime;

        transform.position = Vector2.Lerp(currentRoad.transform.position, nextPos, t);

        if(t > 1f)
        {
            t = 0f;
            SetCurrentRoad(currentRoad.GetNextRoad());
        }
    }

}

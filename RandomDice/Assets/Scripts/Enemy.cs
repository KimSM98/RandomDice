using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public MonsterStatus normalMonsterStatus;
    public Canvas canvas;
    public Text hpText;

    public float hp;
    public float moveSpeed;
    public float attackPower; // Player HP에 영향

    public void InitCanvas()
    {
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        canvasTransform.position = transform.position;
    }

    public void InitHpText()
    {
        hpText.text = hp.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public DiceEye diceEyePrefab;

    private DiceType diceType;
    private SpriteRenderer spriteRenderer;
    private float attackPower;
    private float attackSpeed;
    private Color diceEyeColor;

    private List<DiceEye> diceEyes;

    // 타겟

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        diceEyes = new List<DiceEye>();
    }

    private void Start()
    {
    }

    public void InitDice(DiceType _type)
    {
        diceType = _type;
        //attackPower = _type.attackPower;
        //attackSpeed = _type.attackSpeed;
        //spriteRenderer.sprite = _type.sprite;
        //diceEyeColor = _type.diceEyeColor;
        spriteRenderer.sprite = diceType.sprite;

        // Dice Eye 생성
        CreateDiceEye();
    }

    public void CreateDiceEye()
    {
        if (diceEyes.Count >= 6) return;

        DiceEye diceEye = Instantiate(diceEyePrefab, transform);
        diceEye.GetComponent<SpriteRenderer>().color = diceType.diceEyeColor;
        diceEyes.Add(diceEye);

        ArrangeDicePostion();
    }

    private void ArrangeDicePostion()
    {
        int count = diceEyes.Count;
        if (count == 1 || count == 3 || count == 5) return;

        switch (count)
        {
            case 2:
                diceEyes[count - 2].transform.localPosition = new Vector2(0.133f, 0.133f);
                diceEyes[count - 1].transform.localPosition = new Vector2(-0.133f, -0.133f);
                break;
            case 4:
                diceEyes[count - 2].transform.localPosition = new Vector3(-0.133f, 0.133f);
                diceEyes[count - 1].transform.localPosition = new Vector3(0.133f, -0.133f);
                break;
            case 6:
                diceEyes[count - 2].transform.localPosition = new Vector3(-0.133f, 0);
                diceEyes[count - 1].transform.localPosition = new Vector3(0.133f, 0);
                break;
        }
    }
}

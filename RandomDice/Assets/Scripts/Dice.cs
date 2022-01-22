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

    // 타겟 필요

    [SerializeField]
    private float moveSpeed = 5f;
    private Vector2 initPos;
    private bool returnToInitPos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        diceEyes = new List<DiceEye>();
    }

    private void Start()
    {
        initPos = transform.position;
        returnToInitPos = false;
    }

    private void Update()
    {
        if(returnToInitPos)
        {
            MoveToInitPos();

            if (ReachedInitPos())
                returnToInitPos = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (returnToInitPos) return;

        Dice collDice = collision.GetComponent<Dice>();
        if (collDice != null)
            DiceCollision(collDice);
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

        ArrangeDiceEyePostion();
    }

    public void Merge()
    {
        CreateDiceEye();
    }

    public string GetDiceTypeName()
    {
        return diceType.typeName;
    }

    private void ArrangeDiceEyePostion()
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

    private void MoveToInitPos()
    {
        transform.position = Vector2.Lerp(transform.position, initPos, moveSpeed * Time.deltaTime);
    }

    private bool ReachedInitPos()
    {
        Vector2 currentPos = transform.position;
        if (Vector2.Distance(currentPos, initPos) < 0.001f)
        {
            return true;
        }

        return false;
    }

    private void DiceCollision(Dice collDice)
    {
        // 현재 상황
        // 두 다이스가 충돌하고 있기 때문에 플레이어가 잡고 있는 다이스가 없어지고, 
        // 안 잡고 있는 다이스가 Merge되도록
        string collDiceType = collDice.GetDiceTypeName();
        if (diceType.typeName == collDiceType)
        {
            Debug.Log("같은 타입 입니다. 머지");
            //collDice.Merge();
            //this.gameObject.SetActive(false);
        }
        else
        {
            returnToInitPos = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public DiceEye diceEyePrefab;

    #region Components
    private BoardInfo boardInfo;
    private DiceType diceType;
    private SpriteRenderer spriteRenderer;
    #endregion

    private List<DiceEye> diceEyes;

    [SerializeField]
    private Enemy target; // target to attack
    
    #region Dice Selection var
    [SerializeField]
    private float moveSpeed = 5f;
    private bool returnToInitPos;
    private bool selected;
    #endregion    

    #region Dice Merger var
    private Dice coll;
    private bool canMerge;
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        diceEyes = new List<DiceEye>();
    }

    private void Start()
    {
        returnToInitPos = false;
        selected = false;
        canMerge = false;
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

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!selected) return;
        if (returnToInitPos) return;

        Dice collDice = collision.GetComponent<Dice>();
        if (collDice != null)
            DiceCollision(collDice);
    }

    private void DiceCollision(Dice collDice)
    {
        coll = null;

        string collDiceType = collDice.GetDiceTypeName();
        if (diceType.typeName == collDiceType && diceEyes.Count == collDice.diceEyes.Count)
        {
            canMerge = true;
            coll = collDice;
        }
    } 
    #endregion

    #region Initialization
    public void InitDice(DiceType _type)
    {
        InitDiceByType(_type);

        // Dice Eye 생성
        CreateDiceEye();
    }
    private void InitDiceByType(DiceType _type)
    {
        diceType = _type;
        spriteRenderer.sprite = diceType.sprite;
    } 

    private void InitDiceEye()
    {
        foreach(var diceEye in diceEyes)
        {
            diceEye.Init(diceType);
        }
    }
    #endregion

    #region Setter/Getter
    public void SetBoardInfo(BoardInfo bi)
    {
        boardInfo = bi;
    }

    public void SetReturnToInitPos(bool val)
    {
        returnToInitPos = val;
    }

    public void SetSelected(bool val)
    {
        selected = val;
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
        foreach(DiceEye diceEye in diceEyes)
        {
            diceEye.SetTarget(target);
        }
    }

    // Getter
    public BoardInfo GetBoardInfo()
    {
        return boardInfo;
    }
    public string GetDiceTypeName()
    {
        return diceType.typeName;
    } 

    public DiceType.TargetType GetDiceTargetType()
    {
        return diceType.targetType;
    }
    #endregion

    #region Dice
    public bool Merge(DiceType randType)
    {
        if (!canMerge) return false;
        if (coll == null) return false;
        if (diceEyes.Count == 6) return false;

        // 랜덤 타입으로 변화
        coll.InitDiceByType(randType);
        coll.InitDiceEye();
        coll.CreateDiceEye();

        Destroy(this.gameObject);

        return true;
    }

    private void MoveToInitPos()
    {
        transform.position = Vector2.Lerp(transform.position, boardInfo.boardPos, moveSpeed * Time.deltaTime);
    }

    private bool ReachedInitPos()
    {
        Vector2 currentPos = transform.position;
        if (Vector2.Distance(currentPos, boardInfo.boardPos) < 0.001f)
        {
            return true;
        }

        return false;
    } 
    #endregion

    #region DiceEye
    public void CreateDiceEye()
    {
        if (diceEyes.Count >= 6) return;

        DiceEye diceEye = Instantiate(diceEyePrefab, transform);
        diceEye.Init(diceType);
        diceEyes.Add(diceEye);

        ArrangeDiceEyePostion();
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
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private DiceManager diceManager;

    private Vector2 mouseWorldPos;

    [SerializeField]
    private Dice selectedDice;

    private void Start()
    {
        diceManager = GetComponentInChildren<DiceManager>();    
    }

    void Update()
    {
        MouseControl();
    }

    private void MouseControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (!hit) return;

            SelectDice(hit);
        }
        else if(Input.GetMouseButton(0))
        {
            MoveDice();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            PutDice();
        }
    }   

    private void SelectDice(RaycastHit2D hit)
    {
        selectedDice = hit.transform.GetComponent<Dice>();
        if (!selectedDice) return;

        selectedDice.GetComponent<SpriteRenderer>().sortingOrder = 2;
        selectedDice.SetSelected(true);
    }

    private void MoveDice()
    {
        if (!selectedDice) return;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedDice.transform.position = mouseWorldPos;
    }
    
    private void PutDice()
    {
        if (!selectedDice) return;

        if(selectedDice.Merge())
        {
            diceManager.RemoveActiveDice(selectedDice);
        }
        else
        {
            ResetSelectedDice();
        }
    }

    private void ResetSelectedDice()
    {
        selectedDice.GetComponent<SpriteRenderer>().sortingOrder = 0;
        selectedDice.SetReturnToInitPos(true);
        selectedDice.SetSelected(false);

        selectedDice = null;
    }
}
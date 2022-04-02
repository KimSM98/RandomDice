using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    private DiceManager diceManager;
    private DiceSpawner diceSpawner; 
    #endregion

    private Vector2 mouseWorldPos;

    [SerializeField]
    private Dice selectedDice;

    private void Start()
    {
        diceManager = GetComponentInChildren<DiceManager>();
        diceSpawner = GetComponentInChildren<DiceSpawner>();
    }

    void Update()
    {
        InputControl();
    }

    private void InputControl()
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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

        DiceType randType = diceSpawner.GetRandDiceType();
        if (selectedDice.Merge(randType))
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
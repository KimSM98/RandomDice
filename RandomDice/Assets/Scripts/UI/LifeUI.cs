using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public Sprite offImage;

    private Image imageComp;
    private bool turnedOff;

    private void Start()
    {
        turnedOff = false;
        imageComp = GetComponent<Image>();
    }

    public bool TurnedOff()
    {
        return turnedOff;
    }
    public void OffLifeUI()
    {
        turnedOff = true;
        imageComp.sprite = offImage;
    }
}

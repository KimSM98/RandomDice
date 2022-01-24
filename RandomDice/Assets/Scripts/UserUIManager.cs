using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIManager : MonoBehaviour
{
    public Text spText;
    public LifeUI[] lifes;
    
    public void Inin(int sp)
    {
        SetSPText(sp);        
    }

    public void SetSPText(int sp)
    {
        spText.text = sp.ToString();
    }

    public void OffLifeUI(int currentHP) // -2가 들어올 수도 있음
    {
        for(int i = lifes.Length - 1; i >= currentHP; i--)
        {
            if (lifes[i].TurnedOff()) continue;
            lifes[currentHP].OffLifeUI();
        }
    }

}

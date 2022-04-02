using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIManager : MonoBehaviour
{
    public Text currentSPText;
    public Text spawnDiceSPText;
    public LifeUI[] lifes;

    private int currentLife;

    private void Start()
    {
        currentLife = lifes.Length - 1;
    }

    public void Inin(int sp, int spawnDiceSP)
    {
        UpdateCurrentSPText(sp);
        UpdateSpawnDiceSP(spawnDiceSP);
    }

    public void UpdateCurrentSPText(int sp)
    {
        currentSPText.text = sp.ToString();
    }
    public void UpdateSpawnDiceSP(int val)
    {
        spawnDiceSPText.text = val.ToString();
    }

    public void OffLifeUI() // -2가 들어올 수도 있음, 3에서 1이 될 때
    {
        //currentLife--;

        if (currentLife < 0) return;

        lifes[currentLife].OffLifeUI();
        currentLife--;
    }
}

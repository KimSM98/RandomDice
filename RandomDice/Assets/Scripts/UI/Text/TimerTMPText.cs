using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTMPText : TMPController
{
    private void Start()
    {
        GetComponent<TimerDisplay>().textUpdateEvent += SetText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TimerDisplay : Timer
{
    public Action<string> textUpdateEvent;

    public override IEnumerator RunTimer()
    {
        while(timeRemaining > 0 )
        {
            yield return waitForOneSecond;
            timeRemaining--;

            UpdateTimeText(timeRemaining);
        }
    }

    public override void SetTimeToCount(int duration)
    {
        base.SetTimeToCount(duration);
        UpdateTimeText(durationInSec);
    }
    
    public override void ResetTimer()
    {
        base.ResetTimer();
        UpdateTimeText(timeRemaining);
    }

    private void UpdateTimeText(int seconds)
    {
        if (textUpdateEvent != null)
            textUpdateEvent(seconds.ToString());
    }
}

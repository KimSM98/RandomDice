using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private int secToCount;
    private int remainTime;
    private WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
    
    public IEnumerator RunTimer()
    {
        while(remainTime > 0 )
        {
            yield return waitForOneSecond;
            remainTime--;

            UpdateTimerText(remainTime);
        }
    }

    public void SetTimeToCount(int timeToEnd)
    {
        secToCount = timeToEnd;
        remainTime = secToCount;

        UpdateTimerText(secToCount);
    }
    
    public void ResetTimer()
    {
        remainTime = secToCount;
        UpdateTimerText(remainTime);
    }

    public void SetTimerTMP(TextMeshProUGUI text)
    {
        timerText = text;
    }

    private void UpdateTimerText(int seconds)
    {
        timerText.text = seconds.ToString();
    }
}

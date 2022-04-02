using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    protected int durationInSec;
    protected int timeRemaining;
    protected WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

    public virtual IEnumerator RunTimer()
    {
        while (timeRemaining > 0)
        {
            yield return waitForOneSecond;
            timeRemaining--;
        }
    }

    public virtual void SetTimeToCount(int duration)
    {
        durationInSec = duration;
        timeRemaining = durationInSec;
    }

    public virtual void ResetTimer()
    {
        timeRemaining = durationInSec;
    }
}

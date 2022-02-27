using System.Collections;
using System;

public class TimerDisplay : Timer
{
    public Action<string> timeTextUpdate;

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
        int min = seconds / 60;
        int sec = seconds % 60;

        string time = MakeTwoDigit(min) + ":" + MakeTwoDigit(sec);

        timeTextUpdate?.Invoke(time);
    }

    private string MakeTwoDigit(int val)
    {
        string twoDigitStr = null;
        int[] digitNums = new int[2];
        digitNums[0] = val / 10;
        digitNums[1] = val % 10;

        foreach(int digitNum in digitNums)
        {
            if (digitNum > 0)
                twoDigitStr += digitNum.ToString();
            else
                twoDigitStr += '0';
        }

        return twoDigitStr;
    }
}

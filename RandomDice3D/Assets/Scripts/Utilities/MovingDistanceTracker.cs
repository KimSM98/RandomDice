using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 특정 위치로부터의 거리를 계산하고 저장한다.
public class MovingDistanceTracker : MonoBehaviour
{
    private Vector2 startingPoint;
    
    private float totalDistancePassed = 0f;
    private float distanceFromPoint = 0f;
    private bool isRunning = false;

    public void StartTrackingFromPoint()
    {
        isRunning = true;
        StartCoroutine(DistanceTracking());
    }

    private IEnumerator DistanceTracking()
    {
        while(isRunning)
        {
            TrackDistance();
            yield return null;
        }
    }
    private void TrackDistance()
    {
        if (startingPoint == null) return;
        distanceFromPoint = Vector2.Distance(startingPoint, this.transform.position);
    }

    public void ResetDistance()
    {
        totalDistancePassed = 0f;
        distanceFromPoint = 0f;
    }

    public void SaveDistancePassed()
    {
        totalDistancePassed += distanceFromPoint;
        distanceFromPoint = 0f;
    }


    #region Setter/Getter
    public void SetStartintPoint(Vector2 pos)
    {
        startingPoint = pos;
    }

    public void SetRunningCondition(bool condition)
    {
        isRunning = condition;
    }
    public float GetCurrentDistancePassed()
    {
        return totalDistancePassed + distanceFromPoint;
    } 
    #endregion

}

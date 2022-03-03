using System.Collections;
using UnityEngine;

public class WaypointFollower : LerpMovement
{
    private Waypoint currentWaypoint;
    private Waypoint nextWaypoint;

    #region Waypoint Movements
    // Waypoints 순회
    public IEnumerator MoveAlongWaypoints(float moveSpeed = 1f)
    {
        while (nextWaypoint != null && isMovementActive)
        {
            yield return StartCoroutine(MoveToNextPoint(moveSpeed));
        }
    }

    public IEnumerator MoveToNextPoint(float moveSpeed = 1f)
    {
        yield return StartCoroutine(MoveLerp(nextWaypoint.transform, 1f, moveSpeed));
        SetCurrentPoint(currentWaypoint.GetNextPoint());
    } 
    #endregion

    #region Setter/Getter
    public void SetCurrentPoint(Waypoint point)
    {
        currentWaypoint = point;
        nextWaypoint = currentWaypoint.GetNextPoint();
    }
    public Waypoint GetCurrentPoint()
    {
        return currentWaypoint;
    }

    public Waypoint GetNextPoint()
    {
        return nextWaypoint;
    } 
    #endregion
}

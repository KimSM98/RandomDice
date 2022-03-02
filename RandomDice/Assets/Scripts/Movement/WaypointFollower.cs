using System.Collections;
using UnityEngine;

public class WaypointFollower : LerpMovement
{
    [SerializeField]
    private Waypoint currentWaypoint;
    [SerializeField]
    private Waypoint nextWaypoint;

    // Waypoints 순회
    public IEnumerator MoveAlongWaypoints(float moveSpeed = 1f)
    {
        while(nextWaypoint != null && isMovementActive)
        {
            yield return StartCoroutine(MoveToNextPoint(moveSpeed));
        }
    }

    public IEnumerator MoveToNextPoint(float moveSpeed = 1f)
    {
        Vector2 nextPos = nextWaypoint.transform.position;
        float speedWeightToMoveSameSpeed = CalculateRatioOfDistance(moveSpeed);

        yield return StartCoroutine(UniformMotion(currentWaypoint.transform.position, nextPos, 1f, speedWeightToMoveSameSpeed));

        SetCurrentPoint(currentWaypoint.GetNextPoint());
    }

    // Waypoint 사이를 같은 속도로 이동하기 위한 스피드 가중치 계산
    private float CalculateRatioOfDistance(float moveSpeed)
    {
        float distanceBetweenRoads = Vector2.Distance(currentWaypoint.transform.position, nextWaypoint.transform.position);
        
        return moveSpeed / distanceBetweenRoads;
    }

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
}

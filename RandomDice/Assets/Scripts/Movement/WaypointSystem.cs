using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public Waypoint[] waypoints;

    private void Awake()
    {
        InitNextWaypoint();
    }

    private void InitNextWaypoint()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            waypoints[i].SetNextPoint(waypoints[i + 1]);
        }
    }

    public Waypoint GetWaypoint(int num)
    {
        return waypoints[num];
    }
}

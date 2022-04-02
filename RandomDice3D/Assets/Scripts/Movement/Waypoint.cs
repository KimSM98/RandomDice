using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private Waypoint nextWaypoint;

    public void SetNextPoint(Waypoint wp)
    {
        nextWaypoint = wp;
    }

    public Waypoint GetNextPoint()
    {
        return nextWaypoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (nextWaypoint != null)
            Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);

        Vector3 size = new Vector3(0.25f, 0.25f, 0);
        Gizmos.DrawCube(transform.position, size);
    }
}

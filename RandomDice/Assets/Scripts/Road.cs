using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private Road nextRoad;

    private void Start()
    {
        nextRoad = null;
    }

    public void SetNextRoad(Road road)
    {
        nextRoad = road;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (nextRoad != null)
            Gizmos.DrawLine(transform.position, nextRoad.transform.position);

        Vector3 size = new Vector3(0.25f, 0.25f, 0);
        Gizmos.DrawCube(transform.position, size);
    }
}

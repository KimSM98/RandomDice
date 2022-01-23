using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoad : MonoBehaviour
{
    public Road[] roads;

    private float totalRoadLength;

    private void Start()
    {
        InitRoads();
    }

    private void InitRoads()
    {
        totalRoadLength = 0f;

        for (int i = 0; i < roads.Length - 1; i++)
        {
            roads[i].SetNextRoad(roads[i + 1]);
            totalRoadLength += Vector2.Distance(roads[i].transform.position, roads[i + 1].transform.position);
        }
    }

    public Road GetRoad(int num)
    {
        return roads[num];
    }
}

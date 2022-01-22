using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoad : MonoBehaviour
{
    public Road[] roads;

    private void Start()
    {
        InitRoads();
    }

    private void InitRoads()
    {
        for (int i = 0; i < roads.Length - 1; i++)
        {
            roads[i].SetNextRoad(roads[i + 1]);
        }
    }

    public Road GetRoad(int num)
    {
        return roads[num];
    }
}

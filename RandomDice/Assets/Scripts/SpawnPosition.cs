using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public float xOffset;
    public float yOffset;

    private List<Vector2> emptyPos;
    private List<Vector2> activePos;

    bool afterStart = false; // for Gizmos

    void Start()
    {
        emptyPos = new List<Vector2>();
        activePos = new List<Vector2>();

        InitSpawnPosition();

        afterStart = true;
    }

    private void InitSpawnPosition()
    {
        Vector2 startPos = transform.position;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector2 pos = new Vector2(startPos.x + xOffset * j, startPos.y - yOffset * i);
                emptyPos.Add(pos);
            }
        }
    }

    public Vector2 GetRandomEmptyPos()
    {
        int randNum = Random.Range(0, emptyPos.Count);
        Vector2 pos = emptyPos[randNum];
        emptyPos.RemoveAt(randNum);
        activePos.Add(pos);

        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 size = new Vector3(0.1f, 0.1f);

        Gizmos.DrawCube(transform.position, size);

        if (!afterStart) return;

        Gizmos.color = Color.green;
        foreach (Vector2 pos in emptyPos)
        {
            Gizmos.DrawCube(pos, size);
        }

        Gizmos.color = Color.magenta;
        foreach (Vector2 pos in activePos)
        {
            Gizmos.DrawCube(pos, size);
        }
    }
}

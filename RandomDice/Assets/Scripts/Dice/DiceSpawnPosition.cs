using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawnPosition : MonoBehaviour
{
    public float xOffset;
    public float yOffset;

    [SerializeField]
    private List<BoardInfo> emptyBoard;
    [SerializeField]
    private List<BoardInfo> activeBoard;

    bool afterStart = false; // for Gizmos

    void Start()
    {
        emptyBoard = new List<BoardInfo>();
        activeBoard = new List<BoardInfo>();

        InitSpawnPosition();

        afterStart = true;
    }

    public void MoveToEmptyBoard(BoardInfo boardInfo)
    {
        emptyBoard.Add(boardInfo);
        activeBoard.Remove(boardInfo);
    }

    private void InitSpawnPosition()
    {
        Vector2 startPos = transform.position;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector2 pos = new Vector2(startPos.x + xOffset * j, startPos.y - yOffset * i);

                BoardInfo bi = new BoardInfo(i, pos);
                emptyBoard.Add(bi);
            }
        }
    }

    public BoardInfo GetRandomBoard()
    {
        int randNum = Random.Range(0, emptyBoard.Count);
        BoardInfo boardInfo = emptyBoard[randNum];
      
        emptyBoard.RemoveAt(randNum);
        activeBoard.Add(boardInfo);

        return boardInfo;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 size = new Vector3(0.1f, 0.1f);

        Gizmos.DrawCube(transform.position, size);

        if (!afterStart) return;

        Gizmos.color = Color.green;
        foreach (BoardInfo bi in emptyBoard)
        {
            Gizmos.DrawCube(bi.boardPos, size);
        }

        Gizmos.color = Color.magenta;
        foreach (BoardInfo bi in activeBoard)
        {
            Gizmos.DrawCube(bi.boardPos, size);
        }
    }
}

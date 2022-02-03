using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{
    Cell[,] grid = new Cell[25,25];
    [SerializeField] Cell cellPref;
    [SerializeField] CellData cellData;

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 tileDistance = CalculateTileDistance();
        for (int i = 0; i < 25;i++)
        {
            for (int j = 0; j < 25; j++)
            {
                Vector2 pos = Vector2.zero;
                pos.x = (firstPos.x + 0.25f )- i * tileDistance.x * 2f;
                pos.y = (firstPos.y + 0.25f )- j * tileDistance.y * 2f;

                grid[i, j] = Instantiate(cellPref, this.transform);
                grid[i, j].Init(cellData.GetCell(j * 25 + i), j, i, pos,cellData);
            }
        }
    }

    private Vector2 CalculateTileDistance()
    {
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 temp = Vector2.zero;
        temp.x = firstPos.x / 25;
        temp.y = firstPos.y / 25;
        return temp;
    }


    public Cell GetCellObject(int gridY, int gridX)
    {
        return grid[gridX, gridY];
    }

}

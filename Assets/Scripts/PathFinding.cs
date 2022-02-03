using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    MoveGrid grid;
    List<Cell> openList;
    List<Cell> closedList;

    public void Init(MoveGrid gridToSet)
    {
        grid = gridToSet;
    }

    public List<Cell> GetPath(int startX, int startY, int endX, int endY)
    {
        Cell pathStartCell = grid.GetCellObject(startX, startY);
        Cell pathEndCell = grid.GetCellObject(endX, endY);

        if (pathEndCell == pathStartCell)
        {
            Debug.Log("<color=red> Start and End Should be different. </color>");
            return null;
        }

        openList = new List<Cell> { pathStartCell };
        closedList = new List<Cell>();

        InitCellsCosts();

        pathStartCell.gCost = 0;
        pathStartCell.heuristicCost = CalcualteHCost(pathStartCell, pathEndCell);
        pathStartCell.CalculateFCost();

        while (openList.Count > 0)
        {
            Cell currentCell = GetMinFCost(openList); // TODO : Min-heap

            currentCell.type = CellType.CheckedPoint;

            if (currentCell == pathEndCell)
            {
                return CalculatePath(currentCell);
            }

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (Cell neighborCell in GetAllNeighbors(currentCell))
            {
                if (closedList.Contains(neighborCell) || neighborCell.isBlock)
                    continue;

                int tempGCost = CalcualteHCost(currentCell, neighborCell) + currentCell.gCost; // gCost of current node = distance between currentnode and it's parent + distance from root to parent.
                if (tempGCost < neighborCell.gCost) // instead of fCost we just compare gCost
                {
                    neighborCell.parentCell = currentCell;
                    neighborCell.gCost = tempGCost;
                    neighborCell.heuristicCost = CalcualteHCost(neighborCell, pathEndCell);
                    neighborCell.CalculateFCost();
                }

                if (!openList.Contains(neighborCell))
                    openList.Add(neighborCell);
            }
        }

        return null;
    }

    private List<Cell> CalculatePath(Cell endCell)
    {
        List<Cell> path = new List<Cell> { endCell };

        while(endCell.parentCell)
        {
            path.Add(endCell.parentCell);
            endCell = endCell.parentCell;

            endCell.type = CellType.FinalPathPoint;
        }

        path.Reverse();

        return path;
    }

    private Cell GetMinFCost(List<Cell> openList)
    {
        Cell minCell = openList[0];
        foreach(Cell tempCell in openList)
        {
            if(minCell.fCost > tempCell.fCost)
            {
                minCell = tempCell;
            }
        }

        return minCell;
    }

    private void InitCellsCosts()
    {
        for (int i = 0; i < 25; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                Cell tempCell = grid.GetCellObject(i, j);
                tempCell.gCost = int.MaxValue;
                tempCell.CalculateFCost();
                tempCell.parentCell = null;
            }
        }
    }

    private int CalcualteHCost(Cell startCell,Cell endCell) //Without considering blocks.
    {
        int xDistance = Mathf.Abs(startCell.gridX - endCell.gridX);
        int yDistance = Mathf.Abs(startCell.gridY - endCell.gridY);
        return xDistance + yDistance;
    }

    private List<Cell> GetAllNeighbors(Cell parentCell)
    {
        List<Cell> childs = new List<Cell>();
        int parentX = parentCell.gridX;
        int parentY = parentCell.gridY;

        //Right Child
        if(parentY + 1 < 25 )
            childs.Add(grid.GetCellObject(parentX, parentY+1));

        //Left Child
        if(parentY - 1 >= 0)
            childs.Add(grid.GetCellObject(parentX, parentY - 1));

        //Up Child
        if(parentX + 1 < 25)
            childs.Add(grid.GetCellObject(parentX + 1, parentY));

        //Down Child
        if(parentX - 1 >= 0)
            childs.Add(grid.GetCellObject(parentX - 1, parentY));

        return childs;
    }
}

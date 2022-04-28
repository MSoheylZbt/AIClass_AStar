using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    MoveGrid grid; // Reference to grid
    List<Cell> openList; //openList used to keep nodes that will exapnd in future.
    List<Cell> closedList; // closedList used to keep unreachable nodes (expanded or blocked)

    public int checkedNodeCounter = 0; // This variabale hold numbers of nodes that added to open list and checked during GetPath()

    public PathFinding(MoveGrid gridToSet) // Constructor
    {
        grid = gridToSet;
    }


    /// <summary>
    /// Use A* algorithm and Min-Heap to find a path between start and end point
    /// </summary>
    /// <returns></returns>
    public List<Cell> GetPath()
    {
        GridData gridData = grid.gridData;

        //Get Path Start and End point from given Data
        Cell pathStartCell = grid.GetCellObjectByXY(gridData.startCellX, gridData.startCellY);
        Cell pathEndCell = grid.GetCellObjectByXY(gridData.endCellX,gridData.endCellY);

        //if End and Start is same point
        if (pathEndCell == pathStartCell)
        {
            return null;
        }

        openList = new List<Cell> { pathStartCell }; // init with start point
        closedList = new List<Cell>();

        InitCellsCosts();

        //Initial StartCell properties.
        pathStartCell.gCost = 0;
        pathStartCell.heuristicCost = CalcualteHCost(pathStartCell, pathEndCell);
        pathStartCell.CalculateFCost();

        MinHeap fCostHeap = new MinHeap(grid); //With min-heaps when a cell from openList with min fCost is required, We can just pop from it and there will be no need to search

        fCostHeap.Add(pathStartCell.index); // add first cell 

        while (openList.Count > 0)
        {
            checkedNodeCounter++;
            Cell currentCell;

            currentCell = grid.GetCellObjectByIndex(fCostHeap.Pop()); // Get a Cell with min f cost

            //For Running algorithm without min-heap, Comment Upper line and de-comment bottom line.

            //currentCell = GetMinFCost(openList);

            currentCell.type = CellType.CheckedPoint;
            currentCell.SetSpriteColor();

            if (currentCell == pathEndCell) // When we reach the end of path
            {
                return CalculatePath(currentCell);
            }

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (Cell neighborCell in GetAllNeighbors(currentCell))
            {
                if (closedList.Contains(neighborCell) || neighborCell.isBlock) // Go to next cell
                    continue;
 
                int tempGCost = CalcualteHCost(currentCell, neighborCell) + currentCell.gCost; // gCost of current node = distance between currentnode and it's parent + distance from root to parent.
                if (tempGCost < neighborCell.gCost) // check if algorithm reach a node with lesser gCost.
                {
                    neighborCell.parentCell = currentCell;
                    neighborCell.gCost = tempGCost;
                    neighborCell.heuristicCost = CalcualteHCost(neighborCell, pathEndCell);
                    neighborCell.CalculateFCost();
                }

                if (!openList.Contains(neighborCell))
                {
                    openList.Add(neighborCell);
                    fCostHeap.Add(neighborCell.index);// Add cell indexs to min-heap but we compare their fCost in Min-Heap.
                }
            }

        }

        return null;
    }

    /// <summary>
    /// Calcualte the path from given cell by using parentCell property.
    /// </summary>
    /// <param name="endCell"></param>
    /// <returns></returns>
    private List<Cell> CalculatePath(Cell endCell)
    {
        List<Cell> path = new List<Cell> { endCell };

        while(endCell.parentCell)
        {
            path.Add(endCell.parentCell);
            endCell = endCell.parentCell;

            endCell.type = CellType.FinalPathPoint;
            endCell.SetSpriteColor();
        }

        path.Reverse();

        return path;
    }

    /// <summary>
    /// This function loop through all cells of given list and return the one with minimum Fcost value.
    /// </summary>
    /// <param name="openList"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Set all cells gCost and fCost to infinite and parent to null (maybe they hold value from their previous run).
    /// </summary>
    private void InitCellsCosts()
    {
        for (int i = 0; i < 25; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                Cell tempCell = grid.GetCellObjectByXY(i, j);
                tempCell.gCost = int.MaxValue;
                tempCell.CalculateFCost();
                tempCell.parentCell = null;
            }
        }
    }

    /// <summary>
    /// Distance between two givel cell (without considering blocks and Diagonal move)
    /// </summary>
    /// <param name="startCell"></param>
    /// <param name="endCell"></param>
    /// <returns></returns>
    private int CalcualteHCost(Cell startCell,Cell endCell) 
    {
        int xDistance = Mathf.Abs(startCell.gridX - endCell.gridX);
        int yDistance = Mathf.Abs(startCell.gridY - endCell.gridY);
        return xDistance + yDistance;
    }

    /// <summary>
    /// Return a list of all childs of given node.
    /// </summary>
    /// <param name="parentCell"></param>
    /// <returns></returns>
    private List<Cell> GetAllNeighbors(Cell parentCell)
    {
        List<Cell> childs = new List<Cell>();
        int parentX = parentCell.gridX;
        int parentY = parentCell.gridY;

        //Right Child
        if(parentY + 1 < 25 ) // Can't move outside of grid
            childs.Add(grid.GetCellObjectByXY(parentX, parentY+1));

        //Left Child
        if(parentY - 1 >= 0)
            childs.Add(grid.GetCellObjectByXY(parentX, parentY - 1));

        //Up Child
        if(parentX + 1 < 25) //Can't move outside of grid
            childs.Add(grid.GetCellObjectByXY(parentX + 1, parentY));

        //Down Child
        if(parentX - 1 >= 0)
            childs.Add(grid.GetCellObjectByXY(parentX - 1, parentY));

        return childs;
    }


}

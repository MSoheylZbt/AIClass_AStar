using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData 
{
    //This two list is used for tracking cells properties in Json file.
    public List<bool> cellBools;
    public List<int> cellBoolIndexes;

    //properties of Start point and end point of path
    //any defined path with PathFinding class will take it's end and start point from here.
    public int startCellX,startCellY;
    public int endCellX,endCellY;

    public GridData()
    {
        cellBools = new List<bool>();
        cellBoolIndexes = new List<int>();

        //Init to Start point of Grid 
        startCellX = 0;
        startCellY = 0;
        //Init to End point of Grid 
        endCellX = 24;
        endCellY = 24;
    }

    /// <summary>
    /// Add a cell or update exisiting cells
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void UpdateGrid(int key,bool value)
    {
        if (cellBoolIndexes.Contains(key))
        {
            int index = cellBoolIndexes.IndexOf(key);
            UpdateCell(index, value);
            return;
        }

        cellBoolIndexes.Add(key);
        cellBools.Add(value);
    }

    private void UpdateCell(int index, bool value)
    {
        cellBools[index] = value;
    }

    /// <summary>
    /// return a bool indicating isBlock of given cell.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetCellBool(int key)
    {
        if (cellBoolIndexes.Contains(key) == false)
            return false;

        int index = cellBoolIndexes.IndexOf(key);
        return cellBools[index];
    }

}

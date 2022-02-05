using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData 
{
    public List<bool> cellBools;
    public List<int> cellBoolIndexes;

    public int startCellX,startCellY;
    public int endCellX,endCellY;


    public CellData()
    {
        cellBools = new List<bool>();
        cellBoolIndexes = new List<int>();

        startCellX = 0;
        startCellY = 0;
        endCellX = 24;
        endCellY = 24;
    }

    public void AddCell(int key,bool value)
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

    public bool GetCell(int key)
    {
        if (cellBoolIndexes.Contains(key) == false)
            return false;

        int index = cellBoolIndexes.IndexOf(key);
        return cellBools[index];
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cell Data")]
public class CellData : ScriptableObject
{
    [SerializeField] List<bool> cellBools = new List<bool>();
    [SerializeField] List<int> cellBoolIndexes = new List<int>();
    public int startCellX,startCellY;
    public int endCellX,endCellY;


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

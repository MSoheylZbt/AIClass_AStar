using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour
{
    [SerializeField] MoveGrid grid;
    [SerializeField] CellData cellData;
    PathFinding pathFinding = new PathFinding();

    private void Start()
    {
        pathFinding.Init(grid);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            pathFinding.GetPath(cellData.startCellX, cellData.startCellY, cellData.endCellX, cellData.endCellY);
        }
    }
}

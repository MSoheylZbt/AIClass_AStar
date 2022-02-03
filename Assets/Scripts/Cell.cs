using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class Cell : MonoBehaviour
{
    #region Utilities
    CellData cellData;
    public int gridX, gridY;
    #endregion

    #region for Pathfinding
    public bool isBlock;
    public int gCost;
    public int heuristicCost;
    public int fCost;
    public Cell parentCell;
    #endregion

    #region Cache
    bool isSkeyHold = false;
    bool isEkeyHold = false;
    #endregion

    #region Editor
    public Color gizmosColor;
    public bool isInPathFinding = false;
    #endregion
    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
            isSkeyHold = true;

        if (Input.GetKeyUp(KeyCode.S))
            isSkeyHold = false;

        if (Input.GetKey(KeyCode.E))
            isEkeyHold = true;

        if (Input.GetKeyUp(KeyCode.E))
            isEkeyHold = false;
    }


    public void Init(bool isBlocked,int gridX , int gridY,Vector2 pos,CellData data)
    {
        GetComponent<CircleCollider2D>().radius = 0.1f;

        transform.position = pos;
        isBlock = isBlocked;
        this.gridX = gridX;
        this.gridY = gridY;

        cellData = data;
    }

    public void CalculateFCost()
    {
        fCost = heuristicCost + gCost;
    }


    private void OnMouseDown()
    {
        if(isEkeyHold)
        {
            print("End Cell is : " + this.gridX + " " + this.gridY);
            cellData.endCellX = gridX;
            cellData.endCellY = gridY;
            return;
        }

        if(isSkeyHold)
        {
            print("Start Cell is : " + this.gridX + " " + this.gridY);
            cellData.startCellX = gridX;
            cellData.startCellY = gridY;
            return;
        }

        print(gridX + " " + gridY);
        isBlock = !isBlock;
        cellData.AddCell(gridX * 25 + gridY , isBlock);
    }


    private void OnDrawGizmos()
    {
        if (isBlock)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        if(isInPathFinding)
            Gizmos.color = gizmosColor;

        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}

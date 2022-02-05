using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum CellType // For Drawing Cell in scene with different color, we need to define different types for cells.
{
    Normal,
    Blocked,
    CheckedPoint,
    FinalPathPoint,
    EndPoint,
    StartPoint
}

[RequireComponent(typeof(CircleCollider2D))] // This attribute will add a CircleCollider2D in a GameObject that it added. we need CircleCollider2D for Clicking function.
public class Cell : MonoBehaviour
{
    #region Utilities
    GridData gridData;
    public int gridX, gridY;//This cell X and Y in grid.

    public int index // this Cell Grid index
    {
        get
        {
            return (gridX * 25 + gridY);
        }
    }
    #endregion

    #region for Pathfinding
    public bool isBlock; // is this cell block?
    public int gCost; //Distance between start cell and this cell.
    public int heuristicCost; // Distance between this cell and Target cell.
    public int fCost; // Sum of gCost and heuristicCost
    public Cell parentCell; // reference to parent cell
    #endregion

    #region Cache
    bool isSkeyHold = false;
    bool isEkeyHold = false;
    SpriteRenderer spriteRender;//Reference to this Cell sprite.
    #endregion

    #region Editor
    public CellType type = CellType.Normal;
    #endregion


    private void Update() // Update Method run at start of each frame.
    {
        if (Input.GetKey(KeyCode.S)) // Check for holding S Key on Keyboard
            isSkeyHold = true;

        if (Input.GetKeyUp(KeyCode.S)) // Check for Releasing S Key on Keyboard
            isSkeyHold = false;

        if (Input.GetKey(KeyCode.E))
            isEkeyHold = true;

        if (Input.GetKeyUp(KeyCode.E))
            isEkeyHold = false;
    }

    /// <summary>
    /// This Method acts as an Constructor.
    /// </summary>
    /// <param name="isBlocked"></param>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <param name="pos"></param>
    /// <param name="data"></param>
    public void Init(bool isBlocked,int gridX , int gridY,Vector2 pos,GridData data) 
    {
        GetComponent<CircleCollider2D>().radius = 0.1f;//Set Radius of area that players can click.
        spriteRender = GetComponent<SpriteRenderer>(); // Set Reference to image of this cell.

        transform.position = pos; // set this cell position.

        isBlock = isBlocked;

        if (isBlock) // set this cell color
        {
            type = CellType.Blocked;
            SetSpriteColor();
        }

        this.gridX = gridX;
        this.gridY = gridY;
        gridData = data;
    }

    public void CalculateFCost()
    {
        fCost = heuristicCost + gCost;
    }


    private void OnMouseDown() // This function will run whenever player clicked on this cell
    {
        if(isEkeyHold) // if E key is holding, then set this cell to End point of path in grid data.
        {
            //print("End Cell is : " + this.gridX + " " + this.gridY);
            gridData.endCellX = gridX;
            gridData.endCellY = gridY;
            type = CellType.EndPoint;
            SetSpriteColor();
            return;
        }

        if(isSkeyHold) // if S key is holding, then set this cell to Start point of path in grid data.
        {
            //print("Start Cell is : " + this.gridX + " " + this.gridY);
            gridData.startCellX = gridX;
            gridData.startCellY = gridY;
            type = CellType.StartPoint;
            SetSpriteColor();
            return;
        }

        isBlock = !isBlock; // Each time player clicks on a cell, it will toggle it's Blcoking situation.
        if (isBlock)
        {
            type = CellType.Blocked;
            SetSpriteColor();
        }
        else
        {
            type = CellType.Normal;
            SetSpriteColor();
        }

        gridData.UpdateGrid(gridX * 25 + gridY , isBlock); // Add this cell to grid data.


        //Update Json File for saving this grid.
        string jsonFile = JsonUtility.ToJson(gridData);
        File.WriteAllText(Application.dataPath + "/saveFile.json", jsonFile);
    }

    /// <summary>
    /// Set Image of Cell based on it's type
    /// </summary>
    public void SetSpriteColor()
    {
        switch (type)
        {
            case CellType.Blocked:
                spriteRender.color = Color.black;
                break;

            case CellType.CheckedPoint:
                spriteRender.color = Color.yellow;
                break;

            case CellType.FinalPathPoint:
                spriteRender.color = Color.magenta;
                break;

            case CellType.StartPoint:
                spriteRender.color = Color.green;
                break;

            case CellType.EndPoint:
                spriteRender.color = Color.red;
                break;

            default:
                spriteRender.color = Color.white;
                break;
        }
    }
}

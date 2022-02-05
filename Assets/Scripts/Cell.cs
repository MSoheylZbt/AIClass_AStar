using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum CellType
{
    Normal,
    Blocked,
    CheckedPoint,
    FinalPathPoint,
    EndPoint,
    StartPoint
}

[RequireComponent(typeof(CircleCollider2D))]
public class Cell : MonoBehaviour
{
    #region Utilities
    CellData cellData;
    public int gridX, gridY;

    public int index
    {
        get
        {
            return (gridX * 25 + gridY);
        }
    }
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
    SpriteRenderer spriteRender;
    #endregion

    #region Editor
    public CellType type = CellType.Normal;
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
        spriteRender = GetComponent<SpriteRenderer>();

        transform.position = pos;

        isBlock = isBlocked;
        if (isBlock)
        {
            type = CellType.Blocked;
            SetSpriteColor();
        }

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
            //print("End Cell is : " + this.gridX + " " + this.gridY);
            cellData.endCellX = gridX;
            cellData.endCellY = gridY;
            type = CellType.EndPoint;
            SetSpriteColor();
            return;
        }

        if(isSkeyHold)
        {
            //print("Start Cell is : " + this.gridX + " " + this.gridY);
            cellData.startCellX = gridX;
            cellData.startCellY = gridY;
            type = CellType.StartPoint;
            SetSpriteColor();
            return;
        }

        //print(gridX + " " + gridY);

        isBlock = !isBlock;
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

        cellData.AddCell(gridX * 25 + gridY , isBlock);


        //Update Json File
        string jsonFile = JsonUtility.ToJson(cellData);
        File.WriteAllText(Application.dataPath + "/saveFile.json", jsonFile);
    }


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

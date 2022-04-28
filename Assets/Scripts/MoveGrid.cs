using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MoveGrid : MonoBehaviour //inherited from MonoBehaviour. This will allow us to use this class as a component in unity editor.
{
    [SerializeField] Cell cellPref; //Prefab of Cell class. this will allow us to set a reference to Cell class via unity editor.

    Cell[,] grid = new Cell[25,25];//2D array for keeping grid.

    public GridData gridData;

    private void Start() // This Function will run at Start of the game.
    {
        //Read Grid data from file
        if (File.Exists(Application.dataPath + "/saveFile.json"))
        {
            string jsonFile = File.ReadAllText(Application.dataPath + "/saveFile.json");
            gridData = JsonUtility.FromJson<GridData>(jsonFile);
        }
        else
        {
            gridData = new GridData();
            string jsonData = JsonUtility.ToJson(gridData);
            File.WriteAllText(Application.dataPath + "/saveFile.json",jsonData);
        }

        CreateGrid();

    }

    private void CreateGrid()
    {
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)); // Convert Screen position of (0,0) which is most down-left point on screen, to a world position.
                                                                                    //World position is position of any object in game world.

        Vector2 tileDistance = CalculateTileDistance(); //Distance between each cell in grid according to number of them.

        for (int i = 0; i < 25;i++) // Loop through all grids cell and make (Instantiate) a cell in each position
        {
            for (int j = 0; j < 25; j++)
            {
                Vector2 pos = Vector2.zero;
                pos.x = (firstPos.x + 0.25f ) - i * tileDistance.x * 2f; // 0.25 is distance from most down-left point of camera.
                                                                            // 2 is Multiplier that make distance between tiles bigger. 
                pos.y = (firstPos.y + 0.25f ) - j * tileDistance.y * 2f;

                grid[i, j] = Instantiate(cellPref, this.transform);
                grid[i, j].Init(gridData.GetCellBool(j * 25 + i), j, i, pos,gridData);
            }
        }
    }

    private Vector2 CalculateTileDistance() //WRONG
    {
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 temp = Vector2.zero; //Vector2 is used for not indicating a direction or position but just for holding two numbers.
        temp.x = firstPos.x / 25;
        temp.y = firstPos.y / 25;
        return temp;
    }


    public Cell GetCellObjectByXY(int gridY, int gridX)
    {
        return grid[gridX, gridY];
    }

    public Cell GetCellObjectByIndex(int index)
    {
        int y = index / 25;
        int x = index % 25;
        return grid[x, y];
    }

}

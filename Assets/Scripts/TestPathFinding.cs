using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestPathFinding : MonoBehaviour
{
    [SerializeField] MoveGrid grid;
    [SerializeField] TextMeshProUGUI UIText;

    PathFinding pathFinding = new PathFinding();
    List<Cell> cells;

    float elapsedTime = 0;
    bool isTimerStarted = false;

    private void Start()
    {
        pathFinding.Init(grid);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!isTimerStarted)
                StartCoroutine(Timer());

            cells = pathFinding.GetPath();
            if (cells == null)
                UIText.text = "There is No Path!";
        }
    }

    IEnumerator Timer()
    {
        isTimerStarted = true;
        while(cells == null)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        UIText.text = "Elapsed Time: " + elapsedTime + " \nNumber of Expanded nodes: " + pathFinding.checkedNodeCounter;
        isTimerStarted = false;
    }
}

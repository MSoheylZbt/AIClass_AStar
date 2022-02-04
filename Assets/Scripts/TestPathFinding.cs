using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour
{
    [SerializeField] MoveGrid grid;

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

        print( "elapsedTime: " + elapsedTime);
        print( "Number of Counted nodes: " + pathFinding.checkedNodeCounter);
        isTimerStarted = false;
    }
}

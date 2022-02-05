using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestPathFinding : MonoBehaviour
{
    [SerializeField] MoveGrid grid;
    [SerializeField] TextMeshProUGUI UIText; // Reference to Text

    PathFinding pathFinding;
    List<Cell> cells;

    float elapsedTime = 0; // Elapsed time for calculating algorithm run time
    bool isTimerStarted = false; // for not runnig a previously runned coroutine again.

    private void Start() // This function will run after Awake() and at start of game.
    {
        pathFinding = new PathFinding(grid);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!isTimerStarted)
                StartCoroutine(Timer());

            cells = pathFinding.GetPath();
            if(cells == null)
                UIText.text = "There is No Path!";
        }
    }

    /// <summary>
    /// A Coroutine that wait for PathFinding algorithm to end and calculate it's compelition duration.
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        isTimerStarted = true;
        while(cells == null)
        {
            elapsedTime += Time.deltaTime; // Time.deltaTime is duration bertween last two frame.
            yield return new WaitForEndOfFrame(); // Wait a frame and then go for next cycle.
        }

        UIText.text = "Elapsed Time: " + elapsedTime + " \nNumber of Expanded nodes: " + pathFinding.checkedNodeCounter;
        isTimerStarted = false;
    }
}

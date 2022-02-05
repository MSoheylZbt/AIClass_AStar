using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap
{
    private List<int> _elements;

    private MoveGrid grid; //MinHeap should has acceess to grid because it us returning a list of int but we need Cells in A* algorithm.

    public MinHeap(MoveGrid gridToSet)
    {
        _elements = new List<int>();
        grid = gridToSet; 
    }

    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _elements.Count;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _elements.Count;
    private bool IsRoot(int elementIndex) => elementIndex == 0;

    private int GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
    private int GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
    private int GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

    private void Swap(int firstIndex, int secondIndex)
    {
        var temp = _elements[firstIndex];
        _elements[firstIndex] = _elements[secondIndex];
        _elements[secondIndex] = temp;
    }


    public bool IsEmpty()
    {
        return _elements.Count == 0;
    }

    public int Peek()
    {
        if (_elements.Count == 0)
            Debug.Log("index out of range");

        return _elements[0];
    }

    public int Pop()
    {
        if (_elements.Count == 0)
            Debug.Log("index out of range");

        var result = _elements[0];
        _elements[0] = _elements[_elements.Count - 1];
        _elements.RemoveAt(_elements.Count - 1);

        HeapifyDown();//Because we remove from start of tree.
        return result;
    }

    public void Add(int element)
    {
        List<int> temp = new List<int> { element };
        _elements.AddRange(temp);

        HeapifyUp(); // Because we add to the end of tree.
    }

    /// <summary>
    /// Start from first node and with comparison to child nodes swap them if they are less.
    /// </summary>
    private void HeapifyDown()
    {
        int index = 0;
        while (HasLeftChild(index)) // if there is no left child then there will be no right child too.
        {
            var smallerIndex = GetLeftChildIndex(index);

            //In _elements index of cells will be stored and with indexes, we compare their fCost using grid reference.
            if (HasRightChild(index) && grid.GetCellObjectByIndex(GetRightChild(index)).fCost < grid.GetCellObjectByIndex(GetLeftChild(index)).fCost)
            {
                smallerIndex = GetRightChildIndex(index);
            }

            if (grid.GetCellObjectByIndex(_elements[smallerIndex]).fCost >= grid.GetCellObjectByIndex(_elements[index]).fCost)
            {
                break;
            }

            Swap(smallerIndex, index);
            index = smallerIndex;
        }
    }

    /// <summary>
    /// Start from last node and with comparison to parent node will heapify the node.
    /// </summary>
    private void HeapifyUp()
    {
        var index = _elements.Count - 1;
        while (!IsRoot(index) && grid.GetCellObjectByIndex(_elements[index]).fCost < grid.GetCellObjectByIndex(GetParent(index)).fCost) // Same as HeapifyDown
        {
            var parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }
}

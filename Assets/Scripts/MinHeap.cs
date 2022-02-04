using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap
{
    private List<int> _elements;
    private MoveGrid grid;

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
        {
            Debug.Log("index out of range");
        } 

        var result = _elements[0];
        _elements[0] = _elements[_elements.Count - 1];
        _elements.RemoveAt(_elements.Count - 1);

        HeapifyDown();
        return result;
    }

    public void Add(int element)
    {
        List<int> temp = new List<int> { element };
        _elements.AddRange(temp);

        HeapifyUp();
    }

    private void HeapifyDown()
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            var smallerIndex = GetLeftChildIndex(index);
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

    private void HeapifyUp()
    {
        var index = _elements.Count - 1;
        while (!IsRoot(index) && grid.GetCellObjectByIndex(_elements[index]).fCost < grid.GetCellObjectByIndex(GetParent(index)).fCost)
        {
            var parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }
}

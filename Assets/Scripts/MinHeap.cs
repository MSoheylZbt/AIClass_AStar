using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap
{
    private readonly int[] _elements;
    private int _size;

    public List<Cell> cells;

    public MinHeap(int size)
    {
        _elements = new int[size];
    }

    public MinHeap(List<Cell> cellsToSet)
    {
        cells = cellsToSet;
        _elements = GetListFCosts(cells);
        HeapifyUp();
    }

    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
    private bool IsRoot(int elementIndex) => elementIndex == 0;

    private int GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
    private int GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
    private int GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

    private void Swap(int firstIndex, int secondIndex)
    {
        var temp = _elements[firstIndex];
        if(cells.Count <= 0)
        {
            _elements[firstIndex] = _elements[secondIndex];
            _elements[secondIndex] = temp;
        }
        else
        {
            Cell tempCell = cells[firstIndex];

            _elements[firstIndex] = _elements[secondIndex];
            cells[firstIndex] = cells[secondIndex];

            _elements[secondIndex] = temp;
            cells[secondIndex] = tempCell;
        }

    }


    public bool IsEmpty()
    {
        return _size == 0;
    }

    public int Peek()
    {
        if (_size == 0)
            Debug.Log("index out of range");

        return _elements[0];
    }

    public int Pop()
    {
        if (_size == 0)
            Debug.Log("index out of range");

        var result = _elements[0];
        _elements[0] = _elements[_size - 1];
        _size--;

        HeapifyDown();

        return result;
    }

    public void Add(int element)
    {
        if (_size == _elements.Length)
            Debug.Log("index out of range");

        _elements[_size] = element;
        _size++;

        HeapifyUp();
    }

    private void HeapifyDown()
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            var smallerIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChild(index) < GetLeftChild(index))
            {
                smallerIndex = GetRightChildIndex(index);
            }

            if (_elements[smallerIndex] >= _elements[index])
            {
                break;
            }

            Swap(smallerIndex, index);
            index = smallerIndex;
        }
    }

    private void HeapifyUp()
    {
        var index = _size - 1;
        while (!IsRoot(index) && _elements[index] < GetParent(index))
        {
            var parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }

    private int[] GetListFCosts(List<Cell> cells)
    {
        int[] result = new int[cells.Count];
        for (int i = 0; i < cells.Count; i++)
        {
            result[i] = cells[i].fCost;
        }
        return result;
    }
}

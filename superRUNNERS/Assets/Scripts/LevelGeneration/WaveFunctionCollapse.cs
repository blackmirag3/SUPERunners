using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public Vector3 startPos;
    public float cellLength;
    public float gridSize;

    public GameObject cellPrefab;
    public List<Cell> cells;
    public List<Cell> remainingCells;
    public Dictionary<Vector3, Cell> activeCells = new Dictionary<Vector3, Cell>();
    public List<Cell> cellsToPropagate = new List<Cell>();


    private void Start()
    {
        InstantiateCells();
        TestActivateCell();
    }
    
    private void InstantiateCells()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 pos = startPos + new Vector3(x * cellLength, 0, z * cellLength);

                GameObject newCellBlock = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                Cell newCell = newCellBlock.GetComponent<Cell>();
                newCell.coords = pos;
                cells.Add(newCell);
                activeCells.Add(newCell.coords, newCell);
            }
        }

        remainingCells = cells;

        foreach (Cell cell in cells)
        {
            FindCellNeighbors(cell);
        }
    }

    private void FindCellNeighbors(Cell cell)
    {
        cell.posXNeighbor = FindCell(cell.coords.x + cellLength, cell.coords.z);
        cell.negXNeighbor = FindCell(cell.coords.x - cellLength, cell.coords.z);
        cell.posZNeighbor = FindCell(cell.coords.x, cell.coords.z + cellLength);
        cell.negZNeighbor = FindCell(cell.coords.x, cell.coords.z - cellLength);
    }

    private Cell FindCell(float x, float z)
    {
        Cell cell = null;
        if (activeCells.TryGetValue(new Vector3(x, startPos.y, z), out cell))
        {
            return cell;
        }
        return null;
    }

    private void TestActivateCell()
    {
        while (!AllCollapsed())
        {
            Cell cellToCollapse = remainingCells[Random.Range(0, remainingCells.Count)];
            CollapseAt(cellToCollapse);
        }
    }

    private bool AllCollapsed()
    {
        if (remainingCells.Count != 0)
        {
            return false;
        }
        return true;
    }

    private void CollapseAt(Cell cell)
    {
        int prototypeIndex = Random.Range(0, cell.possiblePrototypes.Count);
        Prototype selectedProto = cell.possiblePrototypes[prototypeIndex];
        // selectedProto.prefab = cell.possiblePrototypes[prototypeIndex].prefab;
        cell.possiblePrototypes.Clear();
        cell.possiblePrototypes.Add(selectedProto);
        GameObject tilePrefab = Instantiate(selectedProto.prefab, cell.transform, true);

        tilePrefab.transform.Rotate(new Vector3(0f, selectedProto.meshRotation * 90f, 0f), Space.Self);
        tilePrefab.transform.localPosition = Vector3.zero;

        cell.isCollapsed = true;

        if (remainingCells.Contains(cell))
        {
            remainingCells.Remove(cell);
        }
    }
}

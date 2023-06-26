using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class WaveFunctionCollapse : MonoBehaviour
{
    public Vector3 startPos;
    public float cellLength;
    public float gridSize;

    [SerializeField] private Prototype startingTile;

    public GameObject cellPrefab;
    public List<Cell> cells;
    public List<Cell> remainingCells;
    public Dictionary<Vector3, Cell> activeCells = new Dictionary<Vector3, Cell>();
    public List<Cell> cellsToPropagate = new List<Cell>();

    public Vector3 firstCellCoords;

    public NavMeshSurface surface;

    [SerializeField] private EnemySpawn enemies;
    [SerializeField] private ItemSpawn items;

    public void BuildArena(Component sender, object data)
    {
        Debug.Log("Building arena");
        if (data is not int)
        {
            Debug.Log($"Error in data received from event caller {sender}");
            return;
        }
        int enemyCount = (int)data;

        ResetGrid();
        StartCollapse();
        StartCoroutine(UpdateMeshAndSpawn(enemyCount));
    }

    private void Start()
    {
        surface.BuildNavMesh();
    }

    private IEnumerator UpdateMeshAndSpawn(int enemyCount)
    {
        yield return null;
        Destroy(surface.navMeshData);
        surface.BuildNavMesh();
        enemies.SpawnEnemies(enemyCount);
        items.SpawnItems();
    }

    private void ResetGrid()
    {
        Debug.Log("Reset");
        ClearGrid();
        remainingCells.Clear();
        activeCells.Clear();
        InstantiateCells();
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
                remainingCells.Add(newCell);
                activeCells.Add(newCell.coords, newCell);
            }
        }

        //remainingCells = cells;

        foreach (Cell cell in remainingCells)
        {
            FindCellNeighbors(cell);
        }
    }

    private void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
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

    private void StartCollapse()
    {
        Cell firstCell = FindCell(firstCellCoords.x, firstCellCoords.z);
        if (firstCell == null)
        { 
            firstCell = remainingCells[Random.Range(0, remainingCells.Count)];
            Debug.LogWarning($"Failed to locate cell at {firstCellCoords}");
        }
        FirstCollapse(firstCell);
        PropogateCells(firstCell);

        while (!AllCollapsed())
        {
            Cell cellToCollapse = GetLowestCellEntropy();

            if (cellToCollapse.possiblePrototypes.Count == 0)
            {
                Debug.LogWarning("Failure case encountered");
                ResetGrid();
                cellToCollapse = remainingCells[Random.Range(0, remainingCells.Count)];
            }

            CollapseAt(cellToCollapse);
            PropogateCells(cellToCollapse);
        }
    }

    private void FirstCollapse(Cell cell)
    {
        cell.possiblePrototypes.Clear();
        cell.possiblePrototypes.Add(startingTile);
        GameObject tilePrefab = Instantiate(startingTile.prefab, cell.transform, true);

        tilePrefab.transform.Rotate(new Vector3(0f, startingTile.meshRotation * 90f, 0f), Space.Self);
        tilePrefab.transform.localPosition = Vector3.zero;

        cell.isCollapsed = true;

        if (remainingCells.Contains(cell))
        {
            remainingCells.Remove(cell);
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
        int prototypeIndex = GetIndexToCollapse(cell);
        Prototype selectedProto = cell.possiblePrototypes[prototypeIndex];
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

    private int GetIndexToCollapse(Cell cell)
    {
        int weightSum = 0;

        foreach (Prototype prototype in cell.possiblePrototypes)
        {
            weightSum += prototype.weight;
        }

        int randomWeightVal = Random.Range(0, weightSum + 1);

        for (int i = 0, toCompare = 0; i < cell.possiblePrototypes.Count; i++)
        {
            toCompare += cell.possiblePrototypes[i].weight;
            if (randomWeightVal <= toCompare)
            {
                return i;
            }
        }

        return 0;
    }

    private Cell GetLowestCellEntropy()
    {
        List<Cell> possibleCells = new List<Cell>();
        int minSoFar = int.MaxValue;

        foreach (Cell cell in remainingCells)
        {
            if (cell.possiblePrototypes.Count < minSoFar)
            {
                possibleCells.Clear();
                possibleCells.Add(cell);
                minSoFar = cell.possiblePrototypes.Count;
            }
            else if (cell.possiblePrototypes.Count == minSoFar)
            {
                possibleCells.Add(cell);
            }
        }

        return possibleCells[Random.Range(0, possibleCells.Count)];
    }

    private void PropogateCells(Cell cell)
    {
        cellsToPropagate.Add(cell);

        while (cellsToPropagate.Count > 0)
        {
            Cell currCell = cellsToPropagate[0];
            cellsToPropagate.Remove(currCell);

            Cell neighborCell = currCell.posXNeighbor;
            List<FaceData> socketsAccepted = new List<FaceData>();
            socketsAccepted.Clear();
            // positive x neighbor
            if (neighborCell != null)
            {
                bool cellAdjusted = false;
                foreach (Prototype proto in currCell.possiblePrototypes)
                {
                    if (!socketsAccepted.Contains(proto.posX))
                    {
                        socketsAccepted.Add(proto.posX);
                    }
                }
                for (int i = 0; i < neighborCell.possiblePrototypes.Count; i++)
                {
                    if (!CheckForMatch(neighborCell.possiblePrototypes[i].negX, socketsAccepted))
                    {
                        neighborCell.possiblePrototypes.RemoveAt(i);
                        i--;
                        cellAdjusted = true;
                    }
                }

                if (cellAdjusted)
                {
                    cellsToPropagate.Add(neighborCell);
                }
            }

            // negative x neighbor
            socketsAccepted.Clear();
            neighborCell = currCell.negXNeighbor;
            if (neighborCell != null)
            {
                bool cellAdjusted = false;
                foreach (Prototype proto in currCell.possiblePrototypes)
                {
                    if (!socketsAccepted.Contains(proto.negX))
                    {
                        socketsAccepted.Add(proto.negX);
                    }
                }
                for (int i = 0; i < neighborCell.possiblePrototypes.Count; i++)
                {
                    if (!CheckForMatch(neighborCell.possiblePrototypes[i].posX, socketsAccepted))
                    {
                        neighborCell.possiblePrototypes.RemoveAt(i);
                        i--;
                        cellAdjusted = true;
                    }
                }

                if (cellAdjusted)
                {
                    cellsToPropagate.Add(neighborCell);
                }
            }

            // positive z neighbor
            socketsAccepted.Clear();
            neighborCell = currCell.posZNeighbor;
            if (neighborCell != null)
            {
                bool cellAdjusted = false;
                foreach (Prototype proto in currCell.possiblePrototypes)
                {
                    if (!socketsAccepted.Contains(proto.posZ))
                    {
                        socketsAccepted.Add(proto.posZ);
                    }
                }
                for (int i = 0; i < neighborCell.possiblePrototypes.Count; i++)
                {
                    if (!CheckForMatch(neighborCell.possiblePrototypes[i].negZ, socketsAccepted))
                    {
                        neighborCell.possiblePrototypes.RemoveAt(i);
                        i--;
                        cellAdjusted = true;
                    }
                }

                if (cellAdjusted)
                {
                    cellsToPropagate.Add(neighborCell);
                }
            }

            // negative z neighbor
            socketsAccepted.Clear();
            neighborCell = currCell.negZNeighbor;
            if (neighborCell != null)
            {
                bool cellAdjusted = false;
                foreach (Prototype proto in currCell.possiblePrototypes)
                {
                    if (!socketsAccepted.Contains(proto.negZ))
                    {
                        socketsAccepted.Add(proto.negZ);
                    }
                }
                for (int i = 0; i < neighborCell.possiblePrototypes.Count; i++)
                {
                    if (!CheckForMatch(neighborCell.possiblePrototypes[i].posZ, socketsAccepted))
                    {
                        neighborCell.possiblePrototypes.RemoveAt(i);
                        i--;
                        cellAdjusted = true;
                    }
                }

                if (cellAdjusted)
                {
                    cellsToPropagate.Add(neighborCell);
                }
            }
        }
    }

    private bool CheckForMatch(FaceData socket, List<FaceData> socketsToCheck)
    {
        foreach (FaceData face in socketsToCheck)
        {
            switch (socket.socketType)
            {
                case SocketType.symmetric:
                    if (socket.socket == face.socket)
                    {
                        return true;
                    }
                    break;
                case SocketType.asymmetric:
                    if (socket.socket == face.socket && socket.flipped != face.flipped)
                    {
                        return true;
                    }
                    break;
                case SocketType.oneWay:
                    if (socket.socket == face.socket && face.prototypeID != socket.prototypeID)
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour
{
    public GameObject borderWall;
    public GameObject borderCorner;
    public GameObject startRoom;

    public Transform exitRoomHolder;

    private float startX, startZ, endX, endZ;
    private float cellLength, gridSize;
    private Vector3 startPos;

    public WaveFunctionCollapse wave;

    [SerializeField]
    private List<Side> possibleSides = new List<Side>(){ Side.west, Side.north, Side.east, Side.south };
    private Side entrySide, exitSide;

    [SerializeField]
    private Vector3 entryCoord, exitCoord;

    private List<Vector3> wallCoords = new List<Vector3>();
    private Dictionary<Vector3, Side> walls = new Dictionary<Vector3, Side>();

    public GameEvent updateWaypointPos;

    // We start spawn at the south
    private void Start()
    {
        entrySide = Side.west;
        possibleSides.Remove(entrySide);

        cellLength = wave.cellLength;
        gridSize = wave.gridSize;
        startPos = wave.startPos;
    }

    public void BuildBorder()
    {
        ClearOldBorders();

        startX = startPos.x - cellLength;
        startZ = startPos.z - cellLength;

        endX = startX + (gridSize + 1) * cellLength;
        endZ = startZ + (gridSize + 1) * cellLength;

        for (int i = 1; i < gridSize + 1; i++)
        {
            Vector3 west = new Vector3(startX, 0f, startZ + (i * cellLength));
            Vector3 north = new Vector3(startX + (i * cellLength), 0f, endZ);
            Vector3 east = new Vector3(endX, 0f, startZ + (i * cellLength));
            Vector3 south = new Vector3(startX + (i * cellLength), 0f, startZ);
            // Debug.Log($"West = {west}, North = {north}, East = {east}, South = {south}");
            walls.Add(west, Side.west);
            wallCoords.Add(west);

            walls.Add(north, Side.north);
            wallCoords.Add(north);

            walls.Add(east, Side.east);
            wallCoords.Add(east);

            walls.Add(south, Side.south);
            wallCoords.Add(south);
        }

        wallCoords.Remove(entryCoord);
        walls.Remove(entryCoord);
        InstantiateExitRoom();
        StartCoroutine(UpdateSides());

        foreach (Vector3 coord in wallCoords)
        {
            GameObject newWall = Instantiate(borderWall, transform, true);
            newWall.transform.localPosition = coord;

            switch (walls[coord])
            {
                case Side.west:
                    break;
                case Side.north:
                    newWall.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
                    break;
                case Side.east:
                    newWall.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                    break;
                case Side.south:
                    newWall.transform.rotation = Quaternion.AngleAxis(270f, Vector3.up);
                    break;
            }
        }

        AddBorderCorners();

        wallCoords.Clear();
        walls.Clear();
    }
    
    private void InstantiateExitRoom()
    {
        // Select exit side
        exitSide = possibleSides[Random.Range(0, possibleSides.Count)];
        int sidePos = Random.Range(1, (int)gridSize + 1);

        GameObject exitRoom = Instantiate(startRoom, exitRoomHolder, true);
        switch (exitSide)
        {
            case Side.west:
                exitCoord = new Vector3(startX, 0f, startZ + (sidePos * cellLength));
                exitRoom.transform.localPosition = exitCoord - cellLength * Vector3.right;
                exitRoom.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);

                entryCoord = exitCoord - (2 * cellLength * Vector3.right);
                break;
            case Side.north:
                exitCoord = new Vector3(startX + (sidePos * cellLength), 0f, endZ);
                exitRoom.transform.localPosition = exitCoord + cellLength * Vector3.forward;
                exitRoom.transform.rotation = Quaternion.AngleAxis(270f, Vector3.up);

                entryCoord = exitCoord + (2 * cellLength * Vector3.forward);
                break;
            case Side.east:
                exitCoord = new Vector3(endX, 0f, startZ + (sidePos * cellLength));
                exitRoom.transform.localPosition = exitCoord + cellLength * Vector3.right;

                entryCoord = exitCoord + (2 * cellLength * Vector3.right);
                break;
            case Side.south:
                exitCoord = new Vector3(startX + (sidePos * cellLength), 0f, startZ);
                exitRoom.transform.localPosition = exitCoord - cellLength * Vector3.forward;
                exitRoom.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);

                entryCoord = exitCoord - (2 * cellLength * Vector3.forward);
                break;
        }

        updateWaypointPos.CallEvent(this, exitCoord);
        wallCoords.Remove(exitCoord);
        walls.Remove(exitCoord);
    }

    private void AddBorderCorners()
    {
        GameObject southWest = Instantiate(borderCorner, transform, true);
        southWest.transform.localPosition = new Vector3(startX, 0f, startZ);

        GameObject westNorth = Instantiate(borderCorner, transform, true);
        westNorth.transform.localPosition = new Vector3(startX, 0f, endZ);
        westNorth.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);

        GameObject northEast = Instantiate(borderCorner, transform, true);
        northEast.transform.localPosition = new Vector3(endX, 0f, endZ);
        northEast.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);

        GameObject eastSouth = Instantiate(borderCorner, transform, true);
        eastSouth.transform.localPosition = new Vector3(endX, 0f, startZ);
        eastSouth.transform.rotation = Quaternion.AngleAxis(270f, Vector3.up);
    }

    IEnumerator UpdateSides()
    {
        yield return null;
        possibleSides.Add(entrySide);
        wave.firstCellCoords = entryCoord;

        switch (exitSide)
        {
            case Side.west:
                entrySide = Side.east;
                wave.startPos += (gridSize + 3) * cellLength * -Vector3.right;
                wave.firstCellCoords += cellLength * -Vector3.right;
                break;
            case Side.north:
                entrySide = Side.south;
                wave.startPos += (gridSize + 3) *cellLength * Vector3.forward;
                wave.firstCellCoords += cellLength * Vector3.forward;
                break;
            case Side.east:
                entrySide = Side.west;
                wave.startPos += (gridSize + 3) * cellLength * Vector3.right;
                wave.firstCellCoords += cellLength * Vector3.right;
                break;
            case Side.south:
                entrySide = Side.north;
                wave.startPos += (gridSize + 3) * cellLength * -Vector3.forward;
                wave.firstCellCoords += cellLength * -Vector3.forward;
                break;
        }
        startPos = wave.startPos;
        
        possibleSides.Remove(entrySide);
    }

    private void ClearOldBorders()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (exitRoomHolder.childCount != 0)
        {
            exitRoomHolder.GetChild(0).SetParent(transform);
        }
    }
}

public enum Side
{
    west,
    north,
    east,
    south,
}



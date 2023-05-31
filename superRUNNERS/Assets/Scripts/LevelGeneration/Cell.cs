using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isCollapsed;
    public List<Prototype> possiblePrototypes;
    public Vector3 coords = new Vector3();
    public Cell posXNeighbor;
    public Cell negXNeighbor;
    public Cell posZNeighbor;
    public Cell negZNeighbor;
}

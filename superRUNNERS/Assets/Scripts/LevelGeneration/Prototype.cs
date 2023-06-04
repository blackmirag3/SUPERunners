using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype")]
public class Prototype : ScriptableObject
{
    public int meshRotation;
    public GameObject prefab;
    public FaceData posX;
    public FaceData negX;
    public FaceData posZ;
    public FaceData negZ;
    public int weight;
    public NeighbourList validNeighbours;
}

[System.Serializable]
public enum Socket
{
    ground,
    a,
    b,
    air,
    high,
}

[System.Serializable]
public struct FaceData
{
    public bool isSymmetric;
    public Socket socket;
    public bool flipped;
}

[System.Serializable]
public class SocketConnection
{
    public Socket socket;
    public List<Socket> validConnections;
}

[System.Serializable]
public class NeighbourList
{
    public List<Prototype> posX = new List<Prototype>();
    public List<Prototype> posZ = new List<Prototype>();
    public List<Prototype> negX = new List<Prototype>();
    public List<Prototype> negZ = new List<Prototype>();

}

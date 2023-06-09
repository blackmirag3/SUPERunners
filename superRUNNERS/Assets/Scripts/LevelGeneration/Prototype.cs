using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype")]
public class Prototype : ScriptableObject
{
    public int meshRotation;
    public GameObject prefab;
    public byte id;
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
    air,
    b,
    wall,
    high,
}

public enum SocketType
{
    symmetric,
    asymmetric,
    oneWay,
}

[System.Serializable]
public struct FaceData
{
    public SocketType socketType;
    public Socket socket;
    public bool flipped;
    [HideInInspector]
    public byte prototypeID;
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

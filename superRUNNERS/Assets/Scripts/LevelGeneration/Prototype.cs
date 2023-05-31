using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype")]
public class Prototype : ScriptableObject
{
    public int meshRotation;
    public GameObject prefab;
    public Socket posX;
    public Socket negX;
    public Socket posZ;
    public Socket negZ;
    public NeighbourList validNeighbours;
}

[System.Serializable]
public enum Socket
{
    ground,
    a,
    b,
    air,
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObject : MonoBehaviour
{
    [SerializeField] private string targetName;
    private GameObject target;
    public Transform location;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find(targetName);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) location = target.transform;
    }
}

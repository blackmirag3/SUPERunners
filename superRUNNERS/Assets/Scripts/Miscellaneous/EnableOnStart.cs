using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    void Start()
    {
        obj.SetActive(true);
    }
}

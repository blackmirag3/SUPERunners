using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    //public Transform transform;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   //Neutral: Quaternion(-0.0998974591,-0.317469656,-0.0535808504,0.941468358)
        targetRotation = Quaternion.Euler(-12.834f + 1.4f * Mathf.Sin(Time.time / 10f), -37.021f + 1.2f * Mathf.Sin(Time.time / 5f), -2.202f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}

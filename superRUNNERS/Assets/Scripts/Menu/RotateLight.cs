using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public Transform transform;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // x: 15 to 25
        // y: -60 to -70    
        targetRotation = Quaternion.Euler(19f + Mathf.Sin(Time.time / 20), -63f - 3 * Mathf.Cos(Time.time/20), 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

}

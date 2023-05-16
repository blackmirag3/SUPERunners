using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class PlayerCam : MonoBehaviour
{
    public WallRunning wallRunning;

    public float sensX, sensY, mouseY, mouseX;

    public Transform orientation;
    public float xRotation;
    public float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRunning.tilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, wallRunning.tilt);
    }
}

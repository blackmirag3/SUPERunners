using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class PlayerCam : MonoBehaviour
{
    public WallRunning wallRunning;
    public PlayerMovement pMove;

    public float sensX, sensY, mouseY, mouseX;

    public Transform orientation;
    [HideInInspector] public float xRotation;
    [HideInInspector] public float yRotation;

    private Camera cam;

    private float baseFov;
    [SerializeField] private float sprintFov;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
        baseFov = cam.fieldOfView;
    }

    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRunning.Tilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, wallRunning.Tilt);

        fovUpdate();
    }

    private void fovUpdate()
    {
        if (pMove.isSprinting && !pMove.isWallRunning)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFov, 20f * Time.deltaTime);
        }
        else if (!pMove.isSprinting && !pMove.isWallRunning)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFov, 20f * Time.deltaTime);
        }
    }
}

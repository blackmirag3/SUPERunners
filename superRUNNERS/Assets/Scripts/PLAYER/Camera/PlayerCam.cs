using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private PlayerCamSettings settings;

    public WallRunning wallRunning;
    public PlayerMovement pMove;

    public float sensX, sensY, mouseY, mouseX;

    public Transform orientation;
    [HideInInspector] public float xRotation;
    [HideInInspector] public float yRotation;

    private Camera cam;

    private float baseFov;
    [SerializeField] private float sprintFov;

    private bool isBobEnabled;
    private float bobAmplitude;
    public float bobFrequency;
    private Vector3 startPos;
    private float stabAmount;

    public PlayerMovement player;
    public Transform camHolder;

    private void InitialiseSettings()
    {
        sprintFov = settings.sprintFov;
        sensX = settings.sensX;
        sensY = settings.sensY;
        isBobEnabled = settings.isBobEnabled;
        bobAmplitude = settings.bobAmplitude;
        bobFrequency = settings.bobFrequency;
        stabAmount = settings.stabAmount;
    }

    void Start()
    {
        InitialiseSettings();
        startPos = transform.localPosition;
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

    void LateUpdate()
    {
        PlayMotion();
        ResetPosition();
        //cam.LookAt(FocusTarget());
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

    private void PlayMotion()
    {
        if (isBobEnabled && (player.isGrounded || player.isWallRunning) && player.isWASD)
        {
            transform.localPosition += transform.up * Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.localPosition += transform.right * Mathf.Cos(Time.time * bobFrequency / 2) * bobAmplitude / 2;
        }
    }

    private void ResetPosition()
    {
        if (transform.localPosition != startPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
        }
    }

    /*TODO fix stab (clamp mouse angle?)
    private Vector3 FocusTarget()
    {
        Vector3 focusPos = new Vector3(camHolder.position.x, camHolder.position.y, camHolder.position.z);
        focusPos += cam.forward * stabAmount; //stabAmount => distance of focal point for camera. Higher = More stable
        return focusPos;
    }
    */
}

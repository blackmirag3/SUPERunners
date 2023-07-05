using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private PlayerCamSettings settings;

    public WallRunning wallRunning;
    public PlayerMovement playerMovement;
    public PlayerSound playerSound;

    [SerializeField] private float sens; 
    public float mouseY, mouseX;

    public Transform orientation;
    [HideInInspector] public float xRotation;
    [HideInInspector] public float yRotation;

    private Camera cam;

    private float baseFOV;
    //[SerializeField] private float sprintFOV;
    private float targetFOV;

    private bool isBobEnabled;
    private float bobAmplitude;
    public float bobFrequency;
    private Vector3 startPos;
    private float stabAmount;

    public PlayerMovement player;
    public Transform camHolder;

    private bool canFootstep;

    private bool isPaused;

    //private float velocity = 10000f;

    private void InitialiseSettings()
    {
        isBobEnabled = settings.isBobEnabled;
        bobAmplitude = settings.bobAmplitude;
        bobFrequency = settings.bobFrequency;
        stabAmount = settings.stabAmount;
    }

    private void Awake()
    {
        InitialiseSettings();
    }

    private void Start()
    {
        isPaused = false;
        startPos = transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
        baseFOV = cam.fieldOfView;
        targetFOV = baseFOV;

        StartCoroutine(EnsureBaseFovOnStart());
    }

    private IEnumerator EnsureBaseFovOnStart()
    {
        yield return null;
        baseFOV = cam.fieldOfView;
    }

    private void Update()
    {
        if (!isPaused)
        {
            mouseX = Input.GetAxisRaw("Mouse X") * sens;
            mouseY = Input.GetAxisRaw("Mouse Y") * sens;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRunning.Tilt);
            orientation.rotation = Quaternion.Euler(0, yRotation, wallRunning.Tilt);

            fovUpdate();
            PlayMotion();
            ResetPosition();
            //cam.LookAt(FocusTarget());
        }
        
    }

    public void PauseCalled(Component sender, object data)
    {
        if (data is bool)
        {
            isPaused = (bool)data;
            return;
        }
        Debug.Log($"Unwanted event call from {sender}");
    }

    private void fovUpdate()
    {
        if (playerMovement.isSliding || playerMovement.isWallRunning)
        {
            targetFOV = baseFOV + 15f;
        }
        else if (playerMovement.isSprinting && !playerMovement.isWallRunning && !playerMovement.isCrouching && player.hasMovementInputs)
        {
            targetFOV = baseFOV + 10f;
        }
        else if ((!playerMovement.hasMovementInputs || !playerMovement.isSliding) && !playerMovement.isWallRunning)
        {
            targetFOV = baseFOV;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, 10f * Time.deltaTime);
    }

    private void PlayMotion()
    {
        if (isBobEnabled && (player.isGrounded || player.isWallRunning) && player.hasMovementInputs && !player.isSliding)
        {
            float sine = Mathf.Sin(Time.time * bobFrequency);
            playerSound.HandleFootstep(sine);
            transform.localPosition += transform.up * sine * 0.0008f;
            transform.localPosition += transform.right * Mathf.Cos(Time.time * bobFrequency / 2) * 0.0008f / 2f;
        }
    }

    private void ResetPosition()
    {
        if (transform.localPosition != startPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
        }
    }

    public void CameraFovChanged(Component sender, object data)
    {
        if (data is float)
        {
            baseFOV = (float)data;
            targetFOV = baseFOV;
            return;
        }
        Debug.LogWarning($"Unwanted event call from {sender}");
    }

    public void SensitivityChanged(Component sender, object data)
    {
        if (data is float)
        {
            sens = (float)data;
            return;
        }
        Debug.LogWarning($"Unwanted event call from {sender}");
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

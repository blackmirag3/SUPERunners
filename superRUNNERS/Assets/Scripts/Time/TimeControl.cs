using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class TimeControl : MonoBehaviour
{
    private float initialFixedDeltaTime;
    public Rigidbody playerBody;
    public GameObject playerCam;
    public AudioMixer audioMixer;
    [SerializeField]
    private AudioSource warpSound;
    private bool isPlaying;
    private bool isTempAction;

    public bool isShifting;
    public float slowedTimeRatio = 0.5f;
    public float normalTimeRatio = 1f;
    private AudioLowPassFilter audioLowPassFilter;
    private AudioEchoFilter audioEchoFilter;

    // Time resumes for a split second on player action call
    private bool isPlayerAction;
    [SerializeField] private float defaultActionTime;

    private bool gamePaused;

    private InputAction moveInput = null;

    private void Awake()
    {
        moveInput = InputManager.instance.PlayerInput.actions["Move"];
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        gamePaused = false;
        audioLowPassFilter = playerCam.GetComponent<AudioLowPassFilter>();
        audioEchoFilter = playerCam.GetComponent<AudioEchoFilter>();

        normalTimeRatio = Time.timeScale;
        //initialFixedDeltaTime = Time.fixedDeltaTime;
        
        isShifting = false;
        NormalTime();
    }

    private void Update()
    {
        if (!gamePaused)
        {
            Vector2 inputValues = moveInput.ReadValue<Vector2>();
            float xInput = inputValues.x;
            float zInput = inputValues.y;

            float newTime = (xInput != 0 || zInput != 0) ? normalTimeRatio : slowedTimeRatio;
            float lerpTime = (xInput != 0 || zInput != 0) ? 0.05f : 0.5f;

            newTime = isPlayerAction ? normalTimeRatio : newTime;
            lerpTime = isPlayerAction ? 0.1f : lerpTime;

            Time.timeScale = Mathf.Lerp(Time.timeScale, newTime, lerpTime);
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;

            if (newTime == normalTimeRatio)
            {
                NormalTime();
            }
            else if (newTime == slowedTimeRatio)
            {
                WarpTime();
            }
        }
    }

    private void WarpTime()
    {
        audioLowPassFilter.enabled = true;
        audioEchoFilter.enabled = true;
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("Pitch", slowedTimeRatio);
        if (!isPlaying)
        {
            warpSound.Play();
            isPlaying = true;
        }
    }

    private void NormalTime()
    {
        audioLowPassFilter.enabled = false;
        audioEchoFilter.enabled = false;
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("Pitch", normalTimeRatio);
        if (isPlaying && !isTempAction)
        {
            warpSound.Stop();
            isPlaying = false;
        }
    }

    public void PauseCalled(Component sender, object data)
    {
        if (data is bool)
        {
            gamePaused = (bool)data;
            Time.fixedDeltaTime = initialFixedDeltaTime;
            return;
        }
        Debug.Log($"Unwanted event call from {sender}");
    }

    public void ActionCalled(Component sender, object data)
    {
        float actionTime = defaultActionTime;
        if (data is float && (float)data != 0)
        {
            actionTime = (float)data;
        }
        StartCoroutine(AllowActionTime(actionTime));
    }

    private IEnumerator AllowActionTime(float time)
    {
        //Debug.Log("Time temporarily resumed");
        isPlayerAction = true;
        isTempAction = true;
        yield return new WaitForSecondsRealtime(time);
        isPlayerAction = false;
        isTempAction = false;
    }

    private void OnDisable()
    {
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("Pitch", normalTimeRatio);
    }
}

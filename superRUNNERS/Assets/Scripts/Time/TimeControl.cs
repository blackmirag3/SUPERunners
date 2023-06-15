using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TimeControl : MonoBehaviour
{ 
    private float initialFixedDeltaTime;
    public Rigidbody playerBody;
    public GameObject playerCam;
    public AudioMixer audioMixer;

    public bool isShifting;
    public float timeShiftRatio = 0.5f;
    public float normalTimeRatio = 1f;
    private AudioLowPassFilter audioLowPassFilter;
    private AudioEchoFilter audioEchoFilter;

    private bool timeSlowed;

    private bool gamePaused;

    // Start is called before the first frame update
    private void Start()
    {
        gamePaused = false;
        timeSlowed = false;
        audioLowPassFilter = playerCam.GetComponent<AudioLowPassFilter>();
        audioEchoFilter = playerCam.GetComponent<AudioEchoFilter>();

        normalTimeRatio = Time.timeScale;
        initialFixedDeltaTime = Time.fixedDeltaTime;
        isShifting = false;
        NormalTime();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gamePaused)
        {
            if (timeSlowed && playerBody.velocity.magnitude > 0.1f)
            {

                NormalTime();
                timeSlowed = false;
                
            }
            else if (playerBody.velocity.magnitude < 0.1f && !timeSlowed)
            {
                WarpTime();
                timeSlowed = true;
            }
        }
    }

    private void TimeShift()
    {
        if (isShifting)
            WarpTime();
        else
            NormalTime();
    }

    private void WarpTime()
    {
        Time.timeScale = timeShiftRatio;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        audioLowPassFilter.enabled = true;
        audioEchoFilter.enabled = true;
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("MasterPitch", timeShiftRatio);
    }

    private void NormalTime()
    {
        Time.timeScale = normalTimeRatio;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        audioLowPassFilter.enabled = false;
        audioEchoFilter.enabled = false;
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("MasterPitch", normalTimeRatio);
    }

    private bool CheckInput()
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                return false;
            }
            return true;
        }

        return false;
    }

    public void PauseCalled(Component sender, object data)
    {
        if (data is bool)
        {
            gamePaused = (bool)data;
            return;
        }
        Debug.Log($"Unwanted event call from {sender}");
    }
}

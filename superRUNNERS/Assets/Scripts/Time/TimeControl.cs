using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    private float initialFixedDeltaTime;
    private float playerVelocity;
    public GameObject playerCam;
    public AudioMixer audioMixer;

    public bool isShifting;
    public float timeShiftRatio = 0.5f;
    public float normalTimeRatio = 1f;
    private AudioLowPassFilter audioLowPassFilter;
    private AudioEchoFilter audioEchoFilter;

    // Start is called before the first frame update
    private void Start()
    {
        audioLowPassFilter = playerCam.GetComponent<AudioLowPassFilter>();
        audioEchoFilter = playerCam.GetComponent<AudioEchoFilter>();
        initialFixedDeltaTime = Time.fixedDeltaTime;
        isShifting = false;
        NormalTime();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(isShifting);
        if (!pauseMenu.gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isShifting ^= true;
                TimeShift();
            }
        }
    }

    void TimeShift()
    {
        if (isShifting)
            WarpTime();
        else
            NormalTime();
    }

    void WarpTime()
    {
        Time.timeScale = timeShiftRatio;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        audioLowPassFilter.enabled = true;
        audioEchoFilter.enabled = true;
        audioMixer.SetFloat("MasterPitch", timeShiftRatio);
    }

    void NormalTime()
    {
        Time.timeScale = normalTimeRatio;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        audioLowPassFilter.enabled = false;
        audioEchoFilter.enabled = false;
        audioMixer.SetFloat("MasterPitch", normalTimeRatio);
    }
}

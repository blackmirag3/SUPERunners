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
    public float slowedTimeRatio = 0.5f;
    public float normalTimeRatio = 1f;
    private AudioLowPassFilter audioLowPassFilter;
    private AudioEchoFilter audioEchoFilter;

    //private bool timeSlowed;

    // Time resumes for a split second on player action call
    private bool isPlayerAction;
    [SerializeField] private float defaultActionTime;

    private bool gamePaused;

    // Start is called before the first frame update
    private void Start()
    {
        gamePaused = false;
        //timeSlowed = false;
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
            float xInput = Input.GetAxisRaw("Horizontal");
            float zInput = Input.GetAxisRaw("Vertical");

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

    private void TimeShift()
    {
        if (isShifting)
            WarpTime();
        else
            NormalTime();
    }

    private void WarpTime()
    {
        //Time.timeScale = timeShiftRatio;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        audioLowPassFilter.enabled = true;
        audioEchoFilter.enabled = true;
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("MasterPitch", slowedTimeRatio);
    }

    private void NormalTime()
    {
        //Time.timeScale = normalTimeRatio;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        audioLowPassFilter.enabled = false;
        audioEchoFilter.enabled = false;
        if (audioMixer == null)
        {
            return;
        }
        audioMixer.SetFloat("MasterPitch", normalTimeRatio);
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
        Debug.Log("Time temporarily resumed");
        isPlayerAction = true;
        yield return new WaitForSecondsRealtime(time);
        isPlayerAction = false;
    }
}

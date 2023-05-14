using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private TimeSettings settings;

    public float timeSens; //sensitivity of time shift
    public float scaleLimit; //0.05f
    public float velocityThreshold; //time decreases from this value and lower

    public PlayerMovement player;

    private float initialFixedDeltaTime;
    private float playerVelocity;

    public bool isShifting;
    public bool toggleShift = false;

    private void InitializeSettings()
    {
        timeSens = settings.timeSens;
        scaleLimit = settings.scaleLimit;
        velocityThreshold = settings.velocityThreshold;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeSettings();
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = player.currVelocityMagnitude;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            toggleShift ^= true;
        }
        
        if (toggleShift)
        {
            ActiveTimeShift();
        }
        else
        {
            AutoTimeShift();
        }
    }

    void AutoTimeShift() //player.isWASD
    {
        if (playerVelocity < velocityThreshold)
        {
            //float relativeVelocity = Mathf.Pow(playerVelocity / velocityThreshold, timeSens);
            float relativeVelocity = playerVelocity / velocityThreshold;
            Time.timeScale = Mathf.Clamp(relativeVelocity * timeSens, scaleLimit, 1f);
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
            isShifting = true;
        }
        else
        {
            isShifting = false;
        }
    }

    void ActiveTimeShift()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        isShifting = true;
    }
}

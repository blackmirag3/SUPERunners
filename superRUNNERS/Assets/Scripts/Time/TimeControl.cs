using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    private float initialFixedDeltaTime;
    private float playerVelocity;

    public bool isShifting = false;
    public float timeShiftRatio = 0.5f;
    public float normalTime = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    private void Update()
    {
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
        {
            Time.timeScale = timeShiftRatio;
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        }
        else
        {
            Time.timeScale = normalTime;
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        }
    }
}

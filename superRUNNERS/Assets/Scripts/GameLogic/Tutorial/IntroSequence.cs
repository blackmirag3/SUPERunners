using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    public GameObject input;
    public CanvasGroup HUD;
    public GameObject startScreen;
    public GameObject firstSequence;
    public int delayTime;
    private int timer;
    //public GameEvent enableWaypoint;

    private void Start()
    {
        if (input != null) input.SetActive(false);
        if (HUD != null) HUD.alpha = 0f;
        timer = delayTime;
        StartCoroutine(Sequence());
    }
    private IEnumerator Sequence()
    {
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSecondsRealtime(1);
        }
        //enableWaypoint.CallEvent(this, null);
        firstSequence.SetActive(true);

        while (HUD.alpha < 1f)
        {
            HUD.alpha += Time.unscaledDeltaTime;
            yield return null;
        }
        input.SetActive(true);
        startScreen.SetActive(false);
    }
}

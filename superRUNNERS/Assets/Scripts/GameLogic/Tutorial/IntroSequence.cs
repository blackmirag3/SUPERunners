using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    public GameObject input;
    public CanvasGroup HUD;
    public GameObject startScreen;
    private CanvasGroup screenCanvas;
    public GameObject startText;
    public GameObject firstSequence;
    public int delayBeforeText;
    public int delayAfterText;
    private int timer;
    //public GameEvent enableWaypoint;

    private void Start()
    {
        if (startScreen != null) screenCanvas = startScreen.GetComponent<CanvasGroup>();
        if (input != null) input.SetActive(false);
        if (HUD != null) HUD.alpha = 0f;
        StartCoroutine(Sequence());
    }
    private IEnumerator Sequence()
    {
        timer = delayBeforeText;
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSecondsRealtime(1);
        }

        timer = delayAfterText;
        startText.SetActive(true);
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSecondsRealtime(1);
        }

        firstSequence.SetActive(true);
        while (HUD.alpha < 1f || screenCanvas.alpha > 0f)
        {
            screenCanvas.alpha -= Time.unscaledDeltaTime;
            HUD.alpha += Time.unscaledDeltaTime;
            yield return null;
        }
        input.SetActive(true);
        startScreen.SetActive(false);
    }
}

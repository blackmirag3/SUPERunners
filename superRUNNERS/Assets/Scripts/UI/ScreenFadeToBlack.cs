using System;
using System.Collections;
using UnityEngine;

public class ScreenFadeToBlack : MonoBehaviour {
    public int fadeDelay;
    private int timer;
    private bool isDelay;
    private CanvasGroup canvasGroup = null;
    public CanvasGroup CanvasGroup     
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private RectTransform rect = null;
    public  RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    private void Start()
    {
        timer = fadeDelay;
        if (timer > 0) isDelay = true;
        StartTimer();
    }
    private void StartTimer()
    {
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        if (isDelay)
        {
            while (timer > 0)
            {
                timer--;
                yield return new WaitForSecondsRealtime(1);
            }
        }
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 2* Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
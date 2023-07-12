using System;
using System.Collections;
using UnityEngine;

public class HitIndicator : MonoBehaviour {
    //public float indicatorTime;
    private float timer;
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
        timer = 1;
        StartTimer();
    }

    private void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        /*
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        */
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        while (CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 5 * Time.unscaledDeltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
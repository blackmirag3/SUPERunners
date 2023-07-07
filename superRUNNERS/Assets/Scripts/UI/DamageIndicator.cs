using System;
using System.Collections;
using UnityEngine;

public class DamageIndicator : MonoBehaviour {
    public float indicatorTime;
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

    public Vector3 damageSource;
    [SerializeField] private GameObject playerCam;

    Vector3 direction = Vector3.zero;
    Quaternion rotation = Quaternion.identity; 

    private void Start()
    {
        if (playerCam == null) playerCam = GameObject.Find("PlayerCam");

        timer = indicatorTime;
        StartTimer();
    }

    private void Update()
    {
        RotateToTheTarget();
    }

    private void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    private void RotateToTheTarget()
    {
        direction = playerCam.transform.position - damageSource;
        rotation = Quaternion.LookRotation(direction);
        rotation.z = -rotation.y;
        rotation.x = 0;
        rotation.y = 0;

        Vector3 northDirection = new Vector3(0, 0, playerCam.transform.eulerAngles.y);
        Rect.localRotation = rotation * Quaternion.Euler(northDirection);
    }
    private IEnumerator Countdown()
    {
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }
        while (CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
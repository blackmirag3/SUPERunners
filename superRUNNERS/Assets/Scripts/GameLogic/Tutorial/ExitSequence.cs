using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSequence : MonoBehaviour
{
    [SerializeField] private string PlayerTag;
    public GameObject input;
    public CanvasGroup HUD;
    public GameObject wayPoint;
    public GameObject endScreen;
    public int delayTime;
    private int timer;
    //public GameEvent enableWaypoint;

    private void Start()
    {
        if (wayPoint != null) wayPoint.SetActive(false);
        timer = delayTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            StartCoroutine(Sequence());
        }
    }
    private IEnumerator Sequence()
    {
        if (input != null) input.SetActive(false);

        while (timer > 0)
        {
            timer--;
            yield return new WaitForSecondsRealtime(1);
        }
        endScreen.SetActive(true);

        while (HUD.alpha > 0f)
        {
            HUD.alpha -= Time.unscaledDeltaTime;
            yield return null;
        }
    }
}

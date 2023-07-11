using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitSequence : MonoBehaviour
{
    [SerializeField] private string PlayerTag;
    [SerializeField] private GameObject endingText;
    [SerializeField] private GameObject continueText;
    public GameObject input;
    public CanvasGroup HUD;
    public GameObject wayPoint;
    public GameObject endScreen;
    private CanvasGroup screenCanvas;
    public int delayTime;
    private int timer;
    //public GameEvent enableWaypoint;
    private bool isFinished;

    private void Start()
    {
        if (endScreen != null) screenCanvas = endScreen.GetComponent<CanvasGroup>();
        if (wayPoint != null) wayPoint.SetActive(false);
        timer = delayTime;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (isFinished && Input.anyKey) SceneManager.LoadScene(0);
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
        input.SetActive(false);

        while (timer > 0)
        {
            timer--;
            yield return new WaitForSecondsRealtime(1);
        }
        endScreen.SetActive(true);
    
        while (HUD.alpha > 0f || screenCanvas.alpha < 1f)
        {
            HUD.alpha -= Time.unscaledDeltaTime;
            screenCanvas.alpha += 0.5f * Time.unscaledDeltaTime;
            yield return null;
        }
        endingText.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        isFinished = true;
        input.SetActive(true);
        continueText.SetActive(true);
    }
}

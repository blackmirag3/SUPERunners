using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStartWithDelay : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    [SerializeField]
    float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer(time));
    }

    private IEnumerator Timer(float time)
    {
        while (time > 0)
        {
        time --;
        yield return new WaitForSeconds(1);
        }
        obj.SetActive(true);
        yield return null;
    }
}

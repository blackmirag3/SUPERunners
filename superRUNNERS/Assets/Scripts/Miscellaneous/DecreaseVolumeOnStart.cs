using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseVolumeOnStart : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    void Update()
    {
        audio.volume -= Time.deltaTime;
    }
}
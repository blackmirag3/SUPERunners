using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseVolumeOnStart : MonoBehaviour
{
    [SerializeField]
    private AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    void Update()
    {
        sound.volume -= Time.deltaTime;
    }
}
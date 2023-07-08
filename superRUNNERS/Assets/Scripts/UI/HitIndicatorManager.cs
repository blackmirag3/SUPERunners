using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicatorManager : MonoBehaviour
{
    //private Transform Player;
    [SerializeField] private GameObject hitIndicator; //indicator prefab
    [SerializeField] private Transform screen; //parent of indicator
    //[SerializeField] private float indicatorTime;

    void Start()
    {
        screen = transform;
    }

    public void SpawnIndicator(Component sender, object data)
    {
        GameObject newIndicator = Instantiate(hitIndicator, screen);
        //HitIndicator indicator = newIndicator.GetComponent<HitIndicator>();
        //indicator.damageSource = damageSource;
        //indicator.Player = Player;
        //indicator.indicatorTime = indicatorTime;
    }
}

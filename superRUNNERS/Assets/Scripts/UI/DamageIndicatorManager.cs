using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    //private Transform Player;
    [SerializeField] private GameObject damageIndicator; //indicator prefab
    [SerializeField] private Transform screen; //parent of indicator
    [SerializeField] private float indicatorTime;


    // Start is called before the first frame update
    void Start()
    {
        //Player = GetComponentInParent<Transform>();
    }

    public void SpawnIndicator(Vector3 damageSource)
    {
        GameObject newIndicator = Instantiate(damageIndicator, screen);
        DamageIndicator indicator = newIndicator.GetComponent<DamageIndicator>();
        indicator.damageSource = damageSource;
        //indicator.Player = Player;
        indicator.indicatorTime = indicatorTime;
    }
}

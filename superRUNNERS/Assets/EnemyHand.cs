using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    public Collider handHitbox;
    // Start is called before the first frame update
    void Start()
    {
        handHitbox = GetComponent<Collider>();
        handHitbox.enabled = true;
        handHitbox.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enablePunch()
    {
        handHitbox.enabled = true;
    }
    public void disablePunch()
    {
        handHitbox.enabled = false;
    }
}

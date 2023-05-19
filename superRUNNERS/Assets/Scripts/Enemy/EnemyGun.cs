using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD THIS SCRIPT TO ENEMY GUN gameobject! (for enemy behaviour to work)

public class EnemyGun : MonoBehaviour
{
    public EnemyGunData gunData;
    public GameObject currBullet;
    public AudioSource shootingSound;
    public Transform bulletPos, playerPos;

    public float bulletVelocity = 20f;
    public float inaccuracy = 1f;

    //TODO fire different gun functions + stats, based on player gun script or 

    // Start is called before the first frame update
    void Start()
    {
        InitialiseGunData();
    }

    // Update is called once per frame
    void Update()
    {
            //InitialiseSettings
    }

    public void FireBullet()
    {
        
        GameObject newBullet = Instantiate(currBullet, bulletPos.position, Quaternion.identity);
        //PrefabUtility.InstantiatePrefab
        Vector3 shootDir = (playerPos.position - bulletPos.position + Random.Range(-inaccuracy,inaccuracy) * Vector3.up).normalized;
        newBullet.transform.forward = shootDir;
        newBullet.GetComponent<Rigidbody>().AddForce(shootDir * bulletVelocity, ForceMode.Impulse);

        // newBullet.GetComponent<Rigidbody>().velocity = bulletPos.forward * bulletVelocity;
        // Destroy(newBullet, 5); //TODO bullet despawn   
        shootingSound.Play(); //TODO improve shooting...? based on weapon type too?
    }

    private void InitialiseGunData()
    {

    }

}
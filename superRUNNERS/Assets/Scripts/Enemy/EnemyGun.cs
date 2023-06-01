using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD THIS SCRIPT TO ENEMY GUN gameobject! (for enemy behaviour to work)

public class EnemyGun : MonoBehaviour
{
    public GunData gunData;
    public GameObject currBullet;
    public AudioSource shootingSound;
    public Transform bulletPos, playerPos;

    private float inaccuracy;
    public float maxShotDelay;
    public float minShotDelay;
    public float maxBurstSize;
    private float bulletVelocity;
    public float fireRate;

    private float bulletsShot;
    public float bulletsPerShot;


    //TODO fire different gun functions + stats, based on player gun script or 

    // Start is called before the first frame update
    void Awake()
    {
        InitialiseGunData();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Shoot()
    {
        SpawnOneBullet();
        shootingSound.Play();
        bulletsShot = 0;
    }

    public void SpawnOneBullet()
    {
        float x = Random.Range(-inaccuracy, inaccuracy);
        float y = Random.Range(-inaccuracy, inaccuracy);
        float z = Random.Range(-inaccuracy, inaccuracy);
        Vector3 shotDir = playerPos.position - bulletPos.position + new Vector3(x, y, z);
        GameObject newBullet = Instantiate(currBullet, bulletPos.position, Quaternion.identity);
        newBullet.transform.forward = shotDir.normalized;
        newBullet.GetComponent<Rigidbody>().AddForce(shotDir.normalized * bulletVelocity, ForceMode.Impulse);
        bulletsShot += 1f;
        if (bulletsShot < bulletsPerShot)
            SpawnOneBullet();
    }

    private void InitialiseGunData()
    {
        inaccuracy = gunData.enemyInaccuracy;
        bulletVelocity = gunData.enemyBulletVelocity;
        //for enemy behaviour
        maxShotDelay = gunData.enemyMaxShotDelay;
        maxBurstSize = gunData.enemyMaxBurstSize;
        fireRate = gunData.fireRate;
        bulletsPerShot = gunData.bulletsPerShot;
        minShotDelay = gunData.enemyMinShotDelay;
    }
}
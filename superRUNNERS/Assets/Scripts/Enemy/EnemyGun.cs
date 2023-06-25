using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD THIS SCRIPT TO ENEMY GUN gameobject! (for enemy behaviour to work)

public class EnemyGun : MonoBehaviour
{
    public EnemyBehaviourSettings enemyBehaviourSettings;
    public GameObject currBullet;
    public AudioSource shootingSound;
    public Transform bulletPos, playerPos;

    private float inaccuracy;
    public float maxShotDelay;
    public float minShotDelay;
    public int maxBurstSize;
    private float bulletVelocity;
    public float fireRate;

    private int bulletsShot;
    public int bulletsPerShot;


    //TODO fire different gun functions + stats, based on player gun script or 

    // Start is called before the first frame update
    void Awake()
    {
        InitialiseSettings();
    }

    public void Shoot()
    {
        SpawnOneBullet();
        shootingSound.Play();
        bulletsShot = 0;
    }

    public void SpawnOneBullet()
    {
        Vector3 shotDir = playerPos.position - bulletPos.position;
        float x = Random.Range(-inaccuracy, inaccuracy);
        float y = Random.Range(-inaccuracy, inaccuracy);
        float z = Random.Range(-inaccuracy, inaccuracy);
        Vector3 shotDirSpread = shotDir.normalized + new Vector3(x, y, z);
        shotDirSpread = shotDirSpread.normalized;
        GameObject newBullet = Instantiate(currBullet, bulletPos.position, Quaternion.identity);
        newBullet.transform.forward = shotDirSpread;
        newBullet.GetComponent<Rigidbody>().AddForce(shotDirSpread * bulletVelocity, ForceMode.Impulse);
        bulletsShot += 1;
        if (bulletsShot < bulletsPerShot)
            SpawnOneBullet();
    }

    private void InitialiseSettings()
    {
        inaccuracy = enemyBehaviourSettings.enemyInaccuracy;
        bulletVelocity = enemyBehaviourSettings.enemyBulletVelocity;
        bulletsPerShot = enemyBehaviourSettings.bulletsPerShot;
    }
}
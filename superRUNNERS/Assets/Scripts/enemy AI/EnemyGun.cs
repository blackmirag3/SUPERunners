using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD THIS SCRIPT TO ENEMY GUN gameobject! (for enemy behaviour to work)

public class EnemyGun : MonoBehaviour
{
    public GameObject currBullet;
    public Transform bulletPos;
    public Quaternion bulletRot;
    public float bulletVelocity = 20f; //TODO get gun velocity
    public AudioSource shootingSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            
    }
    //TODO fire different gun functions?
    public void FireBullet()
    {

        //GameObject newBullet = Instantiate(currBullet, bulletPos.position, Quaternion.identity);
        //PrefabUtility.InstantiatePrefab
        GameObject newBullet = Instantiate(currBullet);
        newBullet.transform.position = bulletPos.position;
        newBullet.GetComponent<Rigidbody>().velocity = bulletPos.forward * bulletVelocity;
        Destroy(newBullet, 5); //TODO bullet despawn   
        shootingSound.Play(); //TODO improve shooting...? based on weapon type too?
    }
}
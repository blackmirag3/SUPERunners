using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{

    public EnemyBehaviour enemy;
    public GameObject gunType;
    public Transform enemyHand;

    private void DropGun()
    {
        GameObject playerGun = Instantiate(gunType, enemyHand.position, Quaternion.identity);

        enemyHand.GetChild(0).gameObject.SetActive(false);
        Rigidbody gunRb = playerGun.GetComponent<Rigidbody>();

        Vector3 dir = enemy.player.position - playerGun.transform.position;
        float h = dir.y;
        dir.y = 0;
        float distance = dir.magnitude;
        float a = 45 * Mathf.Deg2Rad;
        dir.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        gunRb.velocity = velocity * dir.normalized;
        float spin = Random.Range(-1f, 1f);
        gunRb.AddTorque(new Vector3(spin, spin, spin));
    }

    private void OnEnable()
    {
        if (gunType != null)
        {
            DropGun();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDrop : MonoBehaviour
{
    public EnemyBehaviour enemy;
    public GameObject gunType;
    public Transform enemyHand;

    private void DropGun()
    {
        GameObject PlayerGun = Instantiate(gunType, enemyHand.position, Quaternion.identity, transform.parent.parent);

        Destroy(enemyHand.GetChild(0).gameObject);
        Rigidbody gunRb = PlayerGun.GetComponent<Rigidbody>();
        ColIgnore(PlayerGun.GetComponent<Collider>());

        Vector3 dir = enemy.Player.position - PlayerGun.transform.position;
        float h = dir.y;
        dir.y = 0;
        float distance = dir.magnitude;
        float a = 45 * Mathf.Deg2Rad;
        dir.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(Mathf.Abs(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a)));
        gunRb.velocity = dir.normalized * velocity;
        float spin = Random.Range(-10f, 10f);
        gunRb.AddTorque(new Vector3(spin, spin, spin));
    }
    
    private void ColIgnore(Collider col)
    {
        Collider[] enemyColls = GetComponentsInChildren<Collider>();
        foreach (Collider enemyCol in enemyColls)
        {
            Physics.IgnoreCollision(col, enemyCol);
        }
    }

    private void OnEnable()
    {
        if (gunType != null)
        {
            DropGun();
        }
    }

}

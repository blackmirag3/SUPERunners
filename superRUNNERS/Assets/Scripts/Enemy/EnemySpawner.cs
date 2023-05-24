using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDist;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int enemyPerSpawn;
    private float timer = 0;

    // For testing
    public bool enableSpawn = true;
    public KillCounter kill;

    public Transform player;
    public GameObject enemy;
    private float currDistFromPlayer;
    private bool canSpawn = true;

    private void Start()
    {
        
    }

    private void Update()
    {
        enableSpawn = !kill.LevelWon;
        if (enableSpawn)
        {
            if (!canSpawn)
            {
                Timer();
            }
            else
            {
                currDistFromPlayer = Vector3.Distance(player.position, transform.position);
                if (currDistFromPlayer > spawnDist)
                {
                    StartCoroutine(SpawnEnemy(spawnDelay, enemy, enemyPerSpawn));
                    canSpawn = false;
                }
            }
        }
    }

    private void Timer()
    {
        if (timer < spawnTimer)
        {
            timer += Time.deltaTime;
        }
        else
        {
            canSpawn = true;
            timer = 0;
        }
    }

    IEnumerator SpawnEnemy(float delay, GameObject enemy, int spawnsLeft)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(enemy, transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Quaternion.identity);
        spawnsLeft--;
        if (spawnsLeft != 0)
        {
            StartCoroutine(SpawnEnemy(delay, enemy, spawnsLeft));
        }
    }
}

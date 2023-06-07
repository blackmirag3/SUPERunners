using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float startX, startZ, endX, endZ;
    private float gridSize, cellSize;
    public float y;

    public WaveFunctionCollapse wave;

    public int enemyPerFloor;
    public LayerMask ground;

    public GameObject[] enemyType;

    private void Start()
    {
        gridSize = wave.gridSize;
        cellSize = wave.cellLength;

        startX = wave.startPos.x - (0.5f * cellSize);
        startZ = wave.startPos.z - (0.5f * cellSize);

        endX = startX + cellSize * gridSize;
        endZ = startZ + cellSize * gridSize;
    }

    private void Update()
    {
        if (wave.testSpawn)
        {
            SpawnEnemies(enemyPerFloor);
            wave.testSpawn = false;
        }
    }

    private void SpawnEnemies(int enemyCount)
    {
        int enemiesSpawned = 0;
        byte timeOut = 0;
        while (enemiesSpawned < enemyCount && timeOut < byte.MaxValue)
        {
            float xPos = Random.Range(startX, endX);
            float zPos = Random.Range(startZ, endZ);
            Vector3 spawnPos = new Vector3(xPos, y, zPos);
            Ray ray = new Ray(spawnPos, Vector3.down);

            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, y + 1f, ground))
            {
                int selectEnemy = Random.Range(0, enemyType.Length);

                GameObject newEnemy = Instantiate(enemyType[selectEnemy], hit.point, Quaternion.identity, transform);
                newEnemy.SetActive(true);
                enemiesSpawned++;
                timeOut = 0;
                Debug.Log("Spawned");
            }
            else
            {
                timeOut++;
                Debug.Log($"Not spawning, pos = {spawnPos}");
            }

        }
    }
}

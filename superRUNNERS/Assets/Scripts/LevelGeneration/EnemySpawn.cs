using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private float startX, startZ, endX, endZ;
    private float gridSize, cellSize;
    [SerializeField]
    private float y;

    public WaveFunctionCollapse wave;

    public LayerMask ground;

    [SerializeField]
    private List<GameObject> enemySpawnPool = new List<GameObject>();

    [SerializeField] private GameObject pistolEnemy;
    [SerializeField] private GameObject meleeEnemy;
    [SerializeField] private GameObject rifleEnemy;
    [SerializeField] private GameObject shotgunEnemy;

    private byte phase;

    private void Awake()
    {
        enemySpawnPool.Clear();

        enemySpawnPool.Add(meleeEnemy);
        enemySpawnPool.Add(pistolEnemy);

        phase = 0;
    }

    private void Start()
    {
        gridSize = wave.gridSize;
        cellSize = wave.cellLength;

        startX = wave.startPos.x - (0.5f * cellSize);
        startZ = wave.startPos.z - (0.5f * cellSize);

        endX = startX + cellSize * gridSize;
        endZ = startZ + cellSize * gridSize;
    }

    public void SpawnEnemies(int enemyCount)
    {
        UpdateSpawnpoints();

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
                int selectEnemy = Random.Range(0, enemySpawnPool.Count);

                GameObject newEnemy = Instantiate(enemySpawnPool[selectEnemy], hit.point, Quaternion.identity, transform);
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

    private void UpdateSpawnpoints()
    {
        startX = wave.startPos.x - (0.5f * cellSize);
        startZ = wave.startPos.z - (0.5f * cellSize);

        endX = startX + cellSize * gridSize;
        endZ = startZ + cellSize * gridSize;
    }

    public void UpdateSpawnPool(Component sender, object data)
    {
        phase++;

        if (phase > 5)
        {
            return;
        }

        switch (phase)
        {
            case 1:
                enemySpawnPool.Add(rifleEnemy);
                break;
            case 2:
                enemySpawnPool.Add(pistolEnemy);
                enemySpawnPool.Add(rifleEnemy);
                break;
            case 3:
                enemySpawnPool.Add(shotgunEnemy);
                break;
            case 4:
                enemySpawnPool.Add(shotgunEnemy);
                break;
        }
    }
}

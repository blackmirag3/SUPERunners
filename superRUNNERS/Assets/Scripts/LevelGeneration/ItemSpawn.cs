using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    private float startX, startZ, endX, endZ;
    private float gridSize, cellSize;
    [SerializeField]
    private float y; 
    [SerializeField]
    private int minItems, maxItems;

    [SerializeField]
    private WaveFunctionCollapse wave;

    public LayerMask ground;

    [SerializeField]
    private GameObject[] items;

    private void Start()
    {
        gridSize = wave.gridSize;
        cellSize = wave.cellLength;

        startX = wave.startPos.x - (0.5f * cellSize);
        startZ = wave.startPos.z - (0.5f * cellSize);

        endX = startX + cellSize * gridSize;
        endZ = startZ + cellSize * gridSize;
    }

    private void UpdateSpawnpoints()
    {
        startX = wave.startPos.x - (0.5f * cellSize);
        startZ = wave.startPos.z - (0.5f * cellSize);

        endX = startX + cellSize * gridSize;
        endZ = startZ + cellSize * gridSize;
    }

    public void SpawnItems()
    {
        int itemsToSpawn = Random.Range(minItems, maxItems + 1);
        byte timeOut = 0;
        int itemsSpawned = 0;

        while (itemsSpawned < itemsToSpawn && timeOut < byte.MaxValue)
        {
            float xPos = Random.Range(startX, endX);
            float zPos = Random.Range(startZ, endZ);
            Vector3 spawnPos = new Vector3(xPos, y, zPos);
            Ray ray = new Ray(spawnPos, Vector3.down);

            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, y + 1f, ground))
            {
                int selectItem = Random.Range(0, items.Length);

                GameObject newEnemy = Instantiate(items[selectItem], hit.point, Quaternion.identity, transform);
                newEnemy.SetActive(true);
                itemsSpawned++;
                timeOut = 0;
            }
            else
            {
                timeOut++;
            }
        }
    }
}

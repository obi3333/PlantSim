using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PrefabSpawner))]
[RequireComponent(typeof(CactusMover))]
public class CactusController : MonoBehaviour
{
    public GameObject branchPrefab;
    public GameObject flowerPrefab;
    public GameObject thirdBranchPrefab;
    public GameObject fourthBranchPrefab;
    public float spawnProbability = 0.5f;
    public float moveUpAmount = 0.1f;
    public float moveDuration = 1f;
    public int maxMovesUp = 15;

    private PrefabSpawner prefabSpawner;
    private CactusMover cactusMover;
    private int movesUp = 0;

    void Start()
    {
        prefabSpawner = GetComponent<PrefabSpawner>();
        cactusMover = GetComponent<CactusMover>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            movesUp++;
            if (movesUp > maxMovesUp)
            {
                prefabSpawner.SpawnRandomBranch(thirdBranchPrefab, fourthBranchPrefab);
            }
            else
            {
                TrySpawnBranchOrFlower();
            }
        }
    }

    void TrySpawnBranchOrFlower()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < spawnProbability / 2)
        {
            prefabSpawner.SpawnPrefab(branchPrefab);
        }
        else if (randomValue < spawnProbability)
        {
            prefabSpawner.SpawnPrefab(flowerPrefab);
        }
        else
        {
            StartCoroutine(cactusMover.MoveCactusUpSmoothly(moveUpAmount, moveDuration));
        }
    }
}
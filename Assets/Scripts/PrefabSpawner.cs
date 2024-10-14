using System.Collections;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public float growthDuration = 1f;
    public Vector3 finalScale = new Vector3(0.3f, 0.3f, 0.3f);
    public Vector3 flowerScale = new Vector3(0.03f, 0.03f, 0.03f);
    public float distanceFromCenter = 0.5f;
    public float spawnDistance = 0.5f;

    private Renderer cactusRenderer;
    private PrefabGrowth prefabGrowth;

    void Start()
    {
        cactusRenderer = GetComponent<Renderer>();
        prefabGrowth = GetComponent<PrefabGrowth>();
    }

    public void SpawnPrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            Vector3 spawnPosition = GetRandomPointOutsideCactus();
            GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
            prefabGrowth.OrientAndGrowPrefab(instance, spawnPosition, growthDuration, finalScale, flowerScale);
        }
    }

    public void SpawnRandomBranch(GameObject thirdBranchPrefab, GameObject fourthBranchPrefab)
    {
        float randomValue = Random.Range(0f, 1f);
        GameObject prefabToSpawn = (randomValue < 0.5f) ? thirdBranchPrefab : fourthBranchPrefab;
        SpawnPrefab(prefabToSpawn);
    }

    Vector3 GetRandomPointOutsideCactus()
    {
        Bounds bounds = cactusRenderer.bounds;

        Vector3 randomPointOnSurface = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        Vector3 directionFromCenter = (randomPointOnSurface - bounds.center).normalized;

        float radius = bounds.extents.magnitude;
        Vector3 surfacePoint = bounds.center + directionFromCenter * (radius - distanceFromCenter);
        Vector3 spawnPoint = surfacePoint + directionFromCenter * spawnDistance;

        return spawnPoint;
    }
}
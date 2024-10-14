using UnityEngine;

public class LeafPlacer : MonoBehaviour
{
    public GameObject prefabToSpawn; // Prefab to instantiate
    public float spawnInterval = 2f; // Interval between spawning prefabs
    public float spawnOffset = 0.1f; // Offset from the line renderer to spawn prefabs
    public int maxLeaves = 2;
    private int prefabNumber = 0;
    private LineRenderer lineRenderer;
    private bool isSpawningEnabled = true;
    private float distanceTraveled = 0f;
    private float timeSinceLastSpawn = 0f;
    private int posCount;
    private int startCount = 0;
    public ObjectScaler objectScaler; // Reference to ObjectScaler

    void Start()
    {
        posCount = startCount;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (lineRenderer.positionCount < 2)
            return;

        distanceTraveled += Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 2), lineRenderer.GetPosition(lineRenderer.positionCount - 1));

        if (distanceTraveled >= spawnInterval && Input.GetKeyDown(KeyCode.Alpha1) && isSpawningEnabled)
        {
            // Calculate a random position along the line
            float t = Random.Range(-0.2f, -1f); // Modified range to spawn leaves only on the upper part of the stem
            Vector3 spawnPosition = Vector3.Lerp(lineRenderer.GetPosition(lineRenderer.positionCount - 2), lineRenderer.GetPosition(lineRenderer.positionCount - 1), t);

            // Add a random offset perpendicular to the line to spawnPosition
            Vector3 offsetDirection = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * (lineRenderer.GetPosition(lineRenderer.positionCount - 1) - lineRenderer.GetPosition(lineRenderer.positionCount - 2)).normalized;
            spawnPosition += offsetDirection * spawnOffset;

            // Random quaternion rotation
            Quaternion spawnRotation = Random.rotation;

            // Use ObjectScaler to instantiate and scale the prefab
            objectScaler.InstantiateAndScale(spawnPosition, spawnRotation);

            distanceTraveled = 0f;
            prefabNumber++;

            if (prefabNumber > maxLeaves)
            {
                isSpawningEnabled = false;
                posCount = lineRenderer.positionCount;
            }
        }
    }

    public void AddPosition(Vector3 position)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
        if (posCount < lineRenderer.positionCount)
        {
            isSpawningEnabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public GameObject dustPrefab; // Assign your dust prefab here
    public int numberOfDustParticles = 100; // Number of dust particles
    public float spawnRadius = 5f; // Radius around the camera to spawn dust particles
    public float windStrength = 0.1f; // Strength of the wind
    public float windFrequency = 0.5f; // Frequency of the wind changes

    private List<GameObject> dustParticles;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        dustParticles = new List<GameObject>();

        // Spawn dust particles
        for (int i = 0; i < numberOfDustParticles; i++)
        {
            Vector3 randomPos = mainCamera.transform.position + mainCamera.transform.forward * spawnRadius + Random.insideUnitSphere * spawnRadius;
            GameObject dust = Instantiate(dustPrefab, randomPos, Quaternion.identity);
            dustParticles.Add(dust);
        }
    }

    void Update()
    {
        foreach (GameObject dust in dustParticles)
        {
            if (dust == null) continue;

            // Simulate light wind by adding a small random force to each dust particle
            Vector3 wind = new Vector3(
                Mathf.PerlinNoise(Time.time * windFrequency, dust.transform.position.y) - 0.5f,
                Mathf.PerlinNoise(Time.time * windFrequency, dust.transform.position.x) - 0.5f,
                Mathf.PerlinNoise(Time.time * windFrequency, dust.transform.position.z) - 0.5f
            ) * windStrength;

            dust.transform.position += wind * Time.deltaTime;

            // Keep the dust particles within the spawn radius
            if ((dust.transform.position - mainCamera.transform.position).magnitude > spawnRadius * 2)
            {
                dust.transform.position = mainCamera.transform.position + mainCamera.transform.forward * spawnRadius + Random.insideUnitSphere * spawnRadius;
            }
        }
    }
}

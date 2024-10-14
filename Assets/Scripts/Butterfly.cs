using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    public float flightSpeed = 5.0f; // Speed of the butterfly's flight
    public float turnSpeed = 2.0f; // Speed at which the butterfly turns
    public float noiseScale = 0.1f; // Scale of the Perlin noise used for flight path
    public float verticalAmplitude = 1.0f; // Amplitude of vertical oscillation
    public float flightRadius = 10.0f; // Radius around the center point where the butterfly flies

    private Vector3 flightCenter; // The center point around which the butterfly flies
    private float noiseTime; // Time parameter for Perlin noise

    void Start()
    {
        // Set the initial flight center to the butterfly's starting position
        flightCenter = transform.position;
        noiseTime = Random.Range(0f, 100f); // Initialize the noise time with a random value
    }

    void Update()
    {
        // Update the noise time
        noiseTime += Time.deltaTime;

        // Calculate the new position using Perlin noise
        float noiseX = Mathf.PerlinNoise(noiseTime * noiseScale, 0) * 2 - 1;
        float noiseZ = Mathf.PerlinNoise(0, noiseTime * noiseScale) * 2 - 1;
        float noiseY = Mathf.Sin(noiseTime * verticalAmplitude);

        Vector3 targetPosition = flightCenter + new Vector3(noiseX, noiseY, noiseZ) * flightRadius;

        // Calculate the direction to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate the butterfly towards the target direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Move the butterfly forward
        transform.position += transform.forward * flightSpeed * Time.deltaTime;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRadius : MonoBehaviour
{
    public Transform target;        // The target object to orbit around
    public float orbitRadius = 5f;  // Radius of the orbit
    public float orbitSpeed = 1f;   // Speed of orbiting
    public float yOffset = 0f;      // Vertical offset from the target

    private Vector3 orbitPosition;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned for OrbitCamera script.");
            enabled = false;
        }

        // Initialize orbit position
        orbitPosition = CalculateOrbitPosition();
    }

    void Update()
    {
        // Calculate new orbit position
        Quaternion rotation = Quaternion.Euler(0, orbitSpeed * Time.deltaTime, 0);
        orbitPosition = rotation * (orbitPosition - target.position) + target.position;

        // Update camera position
        transform.position = orbitPosition + Vector3.up * yOffset;

        // Look at the target
        transform.LookAt(target.position + Vector3.up * yOffset);
    }

    Vector3 CalculateOrbitPosition()
    {
        // Calculate initial orbit position
        return (transform.position - target.position).normalized * orbitRadius + target.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineLeaf : MonoBehaviour
{
    public GameObject objectToPlace; // Prefab or GameObject to place along the complex shape
    public GameObject complexShape;  // Reference to the complex shape GameObject
    public int numberOfObjects = 10; // Number of objects to place around the shape
    public float distanceFromSurface = 0.1f; // Distance from the surface of the complex shape
    public float growSpeed = 1.0f; // Speed at which the object grows from scale 0 to final scale

    void Start()
    {
        // Ensure objectToPlace and complexShape are assigned
        
        if (objectToPlace == null || complexShape == null)
        {
            Debug.LogError("Please assign the object to place and the complex shape GameObject.");
            return;
        }
    }

    void Update()
    
    {
        // Check for key press to instantiate objects
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Check for key '2' press
        {
            PlaceSingleObject();
        }
    }

    void PlaceSingleObject()
    {
        // Calculate position around the complex shape
        Vector3 position = CalculatePosition();

        // Instantiate object at calculated position
        GameObject newObj = Instantiate(objectToPlace, position, Quaternion.identity, transform);

        // Start the scaling coroutine
        StartCoroutine(GrowObject(newObj.transform));
    }

    IEnumerator GrowObject(Transform objTransform)
    {
        float elapsedTime = 0f;
        float finalScale = objTransform.localScale.x; // Assuming uniform scaling

        // Set initial scale to zero
        objTransform.localScale = Vector3.zero;

        // Gradually increase the scale
        while (elapsedTime < growSpeed)
        {
            float scale = Mathf.Lerp(0f, finalScale, elapsedTime / growSpeed);
            objTransform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale
        objTransform.localScale = new Vector3(finalScale, finalScale, finalScale);
    }

    Vector3 CalculatePosition()
    {
        Vector3 position = Vector3.zero;

        Mesh complexShapeMesh = complexShape.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = complexShapeMesh.vertices;
        Vector3[] normals = complexShapeMesh.normals;

        // Ensure there are vertices and normals to work with
        if (vertices.Length == 0 || normals.Length == 0)
        {
            Debug.LogError("Mesh vertices or normals not available.");
            return position; // Return zero vector or handle error as needed
        }

        // Calculate position along the normal of the complex shape
        Vector3 vertex = vertices[Random.Range(0, vertices.Length)];
        Vector3 normal = normals[Random.Range(0, normals.Length)].normalized;

        // Check if normal length is zero to prevent division by zero
        if (normal.sqrMagnitude == 0f)
        {
            Debug.LogError("Zero length normal detected.");
            return position; // Return zero vector or handle error as needed
        }

        position = complexShape.transform.TransformPoint(vertex + normal * distanceFromSurface);

        return position;
    }
}

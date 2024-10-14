using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class LeafSpawner : MonoBehaviour
{
    public GameObject leafPrefab; // Prefab for the leaf
    public float leafScaleSize = 0.2f; // Initial scale of the leaf and the final size
    public int maxLeavesPerBranch = 3; // Maximum number of leaves per branch

    private LineRenderer lineRenderer;
    private List<GameObject> leaves = new List<GameObject>(); // List to store spawned leaves

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Check for input to spawn a leaf
        if (Input.GetKeyDown(KeyCode.Alpha1) && leaves.Count < maxLeavesPerBranch)
        {
            SpawnLeafRandomly();
        }
    }

    void SpawnLeafRandomly()
    {
        // Determine a random point index along the LineRenderer
        int randomPointIndex = Random.Range(0, lineRenderer.positionCount);

        // Check if a leaf is already spawned at this point
        if (IsLeafSpawnedAtPoint(randomPointIndex))
        {
            return;
        }

        // Randomize rotation
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        // Spawn leaf prefab at the selected point with random rotation
        Vector3 leafPosition = lineRenderer.GetPosition(randomPointIndex);
        GameObject newLeaf = Instantiate(leafPrefab, leafPosition, randomRotation);

        // Scale leaf gradually
        StartCoroutine(ScaleLeaf(newLeaf.transform, Vector3.one * leafScaleSize, 0.5f));

        // Add the leaf to the list
        leaves.Add(newLeaf);
    }

    bool IsLeafSpawnedAtPoint(int pointIndex)
    {
        foreach (GameObject leaf in leaves)
        {
            if (leaf != null && leaf.transform.position == lineRenderer.GetPosition(pointIndex))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator ScaleLeaf(Transform leafTransform, Vector3 targetScale, float duration)
    {
        float timer = 0f;
        Vector3 startScale = Vector3.zero;

        while (timer < duration)
        {
            leafTransform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        leafTransform.localScale = targetScale; // Ensure final scale is exactly target scale
    }
}

using UnityEngine;
using System.Collections;

public class ObjectScaler : MonoBehaviour
{
    public GameObject objectToInstantiate;  // Prefab to instantiate
    public float scalingDuration = 1.0f;    // Duration for the scaling effect
    public Vector3 finalScale = Vector3.one; // Final scale, adjustable in the Inspector

    // Method to instantiate and start scaling the object
    public void InstantiateAndScale(Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(objectToInstantiate, position, rotation);
        StartCoroutine(ScaleObject(instance));
    }

    // Coroutine to scale the object over time
    private IEnumerator ScaleObject(GameObject obj)
    {
        Vector3 initialScale = Vector3.zero;  // Starting scale
        float elapsedTime = 0f;

        // Initially set the object's scale to zero
        obj.transform.localScale = initialScale;

        // Gradually scale the object to its final size
        while (elapsedTime < scalingDuration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / scalingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set after the loop
        obj.transform.localScale = finalScale;
    }
}
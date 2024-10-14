using System.Collections;
using UnityEngine;

public class PrefabGrowth : MonoBehaviour
{
    public void OrientAndGrowPrefab(GameObject instance, Vector3 spawnPosition, float growthDuration, Vector3 finalScale, Vector3 flowerScale)
    {
        if (instance.CompareTag("Flower"))
        {
            instance.transform.position = spawnPosition;
            instance.transform.rotation = Quaternion.identity; // or any default rotation you prefer
        }
        else
        {
            Vector3 directionFromCactus = (spawnPosition - transform.position).normalized;
            instance.transform.position = spawnPosition;
            instance.transform.rotation = Quaternion.LookRotation(directionFromCactus);
        }

        instance.transform.localScale = Vector3.zero;
        StartCoroutine(GrowPrefab(instance, growthDuration, finalScale, flowerScale));
    }

    IEnumerator GrowPrefab(GameObject instance, float growthDuration, Vector3 finalScale, Vector3 flowerScale)
    {
        float elapsedTime = 0f;
        Vector3 targetScale = instance.CompareTag("Flower") ? flowerScale : finalScale;

        while (elapsedTime < growthDuration)
        {
            instance.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / growthDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        instance.transform.localScale = targetScale;
    }
}
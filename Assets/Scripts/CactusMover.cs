using System.Collections;
using UnityEngine;

public class CactusMover : MonoBehaviour
{
    public IEnumerator MoveCactusUpSmoothly(float moveUpAmount, float moveDuration)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, moveUpAmount, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }
}
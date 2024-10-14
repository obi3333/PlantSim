using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DOF : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;
    private Coroutine transitionCoroutine;

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out depthOfField);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartTransition(24f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartTransition(15f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartTransition(50f);
        }
    }

    void StartTransition(float targetDistance)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(SmoothTransition(targetDistance));
    }

    IEnumerator SmoothTransition(float targetDistance)
    {
        float startDistance = depthOfField.focusDistance.value;
        float elapsedTime = 0f;
        float transitionTime = 1f; 

        while (elapsedTime < transitionTime)
        {
            depthOfField.focusDistance.value = Mathf.Lerp(startDistance, targetDistance, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        depthOfField.focusDistance.value = targetDistance;
    }
}

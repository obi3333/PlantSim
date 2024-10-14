using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemTest : MonoBehaviour
{
    public float growthSpeed = 0.1f; // Speed of the stem growth
    public float curveFrequency = 0.5f; // Frequency of the curve
    public float curveMagnitude = 0.1f; // Magnitude of the curve
    public int segmentCount = 100; // Number of segments in the LineRenderer
    public float baseWidth = 0.1f; // Starting width of the LineRenderer
    public float maxWidth = 0.5f; // Maximum width of the LineRenderer

    private LineRenderer lineRenderer;
    public List<Vector3> points { get; private set; } = new List<Vector3>();
    private bool isGrowing = false;
    private float currentSegmentIndex = 0;
    private int maxSegmentsPerClick = 100; // Maximum segments to add per click

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = baseWidth;
        lineRenderer.endWidth = baseWidth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isGrowing)
        {
            isGrowing = true;
            currentSegmentIndex = 0; // Optionally reset currentSegmentIndex to restart growth from the beginning
        }

        if (isGrowing)
        {
            if (currentSegmentIndex >= segmentCount)
            {
                isGrowing = false;
            }
            else
            {
                currentSegmentIndex += growthSpeed;
                GrowStem();
            }
        }

        // Check for input to add segments
        if (Input.GetButtonDown("Fire1")) // Replace "Fire1" with your desired input
        {
            AddSegments(5); // Change to growthIncrement if needed
        }
    }

    void AddSegments(int count)
    {
        segmentCount += count;
    }

    void GrowStem()
    {
        if (currentSegmentIndex >= segmentCount)
        {
            isGrowing = false; // Stop growing when segment count is reached
            return;
        }

        float t = currentSegmentIndex / segmentCount;
        float width = Mathf.Lerp(maxWidth, baseWidth, Mathf.Pow(t, 0.5f)); // Exponential tapering width

        Vector3 newPoint = new Vector3(
            Mathf.Sin(currentSegmentIndex * curveFrequency) * curveMagnitude,
            currentSegmentIndex * 0.1f,
            0f
        );
        points.Add(newPoint);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        if (currentSegmentIndex >= segmentCount)
        {
            isGrowing = false; // Ensure growing stops if max segments reached
        }
    }
}

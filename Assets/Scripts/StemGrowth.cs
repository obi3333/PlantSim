using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemGrowth : MonoBehaviour
{
    public float growthSpeed = 0.1f; // Speed of the stem growth
    public float curveFrequency = 0.5f; // Frequency of the curve
    public float curveMagnitude = 0.1f; // Magnitude of the curve
    public int segmentCount = 100; // Number of segments in the LineRenderer
    public float baseWidth = 0.1f; // Starting width of the LineRenderer
    public float maxWidth = 0.5f; // Maximum width of the LineRenderer
    public int maxSegmentsPerClick = 100; // Maximum segments to add per click

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private bool isGrowing = false;
    private float currentSegmentIndex = 0;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = baseWidth;
        lineRenderer.endWidth = baseWidth;

        // Start growing the stem when the script starts
        StartGrowing();
    }

    void Update()
    {
        if (isGrowing)
        {
            if (currentSegmentIndex >= segmentCount)
            {
                isGrowing = false;
                // Start creating branches when the stem reaches full length
                CreateBranches();
            }
            else
            {
                currentSegmentIndex += growthSpeed;
                GrowStem();
            }
        }

        // Check for input to add segments (for testing or manual control)
        if (Input.GetButtonDown("Fire1")) // Replace "Fire1" with your desired input
        {
            AddSegments(5); // Example: Adding 5 segments per click
        }
    }

    void StartGrowing()
    {
        isGrowing = true;
    }

    void GrowStem()
    {
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
    }

    void CreateBranches()
    {
        // Here we can create child stems (branches)
        if (points.Count > 0)
        {
            // Example: Create 2 child branches
            CreateChildBranch(points[points.Count - 1], Vector3.right * 0.5f, 0.8f, 0.6f);
            CreateChildBranch(points[points.Count - 1], Vector3.left * 0.5f, 0.8f, 0.6f);
        }
    }

    void CreateChildBranch(Vector3 startPoint, Vector3 direction, float lengthScale, float widthScale)
    {
        GameObject branchObject = new GameObject("Branch");
        branchObject.transform.SetParent(transform);
        LineRenderer branchRenderer = branchObject.AddComponent<LineRenderer>();

        branchRenderer.startWidth = baseWidth * widthScale;
        branchRenderer.endWidth = baseWidth * widthScale;

        List<Vector3> branchPoints = new List<Vector3>();
        branchPoints.Add(startPoint);

        float branchLength = segmentCount * lengthScale; // Scale the length of the branch
        for (float i = 1; i <= branchLength; i++)
        {
            float t = i / branchLength;
            float width = Mathf.Lerp(maxWidth * widthScale, baseWidth * widthScale, Mathf.Pow(t, 0.5f));

            Vector3 newPoint = startPoint + direction * i * 0.1f; // Adjust multiplier for branch length
            branchPoints.Add(newPoint);
        }

        branchRenderer.positionCount = branchPoints.Count;
        branchRenderer.SetPositions(branchPoints.ToArray());
    }

    void AddSegments(int count)
    {
        segmentCount += count;
        currentSegmentIndex = Mathf.Min(currentSegmentIndex, segmentCount);
    }
}

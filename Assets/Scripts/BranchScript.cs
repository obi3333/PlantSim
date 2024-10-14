using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchScript : MonoBehaviour
{
    public StemTest stemTest; // Reference to the StemTest script
    public GameObject branchPrefab; // Prefab with a LineRenderer component for branches
    public int branchSegmentCount = 20; // Number of segments for each branch
    public float branchGrowthSpeed = 0.05f; // Speed of the branch growth
    public float branchCurveFrequency = 1.0f; // Frequency of the branch curve
    public float branchCurveMagnitude = 0.05f; // Magnitude of the branch curve
    public float branchBaseWidth = 0.05f; // Starting width of the branch LineRenderer
    public float branchMaxWidth = 0.2f; // Maximum width of the branch LineRenderer
    public float branchAngle = 30f; // Angle in degrees for the initial branch direction
    public int requiredKeyPresses = 15; // Number of '1' key presses required to start branching
    public float branchSpawnChance = 0.2f; // 20% chance to spawn a branch

    private int keyPressCount = 0; // Counter for '1' key presses
    private List<Branch> branches = new List<Branch>(); // List of active branches

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            keyPressCount++;
        }

        if (keyPressCount >= requiredKeyPresses)
        {
            if (Random.value < branchSpawnChance)
            {
                CreateBranch();
                keyPressCount = 0; // Reset the key press count after creating a branch
            }
        }

        UpdateBranches();
    }

    void CreateBranch()
    {
        int randomPointIndex = Random.Range(0, stemTest.points.Count);
        Vector3 point = stemTest.points[randomPointIndex];

        GameObject branchObj = Instantiate(branchPrefab, point, Quaternion.identity);
        LineRenderer branchLineRenderer = branchObj.GetComponent<LineRenderer>();
        branchLineRenderer.positionCount = 0;
        branchLineRenderer.startWidth = branchBaseWidth;
        branchLineRenderer.endWidth = branchBaseWidth;

        Vector3 branchDirection = Quaternion.Euler(0, 0, branchAngle) * Random.onUnitSphere;
        branchDirection.z = 0; // Ensure the branch direction is 2D

        branches.Add(new Branch(branchLineRenderer, point, branchDirection));
    }

    void UpdateBranches()
    {
        foreach (Branch branch in branches)
        {
            if (branch.currentSegmentIndex < branchSegmentCount)
            {
                branch.currentSegmentIndex += branchGrowthSpeed;
                GrowBranch(branch);
            }
        }
    }

    void GrowBranch(Branch branch)
    {
        float t = branch.currentSegmentIndex / branchSegmentCount;
        float width = Mathf.Lerp(branchMaxWidth, branchBaseWidth, Mathf.Pow(t, 0.5f)); // Exponential tapering width

        Vector3 newPoint = branch.startPoint + branch.direction * branch.currentSegmentIndex;
        newPoint += new Vector3(
            Mathf.Sin(branch.currentSegmentIndex * branchCurveFrequency) * branchCurveMagnitude,
            branch.currentSegmentIndex * 0.1f,
            0f
        );

        branch.points.Add(newPoint);

        branch.lineRenderer.positionCount = branch.points.Count;
        branch.lineRenderer.SetPositions(branch.points.ToArray());
    }

    class Branch
    {
        public LineRenderer lineRenderer;
        public Vector3 startPoint;
        public Vector3 direction;
        public List<Vector3> points;
        public float currentSegmentIndex;

        public Branch(LineRenderer lineRenderer, Vector3 startPoint, Vector3 direction)
        {
            this.lineRenderer = lineRenderer;
            this.startPoint = startPoint;
            this.direction = direction;
            this.points = new List<Vector3> { startPoint };
            this.currentSegmentIndex = 0;
        }
    }
}
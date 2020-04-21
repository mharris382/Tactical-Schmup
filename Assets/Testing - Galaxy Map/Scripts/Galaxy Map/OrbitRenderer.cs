using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitRenderer : MonoBehaviour
{
    [Range(3, 36)] public int segments;
    public Ellipse ellipse;

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        CalculateEllipse();
    }

    private void CalculateEllipse()
    {
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / (float)segments) * 360 * Mathf.Deg2Rad;
            Vector2 position2D = ellipse.Evaluate((float)i / (float)segments);
            points[i] = new Vector3(position2D.x, position2D.y, 0f);
        }
        points[segments] = points[0];

        lr.positionCount = segments + 1;
        lr.SetPositions(points);
    }

    private void OnValidate()
    {
        if (Application.isPlaying && lr != null)
        {
            CalculateEllipse();
        }
    }
}

using UnityEngine;

/// <summary>
/// Taken from: http://www.theappguruz.com/blog/bezier-curve-in-games
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class DrawBezier : MonoBehaviour {

    [SerializeField] private Transform[] controlPoints;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int SEGMENT_COUNT = 50;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

	void Update () {
        DrawCurve();
	}

    void DrawCurve()
    {
        for (int j = 0; j < (int)controlPoints.Length / 3; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);
                lineRenderer.positionCount = ((j * SEGMENT_COUNT) + i);
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
            }

        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 startPoint, Vector3 startTangent, Vector3 endTangent, Vector3 endPoint)
    {
        float u = 1 - t;

        Vector3 p = Mathf.Pow(u, 3) * startPoint;
        p += 3 * Mathf.Pow(u, 2) * t * startTangent;
        p += 3 * u * Mathf.Pow(t, 2) * endTangent;
        p += Mathf.Pow(t, 3) * endPoint;

        return p;
    }
}

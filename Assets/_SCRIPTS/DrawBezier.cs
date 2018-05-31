using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Taken from: http://www.theappguruz.com/blog/bezier-curve-in-games
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class DrawBezier : MonoBehaviour {

    [SerializeField] private Transform[] controlPoints;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float distance = 0.1f;
    [SerializeField] private int precision = 100;
    private Vector3[] points;
    private Vector3[] tangents;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        DrawCurve();
    }

	void Update () {
        if (distance == 0f)
            return;
        DrawCurve();
	}

    void DrawCurve()
    {
        /* Calculate the points with high precision */
        Vector3[] precisionPoints = new Vector3[((int)controlPoints.Length / 3) * precision];
        Vector3[] precicionTangents = new Vector3[((int)controlPoints.Length / 3) * precision];
        for (int j = 0; j < (int)controlPoints.Length / 3; j++)
        {
            for (int i = 0; i < precision; i++)
            {
                int nodeIndex = j * 3;
                float t = (float)i / (float)precision;
                precisionPoints[j * precision + i] = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);
                precicionTangents[j * precision + i] = CalculateCubicBezierTangent(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);
            }
        }

        /* Filter out points that are above the distance threshold */
        List<Vector3> reducedPoints = new List<Vector3>
        {
            precisionPoints[0]
        };
        List<Vector3> reducedTangents = new List<Vector3>();
        Vector3 previous = precisionPoints[0];
        for (int i = 1; i < precisionPoints.Length; i++)
        {
            if (Vector3.Distance(previous, precisionPoints[i]) > distance)
            {
                reducedPoints.Add(precisionPoints[i]);
                reducedTangents.Add(precicionTangents[i]);
                previous = precisionPoints[i];
            }
        }

        /* Populate the Line renderer */
        points = reducedPoints.ToArray();
        tangents = reducedTangents.ToArray();

        lineRenderer.positionCount = reducedPoints.Count;
        lineRenderer.SetPositions(points);
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

    /* Taken from: https://stackoverflow.com/a/4091430 */
    Vector3 CalculateCubicBezierTangent(float t, Vector3 startPoint, Vector3 startTangent, Vector3 endTangent, Vector3 endPoint)
    {
        return
            -3 * Mathf.Pow(1 - t, 2) * startPoint
            + 3 * Mathf.Pow(1 - t, 2) * startTangent - 6 * t * (1 - t) * startTangent
            - 3 * Mathf.Pow(t, 2) * endTangent + 6 * t * (1 - t) * endTangent
            + 3 * Mathf.Pow(t, 2) * endPoint; 
    }

    public IEnumerable<Vector3> GetPositionAnimations(float timeStep)
    {
        foreach (Vector3 point in points)
            yield return point;
    }

    public IEnumerable<Vector3> GetTangentAnimations(float timeStep)
    {
        foreach (Vector3 tangent in tangents)
            yield return tangent;
    }
}

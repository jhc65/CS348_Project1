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
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + i - 1, pixel);
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
        for (int segment = 0; segment < (int) controlPoints.Length / 3; segment++)
        {
            for (float t = 0f; t <= 1f; t += timeStep)
            {
                yield return CalculateCubicBezierPoint(t,
                    controlPoints[segment * 3].position,
                    controlPoints[segment * 3 + 1].position,
                    controlPoints[segment * 3 + 2].position,
                    controlPoints[segment * 3 + 3].position);
            }

        }
    }
}

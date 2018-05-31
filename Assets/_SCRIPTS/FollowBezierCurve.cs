using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBezierCurve : MonoBehaviour {

    [SerializeField] private DrawBezier bezier;
    [SerializeField] private float timeStep;
    private float startTime;
	
    void Start()
    {
        StartCoroutine(AnimateAlongBezierCurve());
    }

    IEnumerator AnimateAlongBezierCurve()
    {
        foreach (Vector3 position in bezier.GetPositionAnimations(timeStep))
        {
            this.transform.position = position;
            yield return new WaitForSeconds(timeStep);
        }
    }
}

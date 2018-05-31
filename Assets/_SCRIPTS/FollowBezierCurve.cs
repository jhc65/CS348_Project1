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
        yield return new WaitForSeconds(1f);
        IEnumerable<Vector3> enumurator = bezier.GetTangentAnimations(timeStep);
        IEnumerator<Vector3> tangents = enumurator.GetEnumerator();
        //IEnumerator<Vector3> iterator = 
        foreach (Vector3 position in bezier.GetPositionAnimations(timeStep))
        {
            /* Move the position */
            this.transform.position = position;

            /* Align the rotation */
            this.transform.localRotation = Quaternion.Euler(tangents.Current);
            tangents.MoveNext();
            yield return new WaitForSeconds(timeStep);
        }
    }
}

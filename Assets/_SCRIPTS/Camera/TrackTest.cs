using UnityEngine;

public class TrackTest : MonoBehaviour {

    [SerializeField] private float bias;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool active;
    [SerializeField] private bool trackX;
    [SerializeField] private bool trackY;
    [SerializeField] private bool trackZ;
    private TrackObject trackObject;

	// Use this for initialization
	void Start () {
        trackObject = Camera.main.GetComponent<TrackObject>();
        trackObject.StartTracking(this.transform, offset, bias, active, trackX, trackY, trackZ);
	}
}

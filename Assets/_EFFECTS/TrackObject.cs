using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour {

	[SerializeField] private GameObject ToTrack;
    [SerializeField]
    private bool X;
    [SerializeField]
    private bool Y;
    [SerializeField]
    private bool Z;
	
	// Update is called once per frame
	void Update () {
        /* I like the ternary operator... a lot */
        Vector3 target = new Vector3(
            X ? ToTrack.transform.position.x : this.transform.position.x,
            Y ? ToTrack.transform.position.y : this.transform.position.y,
            Z ? ToTrack.transform.position.z : this.transform.position.z);
        this.transform.position = target;
	}
}

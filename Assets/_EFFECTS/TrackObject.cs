using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour {

	[SerializeField] private GameObject ToTrack;
    [SerializeField]
    private bool trackX;
    [SerializeField]
    private bool trackY;
    [SerializeField]
    private bool trackZ;
	
	// Update is called once per frame
	void Update () {
        /* I like the ternary operator... a lot */
        Vector3 target = new Vector3(
            trackX ? ToTrack.transform.position.x : this.transform.position.x,
            trackY ? ToTrack.transform.position.y : this.transform.position.y,
            trackZ ? ToTrack.transform.position.z : this.transform.position.z);
        this.transform.position = target;
	}
}

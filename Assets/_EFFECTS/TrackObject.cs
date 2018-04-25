using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour {

	[SerializeField] private GameObject ToTrack;
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(
			 ToTrack.transform.position.x,
			 ToTrack.transform.position.y,
			 this.transform.position.z);
	}
}

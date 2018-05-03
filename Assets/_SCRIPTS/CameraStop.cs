using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStop : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "coaster")
        {
            /* The coaster entered this trigger, so tell the camera to stop following it */
            Camera.main.GetComponent<TrackObject>().PauseTracking();
        }
    }
}

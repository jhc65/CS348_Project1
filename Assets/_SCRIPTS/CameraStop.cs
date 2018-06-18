using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStop : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "cart (1)")
        {
            Debug.Log("oof");
            if(Camera.main.GetComponent<TrackObject>().Paused)
                /* The coaster entered this trigger, so tell the camera to start following it */
                Camera.main.GetComponent<TrackObject>().ResumeAllTracking();
            else
                /* The coaster entered this trigger, so tell the camera to stop following it */
                Camera.main.GetComponent<TrackObject>().PauseAllTracking();
        }
    }
}

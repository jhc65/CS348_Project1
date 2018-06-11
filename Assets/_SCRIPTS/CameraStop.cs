using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStop : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "cart (1)")
        {
            Debug.Log("oof");
            if(Camera.main.GetComponent<TrackObject>().paused)
                /* The coaster entered this trigger, so tell the camera to start following it */
                Camera.main.GetComponent<TrackObject>().ResumeTracking();
            else
                /* The coaster entered this trigger, so tell the camera to stop following it */
                Camera.main.GetComponent<TrackObject>().PauseTracking();
        }
    }
}

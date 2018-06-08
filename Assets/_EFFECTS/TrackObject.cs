using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour {

    public enum TrackType
    {
        Track,
        Lock,
        Ignore
    }

	[SerializeField] private GameObject ToTrack;
    [SerializeField] private TrackType trackX;
    [SerializeField] private TrackType trackY;
    [SerializeField] private TrackType trackZ;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float maxTrackSpeed;
    [SerializeField] private bool paused = false;
    private Vector3 original;

    private void Start()
    {
        original = this.transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (!paused)
        {
            /* Set each of the three axis based on their tracking type */
            Vector3 target = new Vector3();
            switch (trackX)
            {
                case TrackType.Track:
                    target.x = ToTrack.transform.position.x + offset.x;
                    break;
                case TrackType.Lock:
                    target.x = original.x;
                    break;
                case TrackType.Ignore:
                    target.x = this.transform.position.x;
                    break;
            }
            switch (trackY)
            {
                case TrackType.Track:
                    target.y = ToTrack.transform.position.y + offset.y;
                    break;
                case TrackType.Lock:
                    target.y = original.y;
                    break;
                case TrackType.Ignore:
                    target.y = this.transform.position.y;
                    break;
            }
            switch (trackZ)
            {
                case TrackType.Track:
                    target.z = ToTrack.transform.position.z + offset.z;
                    break;
                case TrackType.Lock:
                    target.z = original.z;
                    break;
                case TrackType.Ignore:
                    target.z = this.transform.position.z;
                    break;
            }
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, maxTrackSpeed);
        }
	}

    public void PauseTracking()
    {
        paused = true;
    }

    public void ResumeTracking()
    {
        paused = true;
    }
}

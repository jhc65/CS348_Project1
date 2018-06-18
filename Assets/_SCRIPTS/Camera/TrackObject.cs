using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{

    public class TrackData
    {
        public Transform transform;
        public Vector3 offset;
        public float bias;
        public bool trackX;
        public bool trackY;
        public bool trackZ;
        public bool active;

        public TrackData(Transform t, Vector3 o, float b, bool x = true, bool y = true, bool z = true)
        {
            transform = t;
            offset = o;
            bias = b;
            trackX = x;
            trackY = y;
            trackZ = z;
            active = true;
        }
    }

    private List<TrackData> transforms;
    public bool Paused { get; private set; }
    [SerializeField] private float MoveTowardsMaxDelta;

    private void Awake()
    {
        transforms = new List<TrackData>();
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* For each transform, create an offset vector to apply to the gameObject, taking into account local offset and bias */
        Vector3 movementVector = new Vector3(0f, 0f, 0f);
        Vector3 min = transforms[0].transform.position;
        Vector3 max = transforms[0].transform.position;
        foreach (TrackData td in transforms)
        {
            if (td.active)
            {
                Vector3 direction = ((td.transform.position + td.offset) - this.transform.position) * td.bias;
                if (td.trackX)
                    movementVector.x += direction.x;
                if (td.trackY)
                    movementVector.y += direction.y;
                if (td.trackZ)
                    movementVector.z += direction.z;

                if (td.transform.position.x < min.x)
                    min.x = td.transform.position.x;
                if (td.transform.position.x > max.x)
                    max.x = td.transform.position.x;
                if (td.transform.position.y < min.y)
                    min.y = td.transform.position.y;
                if (td.transform.position.y > max.y)
                    max.y = td.transform.position.y;
            }
        }

        /* Apply the movement */
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movementVector, MoveTowardsMaxDelta);
        float verticalSize = Camera.main.orthographicSize * 2f;
        float horizontalSize = verticalSize * Screen.width / Screen.height;

        //Debug.Log("Vertical size: " + verticalSize + ", horizontal size: " + horizontalSize);

        Vector3 targetScreenSize = (Camera.main.WorldToScreenPoint(max) - Camera.main.WorldToScreenPoint(min)) / 100f;
        //Debug.Log("Target screen Size: " + targetScreenSize);

        /* Don't want to cut one of the axis off, so whichever has the max change is the one to use */
        float verticalChange = Mathf.Abs(verticalSize - targetScreenSize.y);
        float horizontalChange = Mathf.Abs(horizontalSize - targetScreenSize.x);

        //Debug.Log("Vertical change: " + verticalChange + ", horizontal change: " + horizontalChange);

        Vector2 target = Vector2.MoveTowards(new Vector2(horizontalSize, verticalSize), targetScreenSize, MoveTowardsMaxDelta);

        //Debug.Log("<color=blue>Target: " + target + "</color>");

        if (horizontalChange > verticalChange)
        {
            /* Use the vertical */
            Camera.main.orthographicSize = target.y / 2f;
        }
        else
        {
            /* Use the horizontal size */
            Camera.main.orthographicSize = target.x * Screen.height / Screen.width / 2f;
        }

    }

    public void PauseAllTracking()
    {
        foreach (TrackData td in transforms)
            td.active = false;
        Paused = true;
    }

    public void ResumeAllTracking()
    {
        foreach (TrackData td in transforms)
            td.active = true;
        Paused = false;
    }

    public bool StartTracking(Transform toTrack, Vector3 offset, float bias = 1f, bool beginActive = true, bool trackX = true, bool trackY = true, bool trackZ = true)
    {
        if (transforms.Exists(t => t.transform == toTrack))
            return false;
        else
        {
            TrackData td = new TrackData(toTrack, offset, bias, trackX, trackY, trackZ)
            {
                active = beginActive
            };

            /* Adjust the existing bias */
            foreach (TrackData t in transforms)
            {
                t.bias = t.bias * (1f - bias);
            }
            transforms.Add(td);
            return true;
        }
    }

    public bool PauseTracking(Transform transform)
    {
        if (transforms.Exists(t => t.transform == transform))
        {
            transforms.Find(t => t.transform == transform).active = false;
            return true;
        }
        else
            return false;
    }

    public bool StopTracking(Transform transform)
    {
        if (transforms.Exists(t => t.transform == transform))
        {
            transforms.Remove(transforms.Find(t => t.transform == transform));
            return true;
        }
        else
            return false;
    }
}

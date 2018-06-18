using UnityEngine;

public class PerlinMovement : MonoBehaviour {

    [SerializeField] private Vector3 lowerBound;
    [SerializeField] private Vector3 upperBound;
    [SerializeField] private float magnitude;

    [SerializeField] private bool active = false;

    private static int instanceCount;
    private int instanceNumber;

    static PerlinMovement()
    {
        instanceCount = 0;
    }

    private void Awake()
    {
        instanceCount++;
        instanceNumber = instanceCount;
    }

    void Update () {
		if (active)
        {
            float xMovement = Mathf.Clamp(this.transform.position.x + magnitude * (Mathf.PerlinNoise(0f + instanceNumber, Time.time) * 2f - 1f), lowerBound.x, upperBound.x);
            float yMovement = Mathf.Clamp(this.transform.position.y + magnitude * (Mathf.PerlinNoise(1f + instanceNumber, Time.time) * 2f - 1f), lowerBound.y, upperBound.y);
            float zMovement = Mathf.Clamp(this.transform.position.z + magnitude * (Mathf.PerlinNoise(2f + instanceNumber, Time.time) * 2f - 1f), lowerBound.z, upperBound.z);

            this.transform.position = new Vector3(xMovement, yMovement, zMovement);
        }
    }

    void OnDrawGizmosSelected()
    {
        //float color = (float)instanceNumber / (float)instanceCount;
        Gizmos.color = new Color(0f, .5f, .5f, 0.5F);
        Gizmos.DrawCube((lowerBound + upperBound) / 2f, upperBound - lowerBound);
    }
}

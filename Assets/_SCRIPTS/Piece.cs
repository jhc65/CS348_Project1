using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    #region Variables
    Vector2 startPos;
    bool placed = false;
    #endregion

    #region Unity Overrides

    // Use this for initialization
    void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && !placed) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (!Input.GetMouseButton(0) && !placed) {
            transform.position = Vector2.Lerp(transform.position, startPos, 0.5f);
        }
        Vector2 transformPos = transform.position;
        if (!Input.GetMouseButton(0) && transformPos == startPos) {
            Destroy(gameObject);
        }
	}

    // OnTriggerSTAY (2D)
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("BuildArea") && Input.GetMouseButtonUp(0)) {
            placed = true;
        }
    }
    #endregion
}

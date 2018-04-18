using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    #region Variables
    Vector2 startPos;
    private Vector3 cursorPos;
    bool placed = false;
    bool isPickedUp = false;
    #endregion

    #region Piece Methods
    private void ToggleIsPickedUp() {
        if (isPickedUp) {
            isPickedUp = false;
        }
        else {
            isPickedUp = true;
        }
    }
    #endregion

    #region Unity Overrides

    // Use this for initialization
    void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0)) {
        //    RaycastHit hitInfo = new RaycastHit();
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
        //        gameObject.transform.position = hitInfo.collider.transform.position;
        //    }
        //}
        //if (Input.GetMouseButton(0) && !placed) {
        //    cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    cursorPos.x = cursorPos.x - 3.75f;
        //    isPickedUp = true;
        //}
        if (Input.GetMouseButtonDown(0) && !placed) {
            ToggleIsPickedUp();
        }
        if (isPickedUp && !placed) {
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.x = cursorPos.x - 3.75f;
            transform.position = Vector2.Lerp(transform.position, cursorPos, 0.5f);
        }
        //if (Input.GetMouseButton(0) && !placed) {
        //    transform.position = Vector2.Lerp(transform.position, cursorPos, 0.5f);
        //}
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

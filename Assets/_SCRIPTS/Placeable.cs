using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour {
#region Variables
    private GameController gc;
    private Vector3 startPos;
    private Vector3 cursorPos;
    private bool placed = false;
    private bool isPickedUp = true;

    [SerializeField] float spriteCenterOffset;
    [SerializeField] private int numerator;
    [SerializeField] private int denominator;
    [SerializeField] private FractionTools.Fraction value;
#endregion

#region Piece Methods
    public FractionTools.Fraction Value
    {
        get { return value; }
    }

    private void ToggleIsPickedUp() {
        if (isPickedUp) {
            isPickedUp = false;
            gc.ActiveCursor = Constants.Global.CursorType.HAND;
        }
        else {
            isPickedUp = true;
            gc.ActiveCursor = Constants.Global.CursorType.DRAG;
        }
    }

    private bool IsWithin(Vector3 obj1, Vector3 obj2)
    {
        float tolerance = 0.05f;
        if ((Mathf.Abs(obj1.x - obj2.x) < tolerance) && (Mathf.Abs(obj1.x - obj2.x) < tolerance))
            return true;
        else
            return false;
    }
#endregion

#region Unity Overrides

    // Use this for initialization
    void Start () {
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        startPos = transform.position;
        gc = GameController.Instance;
        value = new FractionTools.Fraction(numerator, denominator);
    }
	
	// Update is called once per frame
	void Update () {
        //Zak's return to base method
        //if (Input.GetMouseButton(0) && !placed)     // follow mouse
        //{
        //    cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    transform.position = Vector2.Lerp(transform.position, cursorPos, 0.5f);
        //}
        //if (!Input.GetMouseButton(0) && !placed)         // return to start if mouse is released
        //{
        //    transform.position = Vector2.Lerp(transform.position, startPos, 0.2f);
        //}
        //if (!Input.GetMouseButton(0) && IsWithin(transform.position, startPos))     // destroy when back to start position
        //{
        //    Destroy(gameObject);
        //    gc.ActiveCursor = Constants.Global.CursorType.HAND;
        //    //inv.Increase(length, 1);
        //    // change cursor to open hand
        //}

        //if (Input.GetMouseButtonDown(0)) {
        //    RaycastHit hitInfo = new RaycastHit();
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
        //        gameObject.transform.position = hitInfo.collider.transform.position;
        //    }
        //}
        //if (Input.GetMouseButton(0) && !placed) {
        //    cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    cursorPos.x = cursorPos.x - spriteCenterOffset;
        //    isPickedUp = true;
        //}

        // Joe's preferred method
        if (isPickedUp && !placed)
        {
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.x = cursorPos.x - spriteCenterOffset;
            transform.position = Vector2.Lerp(transform.position, cursorPos, 0.5f);
        }
    }

    // OnTriggerSTAY (2D)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BuildZone"))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Piece dropped in zone!");
                BuildZone bz = collision.GetComponent<BuildZone>();
                if (bz != null && bz.TryPlacePiece(this))
                {
                    placed = true;
                    gc.ActiveCursor = Constants.Global.CursorType.HAND;
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !placed)
        {
            ToggleIsPickedUp();
        }
    }
    #endregion
}

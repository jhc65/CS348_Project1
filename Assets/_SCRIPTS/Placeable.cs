using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour {
#region Variables
    private GameController gc;
    private Inventory inv;
    private Vector3 startPos;
    private Vector3 cursorPos;
    private bool placed = false;
    private bool isPickedUp = true;
    private FractionTools.Fraction value;
    private BuildZone bz;

    //[SerializeField] private int numerator;
    //[SerializeField] private int denominator;
    [SerializeField] private Constants.PieceLength length;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject animatedPiece;

    #region Getters and Setters
    public Constants.PieceLength Length {
        get { return length; }
    }

    public GameObject AnimatedPiece {
        get { return animatedPiece; }
    }
    #endregion
    #endregion

    #region Piece Methods
    public FractionTools.Fraction Value
    {
        get { return value; }
    }

    private void ToggleIsPickedUp() {
        if (isPickedUp) {
            isPickedUp = false;
            gc.ActiveCursor = Constants.CursorType.HAND;
        }
        else {
            isPickedUp = true;
            gc.ActiveCursor = Constants.CursorType.DRAG;
        }
    }

    private bool IsWithin(Vector3 obj1, Vector3 obj2)
    {
        float tolerance = 0.05f;
        if ((Mathf.Abs(obj1.x - obj2.x) < tolerance) && (Mathf.Abs(obj1.y - obj2.y) < tolerance))
            return true;
        else
            return false;
    }
#endregion

#region Unity Overrides

    void Start () {
        gc = GameController.Instance;
        inv = Inventory.Instance;
        value = new FractionTools.Fraction(1, (int)length);
        sprite.color = Constants.trackColor;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("mouse up");
            if (!placed && bz != null && bz.TryPlacePiece(this))
            {
                Debug.Log("Piece dropped in zone!");
                placed = true;
                gc.ActiveCursor = Constants.CursorType.HAND;
            }
        }
        //Zak's return to base method
        if (Input.GetMouseButton(0) && !placed)     // follow mouse
        {
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.MoveTowards(transform.position, cursorPos, Time.deltaTime * 50f);
        }
        if (!Input.GetMouseButton(0) && !placed)         // return to start if mouse is released
        {
            startPos = inv.pieces[(int)length - 2].transform.position;
            transform.position = Vector2.MoveTowards(transform.position, startPos, Time.deltaTime * 20f);
            gc.ActiveCursor = Constants.CursorType.HAND;
        }
        if (!Input.GetMouseButton(0) && IsWithin(transform.position, startPos))     // destroy when back to start position
        {
            Destroy(gameObject);
            inv.Increase(length, 1);
        }

        //// Joe's preferred method
        //if (isPickedUp && !placed)
        //{
        //    cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    transform.position = Vector2.Lerp(transform.position, cursorPos, 0.5f);
        //}
    }

    // OnTriggerSTAY (2D)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (placed)
            return;

        if (collision.CompareTag("BuildZone"))
        {
            bz = collision.GetComponentInParent<BuildZone>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BuildZone"))
        {
            bz = null;
        }
    }

    private void OnMouseDown()
    {
        if(!placed)
        {
            ToggleIsPickedUp();
        }
    }

    // OnMouseUp is not called unless MouseDown was called on the same object first,
        // which is not the case since we're instantiating this placeable on click
    //private void OnMouseUp()
    //{
    //    Debug.Log("mouse up");
    //    if (!placed && bz != null && bz.TryPlacePiece(this))
    //    {
    //        Debug.Log("Piece dropped in zone!");
    //        placed = true;
    //        gc.ActiveCursor = Constants.CursorType.HAND;
    //    }
    //}
    #endregion
}

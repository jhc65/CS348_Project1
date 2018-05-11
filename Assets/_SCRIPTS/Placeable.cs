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
        startPos = transform.position;
        gc = GameController.Instance;
        inv = Inventory.Instance;
        value = new FractionTools.Fraction(1, (int)length);
        sprite.color = Constants.trackColor;
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
        //    gc.ActiveCursor = Constants.Global.CursorType.HAND;
        //}
        //if (!Input.GetMouseButton(0) && IsWithin(transform.position, startPos))     // destroy when back to start position
        //{
        //    Destroy(gameObject);
        //    inv.Increase(length, 1);
        //}

        // Joe's preferred method
        if (isPickedUp && !placed)
        {
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.Lerp(transform.position, cursorPos, 0.5f);
        }
    }

    // OnTriggerSTAY (2D)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (placed)
            return;

        if (collision.CompareTag("BuildZone"))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Piece dropped in zone!");
                BuildZone bz = collision.GetComponentInParent<BuildZone>();
                if (bz != null && bz.TryPlacePiece(this))
                {
                    placed = true;
                    gc.ActiveCursor = Constants.CursorType.HAND;
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

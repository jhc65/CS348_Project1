using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    #region Variables
    public Texture2D cursorTexture;        // custom tool sprite
    private GameController gc;
    //private Inventory inv;
    private Vector2 hotSpot;            // center of custom cursor icon

    #region Serialized Privates
    [SerializeField]
    private Constants.Global.PieceLength length;
    [SerializeField]
    private Constants.Global.CursorType cursorType;
    [SerializeField]
    private GameObject pieceToDrag;
    [SerializeField]
    private GameObject highlight;
    [SerializeField]
    private GameObject newPiece;
    #endregion
    #endregion

    #region Draggable Functions
    #endregion

    #region Unity Overrides
    // Use this for initialization
    void Start () {
        gc = GameController.Instance;
        //inv = Inventory.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0)) {
        //    RaycastHit hitInfo = new RaycastHit();
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
        //        gameObject.transform.position = hitInfo.collider.transform.position;
        //    }
        //}
    }

    // Mouse OVER
    private void OnMouseOver() {
        if (gc.ActiveCursor == Constants.Global.CursorType.HAND) {
            highlight.SetActive(true);
            if (Input.GetMouseButtonDown(0)) {
                gc.ActiveCursor = Constants.Global.CursorType.DRAG;
                highlight.SetActive(false);
                Instantiate(newPiece, transform.position, Quaternion.identity);
                //inv.Decrease(length);
                // change cursor to closed hand here
            }
            if (Input.GetMouseButtonDown(1)) {
                highlight.SetActive(false);
                // change cursor to cut tool here
            }
        }
    }

    // changes cursor to match selected tool
    //void OnMouseDown() {
    //    if (gc.ActiveCursor == Constants.Global.CursorType.CUT) {
    //        hotSpot = new Vector2(cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
    //        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
    //        gc.ActiveCursor = cursorType;
    //    }
    //    else if (gc.ActiveCursor == Constants.Global.CursorType.HAND) {
    //        //Cursor.SetCursor(CLOSED_HAND_CURSOR_TEXTURE, Vector2.zero, CursorMode.ForceSoftware);
    //        gc.ActiveCursor = Constants.Global.CursorType.PIECE;
    //        Instantiate(newPiece, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f)); 
    //    }
    //}

    // Mouse EXIT
    private void OnMouseExit() {
       if (gc.ActiveCursor == Constants.Global.CursorType.HAND) {
            highlight.SetActive(false);
        }
    }
    #endregion
}

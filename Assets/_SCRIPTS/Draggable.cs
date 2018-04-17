using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    #region Variables
    public Texture2D cursorTexture;        // custom tool sprite
    private GameController gc;
    private Vector2 hotSpot;            // center of custom cursor icon

    #region Serialized Privates
    [SerializeField]
    private Constants.Global.CursorType cursorType;
    [SerializeField]
    private GameObject pieceToDrag;
    [SerializeField]
    private GameObject highlight;
    #endregion
    #endregion

    #region Unity Overrides
    // Use this for initialization
    void Start () {
        gc = GameController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Mouse OVER
    private void OnMouseOver() {
        if (gc.ActiveCursor == Constants.Global.CursorType.HAND) {
            highlight.SetActive(true);
            if (Input.GetMouseButtonDown(0)) {
                Instantiate(pieceToDrag, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            if (Input.GetMouseButtonDown(1)) {
                highlight.SetActive(false);
            }
        }
    }

    // changes cursor to match selected tool
    void OnMouseDown() {
        hotSpot = new Vector2(cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
        GameController.Instance.ActiveCursor = cursorType;
    }

    // Mouse EXIT
    private void OnMouseExit() {
       if (gc.ActiveCursor == Constants.Global.CursorType.HAND) {
            highlight.SetActive(false);
        }
    }
    #endregion
}

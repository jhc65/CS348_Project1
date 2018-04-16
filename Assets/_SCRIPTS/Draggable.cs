using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    #region Variables
    public Texture2D cursorTexture;        // custom tool sprite
    private Vector2 hotSpot;            // center of custom cursor icon
    #endregion

    #region Class Functions

    #endregion

    #region Unity Overrides
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // changes cursor to match selected tool
    void OnMouseDown()
    {
        hotSpot = new Vector2(cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
        GameController.Instance.ActiveCursor = gameObject;
    }
    #endregion
}

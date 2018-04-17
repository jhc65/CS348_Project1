using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static private GameController instance;      // instance of the GameController
    private Constants.Global.CursorType activeCursor;

    public static GameController Instance
    {
        get { return instance; }
    }

    public Constants.Global.CursorType ActiveCursor
    {
        get { return activeCursor; }
        set { activeCursor = value; }
    }

    void Awake()
    {
        instance = this;
        activeCursor = Constants.Global.CursorType.HAND;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (activeCursor == Constants.Global.CursorType.CUT)
            {
                activeCursor = Constants.Global.CursorType.HAND;
            }
            else if (activeCursor == Constants.Global.CursorType.HAND)
            {
                activeCursor = Constants.Global.CursorType.CUT;
            }
        }
    }
}

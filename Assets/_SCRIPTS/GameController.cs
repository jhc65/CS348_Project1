using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static private GameController instance;      // instance of the GameController
    public enum CursorType { HAND, CUT };
    private CursorType activeCursor;

    public static GameController Instance
    {
        get { return instance; }
    }

    public CursorType ActiveCursor
    {
        get { return activeCursor; }
        set { activeCursor = value; }
    }

    private void Awake()
    {
        instance = this;
    }
}

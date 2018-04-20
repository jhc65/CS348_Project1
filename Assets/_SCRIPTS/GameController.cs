using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static private GameController instance;      // instance of the GameController
    [SerializeField] private Texture2D[] cursorTextures;    // custom cursor sprites
    private Vector2 cursorHotSpot;                          // center of custom cursor icon
    private Constants.CursorType activeCursor;
    [SerializeField] private Piece[] pieces;

    public static GameController Instance
    {
        get { return instance; }
    }

    public Constants.CursorType ActiveCursor
    {
        get { return activeCursor; }
        set {
            activeCursor = value;
            int index = 0;
            switch (activeCursor)
            {
                case Constants.CursorType.HAND:
                    index = 0;
                    foreach(Piece piece in pieces)
                        piece.EnableDraggable();
                    break;
                case Constants.CursorType.DRAG:
                    index = 1;
                    break;
                case Constants.CursorType.CUT:
                    index = 2;
                    foreach (Piece piece in pieces)
                        piece.EnableCuttables();
                    break;
            }
            cursorHotSpot = new Vector2(cursorTextures[index].width * 0.5f, cursorTextures[index].height * 0.5f);
            Cursor.SetCursor(cursorTextures[index], cursorHotSpot, CursorMode.ForceSoftware);
        }
    }

    void Awake()
    {
        instance = this;
        ActiveCursor = Constants.CursorType.HAND;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (activeCursor == Constants.CursorType.CUT)
            {
                ActiveCursor = Constants.CursorType.HAND;
            }
            else if (activeCursor == Constants.CursorType.HAND)
            {
                ActiveCursor = Constants.CursorType.CUT;
            }
        }
    }

    public void OnGapFilled()
    {
        Debug.Log("Gap has been filled!");
        /* For now, clear out the existing build zone, and choose a random new fraction
         *
         * TODO: Slide the screen to the next build zone
         */

         /* Get the BuildZone */
         GameObject buildZoneGameObject = GameObject.FindGameObjectWithTag("BuildZone");
         if (buildZoneGameObject == null)
         {
             Debug.LogError("No build zone found upon winning.");
             return;
         }

         /* Clear out the gap */
         BuildZone buildZoneScript = buildZoneGameObject.GetComponent<BuildZone>();
         if (buildZoneScript == null)
         {
             Debug.LogError(buildZoneGameObject.name + " is tagged as a BuildZone, but is missing the BuildZone script.");
             return;
         }
         buildZoneScript.ClearBuildZone();

         /* Choose a random new improper fraction */
         /// TODO: Make this based on difficulty and some form of probability distribution
         FractionTools.Fraction newGapSize = new FractionTools.Fraction(Random.Range(1, 5), Random.Range(2, 10));
         Debug.Log("<color=blue>New gap size: " + newGapSize + "</color>");
         buildZoneScript.SetGapSize(newGapSize);

         /// TODO: Restock the player's inventory
    }
}

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

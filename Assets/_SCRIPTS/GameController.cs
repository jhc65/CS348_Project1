using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables and Declarations
    static private GameController instance;      // instance of the GameController
    [SerializeField] private Texture2D[] cursorTextures;    // custom cursor sprites
    private Constants.CursorType activeCursor;
    [SerializeField] private Piece[] pieces;
    [SerializeField] private GameObject[] sections;
    //[SerializeField] private GameObject background;
    [SerializeField] private GameObject cam;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private float delayOnWin; // How long to wait after clearing a gap before moving on

    private int numBuildZones = 0;
    private int clearedBuildZones = 0;
    private BuildZone lastInteractedBuildZone;
    private Inventory inv;

    // UI Shit
    [SerializeField]
    private Button undoButton;

    #region Getters and Setters
    public static GameController Instance
    {
        get { return instance; }
    }
    #endregion
    #endregion

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
            Vector2 cursorHotSpot = new Vector2(0, 0);
            Cursor.SetCursor(cursorTextures[index], cursorHotSpot, CursorMode.ForceSoftware);
        }
    }

    public BuildZone LastInteractedBuildZone
    {
        set { lastInteractedBuildZone = value; }
    }

    void Awake()
    {
        instance = this;
        ActiveCursor = Constants.CursorType.HAND;
    }

    void Start()
    {
        undoButton.interactable = false;
        Constants.gameOver = false;
        inv = Inventory.Instance;

        // setup build zones and add pieces
        for(int i=0; i<5; i++)
        {
            int ind = Random.Range(0, sections.Length);
            GameObject go = Instantiate(sections[ind], new Vector3(19.2f * i, sections[ind].transform.position.y, 0), Quaternion.identity);
            List<FractionTools.Fraction> gapSizes = go.GetComponent<Section>().SetupBuildZones();
            foreach(FractionTools.Fraction gap in gapSizes)
            {
                numBuildZones++;
                Constants.PieceLength[] pieces = FractionBuilder.BreakMyLifeIntoPieces(gap, 0);
                foreach (Constants.PieceLength piece in pieces)
                {
                    inv.Increase(piece, 1);
                }
            }
        }
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

        if (lastInteractedBuildZone && lastInteractedBuildZone.PieceCount > 0) {
            undoButton.interactable = true;
        }
        else {
            undoButton.interactable = false;
        }

        cam.transform.position = new Vector3(cam.transform.position.x + 0.001f, cam.transform.position.y, cam.transform.position.z);
    }

    public IEnumerator OnGapFilled()
    {
        //Debug.Log("Gap has been filled!");

        /* Play a confetti effect and wait a few seconds for it to finish */
        EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Confetti);
        EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Yay);
        yield return new WaitForSeconds(delayOnWin);


        // after joe adds his code...TODO: increase numBuildZones as you're gathering them in Start
        lastInteractedBuildZone = null;
        clearedBuildZones++;
        if(clearedBuildZones == numBuildZones)
        {
            EndGame(true);
        }


        ///* For now, clear out the existing build zone, and choose a random new fraction
        // *
        // * TODO: Slide the screen to the next build zone
        // */

        // /* Get the BuildZone */
        // GameObject buildZoneGameObject = GameObject.FindGameObjectWithTag("BuildZone");
        // if (buildZoneGameObject == null)
        // {
        //     Debug.LogError("No build zone found upon winning.");
        //     yield return null;
        // }

        // /* Clear out the gap */
        // BuildZone buildZoneScript = buildZoneGameObject.GetComponent<BuildZone>();
        // if (buildZoneScript == null)
        // {
        //     Debug.LogError(buildZoneGameObject.name + " is tagged as a BuildZone, but is missing the BuildZone script.");
        //     yield return null;
        // }
        // buildZoneScript.ClearBuildZone();

        // /* Choose a random new improper fraction */
        // /// TODO: Make this based on difficulty and some form of probability distribution
        // FractionTools.Fraction newGapSize = new FractionTools.Fraction(Random.Range(1, 5), Random.Range(2, 10));
        // Debug.Log("<color=blue>New gap size: " + newGapSize + "</color>");
        // buildZoneScript.SetGapSize(newGapSize);

         /// TODO: Restock the player's inventory
         yield return null;
    }

    public void EndGame(bool won)
    {
        if (won)
            Constants.endgameText = "Congratulations, you won!";
        else
            Constants.endgameText = "You crashed!  Better luck next time!";

        Constants.gameOver = true;
        SceneManager.LoadScene("Menu");
    }

    public void MenuCursorSet()
    {
        Vector2 cursorHotSpot = new Vector2(0, 0);
        Cursor.SetCursor(cursorTextures[3], cursorHotSpot, CursorMode.ForceSoftware);
    }

    public void MenuCursorUnSet()
    {
        if (pauseMenu.activeSelf)   // don't unset if the PointerExit was caused by the menu blocking the button
        {
            Debug.Log("joke's on you, unity!");
            return;
        }
        ActiveCursor = activeCursor;
    }

    public void UndoClick()
    {
        // TODO:
        // 1) Get active Build Zone
        if(lastInteractedBuildZone)
        {
            // 2) Remove the top piece from the queue
            // 3) Update the equation and build zone image
            // 4) Animate it going back to the inventory?
            // 5) Increase the inventory number
            lastInteractedBuildZone.PopStack();
        }

    }

    public void RestartClick()
    {
        // TODO:
        // 1) Get active Build Zone
        if (lastInteractedBuildZone)
        {
            // 2) Loop until Build Zone is empty:
            // a) Remove the top piece from the queue
            // b) Update the equation and build zone image
            // c) Animate it going back to the inventory?
            // d) Increase the inventory number
        }
    }

    public void PauseClick()
    {
        // TODO: pause de game
        pauseMenu.SetActive(true);
        MenuCursorSet();
    }

    public void PlayClick()
    {
        // TODO: unpause de game
        pauseMenu.SetActive(false);
        MenuCursorUnSet();
    }

    public void OptionsClick()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void BackClick()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void QuitClick()
    { SceneManager.LoadScene("Menu"); }
}

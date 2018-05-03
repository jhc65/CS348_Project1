using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static private GameController instance;      // instance of the GameController
    [SerializeField] private Texture2D[] cursorTextures;    // custom cursor sprites
    private Constants.CursorType activeCursor;
    [SerializeField] private Piece[] pieces;
    //[SerializeField] private GameObject[] sections;
    [SerializeField] private Section section;
    [SerializeField] private GameObject cam;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private GameObject coaster;

    [SerializeField] private float delayOnWin; // How long to wait after clearing a gap before moving on

    private List<BuildZone> activeBuildZones;
    private BuildZone lastInteractedBuildZone;
    private List<Section> spawnedSections; /* Holds the sections spawned for a play session */
    private int activeSectionIndex; /* Index of active Section in spawnedSections */
    public int numBuildZones = 0;
    public int clearedBuildZones = 0;

    private Inventory inv;

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
            float offset = 0.5f;
            switch (activeCursor)
            {
                case Constants.CursorType.HAND:
                    index = 0;
                    foreach(Piece piece in pieces)
                        if(piece.Interactable) piece.EnableDraggable(); // only set if interactable
                    break;
                case Constants.CursorType.DRAG:
                    index = 1;
                    break;
                case Constants.CursorType.CUT:
                    index = 2;
                    offset = 0.2f;
                    foreach (Piece piece in pieces)
                        if (piece.Interactable) piece.EnableCuttables(); // only set if interactable
                    break;
            }
            Vector2 cursorHotSpot = new Vector2(cursorTextures[index].width * offset, cursorTextures[index].height * offset);
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
        Constants.gameOver = false;
        Time.timeScale = 1;
        inv = Inventory.Instance;

        // setup build zones and add pieces
        spawnedSections = new List<Section>();
        activeBuildZones = new List<BuildZone>();
        FractionTools.Fraction sum = FractionTools.Fraction.Zero; /* Used for hardmode */
        //for(int i=0; i<5; i++)
        //{
            //int ind = Random.Range(0, sections.Length);
            //GameObject go = Instantiate(sections[ind], new Vector3(19.2f * i, sections[ind].transform.position.y, 0), Quaternion.identity);
            //Section section = go.GetComponent<Section>();
            spawnedSections.Add(section);
            activeBuildZones.AddRange(section.SetupBuildZones());

            List<FractionTools.Fraction> gapSizes = section.GapSizes;
            if (Constants.unlimitedInventory)    // set inventory unlimited
            {
                inv.SetUnlimited();
            }
            else    // distribute inventory
            {
                foreach (FractionTools.Fraction gap in gapSizes)
                {
                    /* If not on the hardest difficulty, break each gap into pieces */
                    if (Constants.difficulty != Constants.Difficulty.DEIFENBACH)
                    {
                        Constants.PieceLength[] pieces = FractionBuilder.BreakMyLifeIntoPieces(gap);
                        foreach (Constants.PieceLength piece in pieces)
                        {
                            inv.Increase(piece, 1);
                        }
                    }
                    /* If playing on the hardest difficulty, sum the pieces before breaking */
                    else
                    {
                        sum += gap;
                    }
                }
            }
        //}
        /* Break the master sum into pieces */
        if (Constants.difficulty == Constants.Difficulty.DEIFENBACH)
        {
            Constants.PieceLength[] pieces = FractionBuilder.BreakMyLifeIntoPieces(sum);
            foreach (Constants.PieceLength piece in pieces)
            {
                inv.Increase(piece, 1);
            }
        }
        /* If this is the first section, tell the coaster to play its animation */
        CoasterManager.Instance.PlaySection(spawnedSections[0].GetAnimationTrigger());
        activeSectionIndex = 0;
        numBuildZones = activeBuildZones.Count;
        lastInteractedBuildZone = activeBuildZones[0];
        lastInteractedBuildZone.Activate();
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

        /* cmb539: Commenting this out, to instead use a script on the camera that follows the coaster */
        //cam.transform.position = new Vector3(cam.transform.position.x + 0.001f, cam.transform.position.y, cam.transform.position.z);
    }

    public void TriggerNextSectionAnimation()
    {
        /* Set the next section as Active */
        activeSectionIndex++;

        /* If there is a next section trigger its animation */
        if (activeSectionIndex < spawnedSections.Count)
            CoasterManager.Instance.PlaySection(spawnedSections[activeSectionIndex].GetAnimationTrigger());
    }

    public IEnumerator OnGapFilled()
    {
        /* Play a confetti effect and wait a few seconds for it to finish */
        //EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Confetti);
        EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Construction);
        lastInteractedBuildZone.Sparkle();

        /* Speed up the coaster */
        CoasterManager.Instance.SpeedUp();

        yield return new WaitForSeconds(delayOnWin);


        // set the new build zone
        lastInteractedBuildZone = null;
        clearedBuildZones++;
        if(clearedBuildZones == numBuildZones)
        {
            //EndGame(true); /* CMB: EndGame is now triggered by a game object in the scene */
        }
        else
        {
            lastInteractedBuildZone = activeBuildZones[clearedBuildZones];
            lastInteractedBuildZone.Activate();
        }

         yield return null;
    }

    public void EndGame(bool won)
    {
        Constants.gameWon = won;
        Constants.gameOver = true;
        SceneManager.LoadScene("Menu");
    }

    public void MenuCursorSet()
    {
        Vector2 cursorHotSpot = new Vector2(cursorTextures[3].width * 0.25f, cursorTextures[3].height * 0.25f);
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
        Time.timeScale = 0; /* Manipulate timeScale to pause the game */
        pauseMenu.SetActive(true);
        MenuCursorSet();
    }

    public void PlayClick()
    {
        // TODO: unpause de game
        Time.timeScale = 1f; /* Change timeScale back from 0 */
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

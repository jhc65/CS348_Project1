using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject gameSettingsMenu;
    [SerializeField] private GameObject instructionsMenu;
    [SerializeField] private GameObject endgameMenu;
    [SerializeField] private GameObject wonMenu;
    [SerializeField] private GameObject lostMenu;

    [SerializeField] private Toggle unlimitedInventoryToggle;
    [SerializeField] private Toggle showCutLengthsToggle;
    [SerializeField] private Toggle gapWidthAlwaysOneToggle;
    [SerializeField] private Toggle gapWidthAlwaysAtomicToggle;
    [SerializeField] private Toggle gapWidthImproperFractionsToggle;
    [SerializeField] private Toggle gapWidthMixedNumbersToggle;
    [SerializeField] private ToggleGroup colorToggles;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private SpriteRenderer track;



    private void Start()
    {
        Time.timeScale = 1;

        track.color = Constants.trackColor;
        CoasterManager.Instance.ChangeColor(Constants.trackColor);
        colorToggles.SetAllTogglesOff();
        Toggle[] toggles = colorToggles.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in toggles)
        {
            if (t.colors.normalColor == Constants.trackColor)
                t.isOn = true;
        }


        Vector2 cursorHotSpot = new Vector2(cursorTexture.width * 0.25f, cursorTexture.height * 0.25f);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.ForceSoftware);
        if(Constants.gameOver)
        {
            startMenu.SetActive(false);
            endgameMenu.SetActive(true);
            if (Constants.gameWon)
                wonMenu.SetActive(true);
            else
                lostMenu.SetActive(true);
        }
    }

    public void PlayClick()
    {
        // save selected settings to Constants
        // TODO: read them out of Constants where relevant
        Constants.gapAlwaysOne = gapWidthAlwaysOneToggle.isOn;
        Constants.gapAlwaysAtomic = gapWidthAlwaysAtomicToggle.isOn;
        Constants.gapAllowImproperFractions = gapWidthImproperFractionsToggle.isOn;
        Constants.gapAllowMixedNumbers = gapWidthMixedNumbersToggle.isOn;
        Constants.unlimitedInventory = unlimitedInventoryToggle.isOn;
        Constants.showCutLengths = showCutLengthsToggle.isOn;

        // track color
        Toggle activeToggle = colorToggles.ActiveToggles().FirstOrDefault();
        Constants.trackColor = activeToggle.colors.normalColor;

        StartCoroutine(ExitTheStation());
    }

    public IEnumerator ExitTheStation()
    {
        /* Play the coaster leaving the station */
        CoasterManager.Instance.PlaySection(CoasterManager.SectionTriggers.PlayEnterScreen);
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Main");
    }

    public void GameSettingsClick()
    {
        gameSettingsMenu.SetActive(true);
        CoasterManager.Instance.PlaySection(CoasterManager.SectionTriggers.PlayEnterScreen);
        startMenu.SetActive(false);
    }

    public void PlayAgainClick()
    {
        gameSettingsMenu.SetActive(true);
        CoasterManager.Instance.PlaySection(CoasterManager.SectionTriggers.PlayEnterScreen);
        endgameMenu.SetActive(false);
    }

    public void TutorialClick()
    {
        instructionsMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void OptionsClick()
    {
        optionsMenu.SetActive(true);
        startMenu.SetActive(false);  
    }

    public void BackClick()
    {
        /* Move the coaster back off the screen */
        CoasterManager.Instance.ResetToStartPosition();
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        endgameMenu.SetActive(false);
        gameSettingsMenu.SetActive(false);
    }

    public void QuitClick()
    { Application.Quit(); }

    public void AlwaysOneCheck()
    {
        if(gapWidthAlwaysOneToggle.isOn)
        {
            gapWidthAlwaysAtomicToggle.isOn = false;
            gapWidthImproperFractionsToggle.isOn = false;
            gapWidthMixedNumbersToggle.isOn = false;
        }        
    }

    public void AlwaysAtomicCheck()
    {
        if (gapWidthAlwaysAtomicToggle.isOn)
        {
            gapWidthAlwaysOneToggle.isOn = false;
            gapWidthImproperFractionsToggle.isOn = false;
            gapWidthMixedNumbersToggle.isOn = false;
        }
    }

    public void ImproperFractionCheck()
    {
        if (gapWidthImproperFractionsToggle.isOn)
        {
            gapWidthAlwaysOneToggle.isOn = false;
            gapWidthAlwaysAtomicToggle.isOn = false;
        }
    }

    public void MixedNumbersCheck()
    {
        if (gapWidthMixedNumbersToggle.isOn)
        {
            gapWidthAlwaysOneToggle.isOn = false;
            gapWidthAlwaysAtomicToggle.isOn = false;
        }
    }

    public void ChangeCoasterColor(Toggle t)
    {
        if (t.isOn)
        {
            track.color = t.colors.normalColor;
            CoasterManager.Instance.ChangeColor(t.colors.normalColor);
        }
    }
}

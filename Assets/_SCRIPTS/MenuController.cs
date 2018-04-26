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
    [SerializeField] private GameObject endgameMenu;

    [SerializeField] private Toggle unlimitedInventoryToggle;
    [SerializeField] private Toggle showCutLengthsToggle;
    [SerializeField] private Toggle gapWidthAlwaysOneToggle;
    [SerializeField] private Toggle gapWidthImproperFractionsToggle;
    [SerializeField] private Toggle gapWidthMixedNumbersToggle;
    [SerializeField] private Toggle multipleGapsToggle;
    [SerializeField] private ToggleGroup colorToggles;

    [SerializeField] private Texture2D cursorTexture;


    private void Start()
    {
        // TODO: set mouse cursor to generic arrow
        Vector2 cursorHotSpot = new Vector2(0, 0);//cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.ForceSoftware);
        if(Constants.gameOver)
        {
            startMenu.SetActive(false);
            endgameMenu.SetActive(false);
        }
    }

    public void PlayClick()
    {
        // save selected settings to Constants
        // TODO: read them out of Constants where relevant
        Constants.gapAlwaysOne = gapWidthAlwaysOneToggle.isOn;
        Constants.gapAllowImproperFractions = gapWidthImproperFractionsToggle.isOn;
        Constants.gapAllowMixedNumbers = gapWidthMixedNumbersToggle.isOn;
        Constants.gapAllowMultiple = multipleGapsToggle.isOn;
        Constants.unlimitedInventory = unlimitedInventoryToggle.isOn;
        Constants.showCutLengths = showCutLengthsToggle.isOn;

        Toggle activeToggle = colorToggles.ActiveToggles().FirstOrDefault();
        Constants.trackColor = activeToggle.colors.normalColor;

        SceneManager.LoadScene("Main");
    }

    public void GameSettingsClick()
    {
        gameSettingsMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void PlayAgainClick()
    {
        gameSettingsMenu.SetActive(true);
        endgameMenu.SetActive(false);
    }

    public void TutorialClick()
    { //SceneManager.LoadScene("controls");
    }

    public void OptionsClick()
    {
        optionsMenu.SetActive(true);
        startMenu.SetActive(false);  
    }

    public void BackClick()
    {
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
            gapWidthImproperFractionsToggle.isOn = false;
            gapWidthImproperFractionsToggle.interactable = false;
            gapWidthMixedNumbersToggle.interactable = false;
            gapWidthMixedNumbersToggle.isOn = false;
        }
        else
        {
            gapWidthImproperFractionsToggle.isOn = true;
            gapWidthMixedNumbersToggle.isOn = true;
        }
        
    }
}

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
    [SerializeField] private Toggle gapWidthAlwaysAtomicToggle;
    [SerializeField] private Toggle gapWidthImproperFractionsToggle;
    [SerializeField] private Toggle gapWidthMixedNumbersToggle;
    [SerializeField] private ToggleGroup colorToggles;

    [SerializeField] private Text endgameText;

    [SerializeField] private Texture2D cursorTexture;


    private void Start()
    {
        Vector2 cursorHotSpot = new Vector2(cursorTexture.width * 0.1f, cursorTexture.height * 0.1f);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.ForceSoftware);
        if(Constants.gameOver)
        {
            startMenu.SetActive(false);
            endgameMenu.SetActive(true);
            endgameText.text = Constants.endgameText;
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
            gapWidthAlwaysAtomicToggle.isOn = false;
            gapWidthAlwaysAtomicToggle.interactable = false;
            gapWidthImproperFractionsToggle.isOn = false;
            gapWidthImproperFractionsToggle.interactable = false;
            gapWidthMixedNumbersToggle.interactable = false;
            gapWidthMixedNumbersToggle.isOn = false;
        }
        else
        {
            gapWidthAlwaysAtomicToggle.interactable = true;
            gapWidthImproperFractionsToggle.interactable = true;
            gapWidthMixedNumbersToggle.interactable = true;
        }
        
    }

    public void AlwaysAtomicCheck()
    {
        if (gapWidthAlwaysAtomicToggle.isOn)
        {
            gapWidthAlwaysOneToggle.isOn = false;
            gapWidthAlwaysOneToggle.interactable = false;
            gapWidthImproperFractionsToggle.isOn = false;
            gapWidthImproperFractionsToggle.interactable = false;
            gapWidthMixedNumbersToggle.interactable = false;
            gapWidthMixedNumbersToggle.isOn = false;
        }
        else
        {
            gapWidthAlwaysOneToggle.interactable = true;
            gapWidthImproperFractionsToggle.interactable = true;
            gapWidthMixedNumbersToggle.interactable = true;
        }
    }
}

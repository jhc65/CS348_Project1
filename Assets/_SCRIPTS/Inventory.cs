using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static private Inventory instance;      // instance of the GameController
    [SerializeField] private Text UIhalves;     // fine, Unity, I'll define them all on a 
    [SerializeField] private Text UIthirds;         // separate line if it'll stop your
    [SerializeField] private Text UIfourths;        // complaining
    [SerializeField] private Text UIfifths;
    [SerializeField] private Text UIsixths;
    [SerializeField] private Text UIsevenths;
    [SerializeField] private Text UIeighths;
    [SerializeField] private Text UIninths;
    [SerializeField] private Text UItenths;
    private int halves, thirds, fourths, fifths, sixths, sevenths, eighths, ninths, tenths;

    public static Inventory Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        RoundStart();
    }

    void RoundStart()
    {
        Set(10, 10, 10, 10, 10, 10, 10, 10, 10);
    }


    public void Decrease(Constants.Global.PieceLength piece)
    {
        switch (piece)
        {
            case Constants.Global.PieceLength.HALF:
                halves--;
                break;
            case Constants.Global.PieceLength.THIRD:
                thirds--;
                break;
            case Constants.Global.PieceLength.FOURTH:
                fourths--;
                break;
            case Constants.Global.PieceLength.FIFTH:
                fifths--;
                break;
            case Constants.Global.PieceLength.SIXTH:
                sixths--;
                break;
            case Constants.Global.PieceLength.SEVENTH:
                sevenths--;
                break;
            case Constants.Global.PieceLength.EIGHTH:
                eighths--;
                break;
            case Constants.Global.PieceLength.NINTH:
                ninths--;
                break;
            case Constants.Global.PieceLength.TENTH:
                tenths--;
                break;
        }
        UpdateUI();
    }

    public void Increase(Constants.Global.PieceLength piece, int num)
    {
        switch (piece)
        {
            case Constants.Global.PieceLength.HALF:
                halves += num;
                break;
            case Constants.Global.PieceLength.THIRD:
                thirds += num;
                break;
            case Constants.Global.PieceLength.FOURTH:
                fourths += num;
                break;
            case Constants.Global.PieceLength.FIFTH:
                fifths += num;
                break;
            case Constants.Global.PieceLength.SIXTH:
                sixths += num;
                break;
            case Constants.Global.PieceLength.SEVENTH:
                sevenths += num;
                break;
            case Constants.Global.PieceLength.EIGHTH:
                eighths += num;
                break;
            case Constants.Global.PieceLength.NINTH:
                ninths += num;
                break;
            case Constants.Global.PieceLength.TENTH:
                tenths += num;
                break;
        }
        UpdateUI();
    }

    public void Set(Constants.Global.PieceLength piece, int num)
    {
        switch (piece)
        {
            case Constants.Global.PieceLength.HALF:
                halves = num;
                break;
            case Constants.Global.PieceLength.THIRD:
                thirds = num;
                break;
            case Constants.Global.PieceLength.FOURTH:
                fourths = num;
                break;
            case Constants.Global.PieceLength.FIFTH:
                fifths = num;
                break;
            case Constants.Global.PieceLength.SIXTH:
                sixths = num;
                break;
            case Constants.Global.PieceLength.SEVENTH:
                sevenths = num;
                break;
            case Constants.Global.PieceLength.EIGHTH:
                eighths = num;
                break;
            case Constants.Global.PieceLength.NINTH:
                ninths = num;
                break;
            case Constants.Global.PieceLength.TENTH:
                tenths = num;
                break;
        }
        UpdateUI();
    }

    public void Set(int numHalves, int numThirds, int numFourths, int numFifths, int numSixths, int numSevenths, int numEighths, int numNinths, int numTenths)
    {
        halves = numHalves;
        thirds = numThirds;
        fourths = numFourths;
        fifths = numFifths;
        sixths = numSixths;
        sevenths = numSevenths;
        eighths = numEighths;
        ninths = numNinths;
        tenths = numTenths;
        UpdateUI();
    }

    void UpdateUI()
    {
        UIhalves.text = halves.ToString();
        UIthirds.text = thirds.ToString();
        UIfourths.text = fourths.ToString();
        UIfifths.text = fifths.ToString();
        UIsixths.text = sixths.ToString();
        UIsevenths.text = sevenths.ToString();
        UIeighths.text = eighths.ToString();
        UIninths.text = ninths.ToString();
        UItenths.text = tenths.ToString();
    }
}

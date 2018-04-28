using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    public enum CursorType { HAND, DRAG, CUT, PIECE };
    public enum PieceLength { HALF = 2, THIRD = 3, FOURTH = 4, FIFTH = 5, SIXTH = 6, SEVENTH = 7, EIGHTH = 8, NINTH = 9, TENTH = 10 };

    public static bool gapAlwaysOne = false;
    public static bool gapAllowImproperFractions = true;
    public static bool gapAllowMixedNumbers = false;
    public static bool gapAllowMultiple = false;
    public static bool unlimitedInventory = false;
    public static bool showCutLengths = false;
    public static Color trackColor = Color.white;
    public static Dictionary<PieceLength, float> difficultyProbabilityDistribution = new Dictionary<PieceLength, float>() 
                                { 
                                    {PieceLength.HALF,.3f},      //level: 1
                                    {PieceLength.THIRD,.2f},     //level: 2
                                    {PieceLength.FOURTH,.2f},     //level: 2
                                    {PieceLength.FIFTH,.05f},    //level: 3
                                    {PieceLength.SIXTH,.075f},    //level: 3
                                    {PieceLength.SEVENTH,.025f},   //level: 4
                                    {PieceLength.EIGHTH,.075f},    //level: 3
                                    {PieceLength.NINTH,.025f},   //level: 4
                                    {PieceLength.TENTH,.05f}    //level: 3
                                };
    public static float extraPiecesPerDifficultyLevel = 1.5f;
    public static string endgameText = "";
    public static bool gameOver = false;
    public static float fastCoasterSpeed = 1.25f;
    public static float slowCoasterSpeed = .25f;
    public static float masterVolume = -44f;
    public static float backgroundVolume = 1f;
    public static float effectsVolume = 1f;
    public static float backgroundPitch = 1f;
}
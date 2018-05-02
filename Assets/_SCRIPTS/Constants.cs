using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    public enum CursorType { HAND, DRAG, CUT, PIECE };
    public enum PieceLength { WHOLE = 1, HALF = 2, THIRD = 3, FOURTH = 4, FIFTH = 5, SIXTH = 6, SEVENTH = 7, EIGHTH = 8, NINTH = 9, TENTH = 10 };
    public enum Difficulty { EASY, MEDIUM, HARD, DEIFENBACH };

    public static Difficulty difficulty = Difficulty.DEIFENBACH;
    public static bool gapAlwaysAtomic = false;
    public static bool gapAlwaysOne = false;
    public static bool gapAllowImproperFractions = true;
    public static bool gapAllowMixedNumbers = false;
    public static bool gapAllowMultiple = false;
    public static bool unlimitedInventory = false;
    public static bool showCutLengths = false;
    public static Color trackColor = Color.white;
    public static float chanceToBreakPiece = .25f;
    public static float chanceToGiveExtraPiece = .33f;
    
    /* Chance to choose the given piece */
    public static Dictionary<PieceLength, float> pieceDistribution = new Dictionary<PieceLength, float>() 
                                {
                                    {PieceLength.WHOLE, 0f }, //Always break down 1's
                                    {PieceLength.HALF,.75f},
                                    {PieceLength.THIRD,.75f},
                                    {PieceLength.FOURTH,.50f},
                                    {PieceLength.FIFTH,.50f},
                                    {PieceLength.SIXTH,.50f},
                                    {PieceLength.SEVENTH,.30f},
                                    {PieceLength.EIGHTH,.30f},
                                    {PieceLength.NINTH,.30f},
                                    {PieceLength.TENTH,.30f}
                                };
    public static string endgameText = "";
    public static bool gameOver = false;
    public static float fastCoasterSpeed = 1.25f;
    public static float slowCoasterSpeed = .25f;
    public static float masterVolume = -40f;
    public static float backgroundVolume = 1f;
    public static float effectsVolume = 1f;
    public static float backgroundPitch = 1f;
}
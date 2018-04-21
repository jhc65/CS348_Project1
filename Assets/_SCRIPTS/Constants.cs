using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    public enum CursorType { HAND, DRAG, CUT, PIECE };
    public enum PieceLength { HALF, THIRD, FOURTH, FIFTH, SIXTH, SEVENTH, EIGHTH, NINTH, TENTH };

    public static bool gapAlwaysOne = false;
    public static bool gapAllowImproperFractions = true;
    public static bool gapAllowMixedNumbers = false;
    public static bool gapAllowMultiple = false;
    public static bool unlimitedInventory = false;
    public static bool showCutLengths = false;
    public static Color trackColor = Color.white;
}
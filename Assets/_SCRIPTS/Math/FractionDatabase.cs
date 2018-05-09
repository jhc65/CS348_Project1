using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class FractionDatabase {

    public Dictionary<int, List<FractionData>> Data;

    public FractionDatabase()
    {
        Data = new Dictionary<int, List<FractionData>>();
    }

    public FractionData GetRandomByDifficulty(Constants.Difficulty difficulty, bool forceImproper = false)
    {
        if (difficulty == Constants.Difficulty.EASY && forceImproper)
            throw new ArgumentException("Easy difficulty can't have improper fractions!");

        List<FractionData> fractionData;
        /* TODO: Medium should include Easy. Hard should include Medium & Easy. Etc */
        switch (difficulty)
        {
            case Constants.Difficulty.EASY:
                fractionData = Data[1];
                break;
            case Constants.Difficulty.MEDIUM:
                fractionData = Data[2];
                break;
            case Constants.Difficulty.HARD:
                fractionData = Data[3];
                break;
            case Constants.Difficulty.DEIFENBACH:
                fractionData = Data[4];
                break;
            default: /* Code shouldn't reach here */
                fractionData = Data[1];
                break;
        }

        /* Choose a random bit of data from the list */
        FractionData choice = fractionData[UnityEngine.Random.Range(0, fractionData.Count)];

        /* If forcing improper, loop until once is randomly chosen */
        while (forceImproper && choice.Value.numerator < choice.Value.denominator)
            choice = fractionData[UnityEngine.Random.Range(0, fractionData.Count)];

        return choice;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private GameObject[] buildZones;
    List<FractionTools.Fraction> gapSizes = new List<FractionTools.Fraction>();

    public List<FractionTools.Fraction> SetupBuildZones()
    {
        int ind = Random.Range(0, buildZones.Length);
        buildZones[ind].SetActive(true);
        FractionTools.Fraction newGapSize = ChooseGapSize();
        buildZones[ind].GetComponent<BuildZone>().SetGapSize(newGapSize);
        gapSizes.Add(newGapSize);

        //TODO: factor difficulty into this
        int coin = Random.Range(0, buildZones.Length);
        if (coin != ind)
        {
            buildZones[coin].SetActive(true);
            newGapSize = ChooseGapSize();
            buildZones[ind].GetComponent<BuildZone>().SetGapSize(newGapSize);
            gapSizes.Add(newGapSize);
        }

        return gapSizes;
    }

    private FractionTools.Fraction ChooseGapSize()
    {
        if (Constants.gapAlwaysOne)
            return new FractionTools.Fraction(1, 1);

        FractionTools.Fraction newGapSize = new FractionTools.Fraction(Random.Range(1, 5), Random.Range(2, 10));

        if (!Constants.gapAllowImproperFractions)
        {
            while (newGapSize.numerator > newGapSize.denominator)
            {
                newGapSize = new FractionTools.Fraction(Random.Range(1, 5), Random.Range(2, 10));
            }
        }

        return newGapSize;
    }
}

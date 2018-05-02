using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private GameObject[] buildZones;
    [SerializeField] private CoasterManager.SectionTriggers coasterAnimation;
    List<FractionTools.Fraction> gapSizes = new List<FractionTools.Fraction>();

    public List<FractionTools.Fraction> SetupBuildZones()
    {
        int ind = Random.Range(0, buildZones.Length);
        buildZones[ind].SetActive(true);
        FractionTools.Fraction newGapSize = FractionBuilder.ChooseGapSize();
        buildZones[ind].GetComponent<BuildZone>().SetGapSize(newGapSize);
        gapSizes.Add(newGapSize);

        //TODO: factor difficulty into this
        int coin = Random.Range(0, buildZones.Length);
        if (coin != ind)
        {
            buildZones[coin].SetActive(true);
            newGapSize = FractionBuilder.ChooseGapSize();
            buildZones[coin].GetComponent<BuildZone>().SetGapSize(newGapSize);
            gapSizes.Add(newGapSize);
        }

        return gapSizes;
    }

    public CoasterManager.SectionTriggers GetAnimationTrigger()
    {
        return coasterAnimation;
    }
}

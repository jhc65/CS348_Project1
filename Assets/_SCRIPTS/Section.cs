using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private SpriteRenderer track;
    [SerializeField] private GameObject[] buildZones;
    [SerializeField] private CoasterManager.SectionTriggers coasterAnimation;
    List<FractionTools.Fraction> gapSizes = new List<FractionTools.Fraction>();
    List<BuildZone> activeBuildZones = new List<BuildZone>();

    public List<FractionTools.Fraction> GapSizes
    {
        get { return gapSizes; }
    }

    void Start()
    {
        track.color = Constants.trackColor;
    }

    public List<BuildZone> SetupBuildZones()
    {
        if(Constants.difficulty == Constants.Difficulty.EASY)
        {
            for (int i = 1; i < buildZones.Length; i+=2)
            {
                buildZones[i].SetActive(true);
                FractionTools.Fraction newGapSize = FractionBuilder.ChooseGapSize();
                buildZones[i].GetComponent<BuildZone>().SetGapSize(newGapSize);
                activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
                gapSizes.Add(newGapSize);
            }
        }
        else if (Constants.difficulty == Constants.Difficulty.MEDIUM)
        {
            for (int i = 1; i < buildZones.Length; i++)
            {
                if (i == 2)
                    continue;
                buildZones[i].SetActive(true);
                FractionTools.Fraction newGapSize = FractionBuilder.ChooseGapSize();
                buildZones[i].GetComponent<BuildZone>().SetGapSize(newGapSize);
                activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
                gapSizes.Add(newGapSize);
            }
        }
        else if (Constants.difficulty == Constants.Difficulty.HARD || Constants.difficulty == Constants.Difficulty.DEIFENBACH)
        {
            for (int i = 0; i < buildZones.Length; i++)
            {
                buildZones[i].SetActive(true);
                FractionTools.Fraction newGapSize = FractionBuilder.ChooseGapSize();
                buildZones[i].GetComponent<BuildZone>().SetGapSize(newGapSize);
                activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
                gapSizes.Add(newGapSize);
            }
        }

        return activeBuildZones;
    }

    public CoasterManager.SectionTriggers GetAnimationTrigger()
    {
        return coasterAnimation;
    }
}

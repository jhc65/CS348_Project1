using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private SpriteRenderer track;
    [SerializeField] private GameObject[] buildZones;
    [SerializeField] private CoasterManager.SectionTriggers coasterAnimation;
    private List<BuildZone> activeBuildZones = new List<BuildZone>();

    void Start()
    {
        track.color = Constants.trackColor;
    }

    public List<BuildZone> SetupBuildZones()
    {
        List<FractionData> data = new List<FractionData>();
        if(Constants.difficulty == Constants.Difficulty.EASY)
        {
            for (int i = 1; i < buildZones.Length; i+=2)
            {
                buildZones[i].SetActive(true);
                activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
            }
        }
        else if (Constants.difficulty == Constants.Difficulty.MEDIUM)
        {
            for (int i = 1; i < buildZones.Length; i++)
            {
                if (i == 2)
                    continue;
                buildZones[i].SetActive(true);
                activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
            }
        }
        else if (Constants.difficulty == Constants.Difficulty.HARD || Constants.difficulty == Constants.Difficulty.DEIFENBACH)
        {
            for (int i = 0; i < buildZones.Length; i++)
            {
                buildZones[i].SetActive(true);
                activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
            }
        }

        foreach (BuildZone bz in activeBuildZones)
        {
            FractionData fractionData = Constants.fractionDatabase.GetRandomByDifficulty(Constants.difficulty
                , (!Constants.gapAllowImproperFractions && !Constants.gapAllowMixedNumbers)
                , Constants.gapAlwaysOne
                , Constants.gapAlwaysAtomic);
            bz.GetComponent<BuildZone>().SetFractionData(fractionData);
            data.Add(fractionData);
        }

        return activeBuildZones;
    }

    public CoasterManager.SectionTriggers GetAnimationTrigger()
    {
        return coasterAnimation;
    }
}

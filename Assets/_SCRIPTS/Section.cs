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
        while (activeBuildZones.Count == 0) // guarantee at least one build zone is active per section
        {
            for (int i = 0; i < buildZones.Length; i++)
            {
                //TODO: factor difficulty into this
                if (Random.Range(0f, 1f) <= .5f)
                {
                    buildZones[i].SetActive(true);
                    FractionTools.Fraction newGapSize = FractionBuilder.ChooseGapSize();
                    buildZones[i].GetComponent<BuildZone>().SetGapSize(newGapSize);
                    activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
                    gapSizes.Add(newGapSize);
                }
            }
        }
        return activeBuildZones;
    }

    public CoasterManager.SectionTriggers GetAnimationTrigger()
    {
        return coasterAnimation;
    }
}

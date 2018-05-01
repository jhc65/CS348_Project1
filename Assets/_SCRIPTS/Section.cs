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
                    FractionTools.Fraction newGapSize = ChooseGapSize();
                    buildZones[i].GetComponent<BuildZone>().SetGapSize(newGapSize);
                    activeBuildZones.Add(buildZones[i].GetComponent<BuildZone>());
                    gapSizes.Add(newGapSize);
                }
            }
        }
        // guarantee one build zone is active per section


        //int ind = Random.Range(0, buildZones.Length);
        //buildZones[ind].SetActive(true);
        //FractionTools.Fraction newGapSize = ChooseGapSize();
        //buildZones[ind].GetComponent<BuildZone>().SetGapSize(newGapSize);
        //activeBuildZones.Add(buildZones[ind].GetComponent<BuildZone>());
        //gapSizes.Add(newGapSize);

        ////TODO: factor difficulty into this
        //int coin = Random.Range(0, buildZones.Length);
        //if (coin != ind)
        //{
        //    buildZones[coin].SetActive(true);
        //    newGapSize = ChooseGapSize();
        //    buildZones[coin].GetComponent<BuildZone>().SetGapSize(newGapSize);
        //    activeBuildZones.Add(buildZones[coin].GetComponent<BuildZone>());
        //    gapSizes.Add(newGapSize);
        //}

        //return gapSizes;
        return activeBuildZones;
    }

    private FractionTools.Fraction ChooseGapSize()
    {
        if (Constants.gapAlwaysOne)
            return FractionTools.Fraction.One;
        if (Constants.gapAlwaysAtomic)
            return new FractionTools.Fraction(1, Random.Range(2, 10));

        FractionTools.Fraction newGapSize = new FractionTools.Fraction(Random.Range(1, 5), Random.Range(2, 10));

        if (!Constants.gapAllowImproperFractions && !Constants.gapAllowMixedNumbers)
        {
            while (newGapSize.numerator > newGapSize.denominator)
            {
                newGapSize = new FractionTools.Fraction(Random.Range(1, 5), Random.Range(2, 10));
            }
        }

        return newGapSize;
    }

    public CoasterManager.SectionTriggers GetAnimationTrigger()
    {
        return coasterAnimation;
    }
}

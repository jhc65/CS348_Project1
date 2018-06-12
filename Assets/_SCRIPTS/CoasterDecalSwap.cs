using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoasterDecalSwap : MonoBehaviour
{
    CoasterManager coaster;

    private void Start()
    {
        coaster = CoasterManager.Instance;
    }

    private void OnMouseDown()
    {
        Constants.decalIndex++;
        Constants.decalIndex = Constants.decalIndex % coaster.decals.Length;
        coaster.decalSprites[0].sprite = coaster.frontDecals[Constants.decalIndex];
        for(int i = 1; i<coaster.decalSprites.Length; i++)
            coaster.decalSprites[i].sprite = coaster.decals[Constants.decalIndex];
    }
}

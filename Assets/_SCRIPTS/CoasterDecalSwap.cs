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
        Constants.decalIndex = Constants.decalIndex % coaster.decalSprites.Length;
        foreach (SpriteRenderer sp in coaster.decalSprites)
            sp.sprite = coaster.decals[Constants.decalIndex];
    }
}

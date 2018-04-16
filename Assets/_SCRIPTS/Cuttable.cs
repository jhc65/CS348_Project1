using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{

    [SerializeField] private GameObject resultOfCut;

    void OnMouseOver()
    {
        resultOfCut.SetActive(true);
    }

    void OnMouseExit()
    {
        resultOfCut.SetActive(false);
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    [SerializeField] private GameObject resultOfCut;
    [SerializeField] private GameObject resultOfCutLengths;
    [SerializeField] private Constants.PieceLength parentLength;
    [SerializeField] private Constants.PieceLength cutLength;
    [SerializeField] private int numCuts;
    private GameController gc;
    private Inventory inv;


    void Start()
    {
        gc = GameController.Instance;
        inv = Inventory.Instance;
    }

    void OnMouseOver()
    {
        // ignore hover if a menu is active
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        if (gc.ActiveCursor == Constants.CursorType.CUT)
        {
            resultOfCut.SetActive(true);
            if (Constants.showCutLengths) resultOfCutLengths.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                // Zak's code to change inventory numbers
                inv.Decrease(parentLength);
                inv.Increase(cutLength, numCuts);
                /* Play sound effect */
                EffectsManager.Instance.PlayEffect(EffectsManager.Effects.CutTool);
            }
            if (Input.GetMouseButtonDown(1))
            {
                resultOfCut.SetActive(false);
                resultOfCutLengths.SetActive(false);
            }
        }
    }

    void OnMouseExit()
    {
        if (gc.ActiveCursor == Constants.CursorType.CUT)
        {
            resultOfCut.SetActive(false);
            resultOfCutLengths.SetActive(false);
        }
    }

    private void OnDisable()
    {
        resultOfCut.SetActive(false);
        resultOfCutLengths.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    [SerializeField] private GameObject resultOfCut;
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
        if (gc.ActiveCursor == Constants.CursorType.CUT)
        {
            resultOfCut.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                // Zak's code to change inventory numbers
                inv.Decrease(parentLength);
                inv.Increase(cutLength, numCuts);
            }
            if (Input.GetMouseButtonDown(1))
            {
                resultOfCut.SetActive(false);
            }
        }
    }

    void OnMouseExit()
    {
        if (gc.ActiveCursor == Constants.CursorType.CUT)
        {
            resultOfCut.SetActive(false);
        }
    }

    private void OnDisable()
    {
        resultOfCut.SetActive(false);
    }
}

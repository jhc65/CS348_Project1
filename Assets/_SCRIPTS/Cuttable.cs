using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    [SerializeField] private GameObject resultOfCut;
    private GameController gc;


    void Start()
    {
        gc = GameController.Instance;
    }

    void OnMouseOver()
    {
        if (gc.ActiveCursor == GameController.CursorType.CUT)
        {
            resultOfCut.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                // Zak's code to change inventory numbers
                //Inventory.Decrease(enum-for-this-piece) <-- calls ui change too
                //Inventory.Increase(enum-for-increased-piece, number-to-add) <-- calls ui change too
            }
            if (Input.GetMouseButtonDown(1))
            {
                resultOfCut.SetActive(false);
            }
        }
    }

    void OnMouseExit()
    {
        if (gc.ActiveCursor == GameController.CursorType.CUT)
        {
            resultOfCut.SetActive(false);
        }
    }
}

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
        resultOfCut.SetActive(true);
    }

    void OnMouseExit()
    {
        resultOfCut.SetActive(false);
    }

    void OnMouseDown()
    {
        if(gc.ActiveCursor == GameController.CursorType.CUT)
        {
            // Zak's code to change inventory numbers
        }
        else if(gc.ActiveCursor == GameController.CursorType.HAND)
        {
            // Joe's code to change the cursor and stuff
        }
    }

}

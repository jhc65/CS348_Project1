using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] GameObject draggable;
    [SerializeField] GameObject cuttables;
    [SerializeField] SpriteRenderer sprite;

    public void EnableDraggable()
    {
        draggable.SetActive(true);
        cuttables.SetActive(false);
    }

    public void EnableCuttables()
    {
        cuttables.SetActive(true);
        draggable.SetActive(false);
    }

    private void Start()
    {
        sprite.color = Constants.trackColor;
    }
}

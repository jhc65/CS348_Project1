using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPiece : MonoBehaviour
{
    private Inventory inv;
    [SerializeField] private Constants.PieceLength length;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Transform destination;
    [SerializeField] private float speed;

    void Start()
    {
        inv = Inventory.Instance;
        sprite.color = Constants.trackColor;

        destination = inv.pieces[(int)length - 2].transform;

        //switch(length)
        //{
        //    case Constants.PieceLength.HALF:

        //        break;
        //    case Constants.PieceLength.THIRD:

        //        break;
        //    case Constants.PieceLength.FOURTH:

        //        break;
        //    case Constants.PieceLength.FIFTH:

        //        break;
        //    case Constants.PieceLength.SIXTH:

        //        break;
        //    case Constants.PieceLength.SEVENTH:

        //        break;
        //    case Constants.PieceLength.EIGHTH:

        //        break;
        //    case Constants.PieceLength.NINTH:

        //        break;
        //    case Constants.PieceLength.TENTH:

        //        break;
        //}
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * speed);

        if (IsWithin(transform.position, destination.position))     // destroy when destination is reached
        {
            Destroy(gameObject);
            inv.Increase(length, 1);
        }
    }

    private bool IsWithin(Vector3 obj1, Vector3 obj2)
    {
        float tolerance = 0.15f;
        if ((Mathf.Abs(obj1.x - obj2.x) < tolerance) && (Mathf.Abs(obj1.y - obj2.y) < tolerance))
            return true;
        else
            return false;
    }
}

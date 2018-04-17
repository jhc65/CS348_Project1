using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildZone : MonoBehaviour {

    private FractionTools.Fraction _fractionGap;
    private FractionTools.Fraction _fractionFilled;

    /// TODO: These should maybe be class references instead of gameObjects
    private List<GameObject> _piecesInBuildZone;
    private GameObject _pieceInBuildZone = null;

    public Vector2 InitialSnapPoint;
    [Range(0f,1f)]
    public float SnapSpeed = .75f;
    public float SnapAnimationThreshold;

	// Use this for initialization
	void Start () {
		/// TODO: Generate a pseudo-random improper fraction, or get one from a GameManager
        
        /* Initialize _fractionFilled to 0 */
        _fractionFilled = new FractionTools.Fraction(0, 0);
        _piecesInBuildZone = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        /// TODO: None of this should probably be the build area's job
        /* If the mouse button is released, check if a piece has been dropped in this build area */
		if(Input.GetMouseButtonUp(0) && _pieceInBuildZone != null)
        {
            ///TODO: Get the piece script
            FractionTools.Fraction f = new FractionTools.Fraction();
            /* Check if the new piece will fit */
            if (_fractionFilled + f < _fractionGap)
            {
                /* Snap the piece */
                /* If this is the first piece, pass null and leave that job to the snap coroutine */
                if (_piecesInBuildZone.Count == 0)
                    StartCoroutine(SnapPiece(_pieceInBuildZone, null));
                else
                    StartCoroutine(SnapPiece(_pieceInBuildZone, _piecesInBuildZone[_piecesInBuildZone.Count - 1]));
                /* Add this piece to the list */
                _piecesInBuildZone.Add(_pieceInBuildZone);
                /* Clear out reference to _pieceInBuildZone */
                _pieceInBuildZone = null;
            }
            else
            {
                /// TODO: Animate the piece back to the inventory
            }
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        /// TODO: Ensure the gameObject entering is a game piece

        _pieceInBuildZone = coll.gameObject;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        /* If the active piece exits the build area, forget about it */
        if (_pieceInBuildZone != null && _pieceInBuildZone == coll.gameObject)
            _pieceInBuildZone = null;
    }

    private IEnumerator SnapPiece(GameObject newPiece, GameObject previousPiece)
    {
        /// TODO: previous piece + offset of previous piece's size
        Vector2 target = previousPiece == null ? InitialSnapPoint : (Vector2)previousPiece.transform.position;
        Quaternion targetRot = previousPiece.transform.rotation;
        /// TODO: Animate the piece snapping into place
        //while (Vector2.Distance((Vector2)newPiece.transform.position, target) > SnapAnimationThreshold)
        //{
        //    /// TODO: Slerp or Lerp the position and rotation

        //    /* Small wait for the animation */
        //    yield return new WaitForSeconds(0.1f);
        //}
        newPiece.transform.SetPositionAndRotation(
            new Vector3(
                target.x,
                target.y,
                newPiece.transform.position.z),
            targetRot);
        yield return null;
    }
}

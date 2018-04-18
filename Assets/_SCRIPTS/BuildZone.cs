using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildZone : MonoBehaviour {

	/*
	 * Need to have numerator and denominator vars since Inspector won't display Fraction struct by default
	 *
	 * I will write an editor script to resolve this workaround eventually... (Corwin)
	 */
	[SerializeField] private int _gapNumerator;
	[SerializeField] private int _gapDenominator;
	private FractionTools.Fraction _gapSize;
	private FractionTools.Fraction _gapFilled;
	private List<Piece> _piecesInZone;

	// Use this for initialization
	void Start () {
		_piecesInZone = new List<Piece>();
		_gapFilled = new FractionTools.Fraction(0, 0);
		_gapSize = new FractionTools.Fraction(_gapNumerator, _gapDenominator);
	}
	
	public bool TryPlacePiece(Piece p)
	{
		//Debug.Log("Trying to place the piece...");
		//Debug.Log("Gap: " + _gapSize + ", piece:" + p.Value + ", filled: " + _gapFilled);
		bool successful = false;

		if (p.Value + _gapFilled <= _gapSize)
		{
			successful = true;
			SnapPiece(p);
			_piecesInZone.Add(p);
			_gapFilled += p.Value;
			/* Check if the gap has been filled */
			//Debug.Log("Gap filled: " + _gapFilled + ", gap size: " + _gapSize);
			if (_gapFilled == _gapSize)
				GameController.Instance.OnGapFilled();
		}
		else{
			Debug.Log("Piece doesn't want to take a fit! Gap filled: " + _gapFilled + ", piece size: " + p.Value + ", gap size: " + _gapSize);
		}

		return successful;
	}

	private void SnapPiece(Piece p)
	{
		//Debug.Log("Attempting to snap piece...");
		Vector3 targetPos;
		Quaternion targetRot;
		if (_piecesInZone.Count == 0)
		{
			//Debug.Log("setting snap point to SnapStart");
			Transform t = this.transform.Find("SnapStart").transform;
			targetPos = t.position;
			targetPos.x += t.localScale.x;
			targetRot = t.rotation;
		}
		else
		{
			//Debug.Log("Setting snap point to previous piece");
			Transform t = _piecesInZone[_piecesInZone.Count - 1].transform;
			targetPos = t.position;
			targetPos.x += t.localScale.x;
			targetRot = t.rotation;
		}
		//Debug.Log("Snapping to: " + targetPos + ", " + targetRot.eulerAngles);

		/// TODO: Animate this
		/* Move and rotate the piece */
		p.transform.SetPositionAndRotation(targetPos, targetRot);
		/* Scale the piece down to within the build area
		 * 
		 * pieceSize (1/2) / _gapSize (3/2) = Percent to fill 1/3
		 * pieceScale * (PercentToFill * BuildZoneScale) = newScale
		*/
		/* Putting this in a try catch made it run...I have no idea why it did not run without this
				Will look into later (Corwin) */
		try
		{
			float PercentToFill = (float)(p.Value / _gapSize);
			//Debug.Log("PercentToFill: " + PercentToFill);
			p.transform.localScale = new Vector3(
			(PercentToFill * this.transform.localScale.x),
			p.transform.localScale.y,
			p.transform.localScale.z);
		}
		catch (System.Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
		
		//yield return null; // For when this becomes a coroutine
	}

	public void SetGapSize(FractionTools.Fraction value)
	{
		_gapNumerator = value.numerator;
		_gapDenominator = value.denominator;
		_gapSize = new FractionTools.Fraction(value);
	}

	public void ClearBuildZone()
	{
		/* Clear out _gapFilled */
		_gapFilled = new FractionTools.Fraction(0, 0);
		/* Clear out _piecesInZone */
		if (_piecesInZone.Count > 0)
		{
			for (int i = _piecesInZone.Count - 1; i >= 0; i--)
			{
				GameObject.Destroy(_piecesInZone[i].gameObject);
				_piecesInZone.RemoveAt(i);
			}
		}
	}
}

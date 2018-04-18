using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildZone : MonoBehaviour {

	public int GapNumerator;
	public int GapDenominator;
	public FractionTools.Fraction GapSize;
	private FractionTools.Fraction _gapFilled;
	private List<Piece> _piecesInZone;

	// Use this for initialization
	void Start () {
		_piecesInZone = new List<Piece>();
		_gapFilled = new FractionTools.Fraction(0, 0);
		GapSize = new FractionTools.Fraction(GapNumerator, GapDenominator);
	}
	
	public bool TryPlacePiece(Piece p)
	{
		Debug.Log("Trying to place the piece...");
		Debug.Log("Gap: " + GapSize + ", piece:" + p + ", filled: " + _gapFilled);
		bool successful = false;

		if (p.Value + _gapFilled < GapSize)
		{
			successful = true;
		}

		return successful;
	}

	private IEnumerator SnapPiece(Piece p)
	{
		Vector3 targetPos;
		Quaternion targetRot;
		if (_piecesInZone.Count == 0)
		{
			Transform t = this.transform.Find("SnapStart").transform;
			targetPos = t.position;
			targetPos.x += t.localScale.x;
			targetRot = t.rotation;
		}
		else
		{
			Transform t = _piecesInZone[_piecesInZone.Count - 1].transform;
			targetPos = t.position;
			targetPos.x += t.localScale.x;
			targetRot = t.rotation;
		}

		/// TODO: Animate this
		p.transform.SetPositionAndRotation(targetPos, targetRot);
		
		yield return null;
	}
}

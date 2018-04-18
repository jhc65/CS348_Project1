﻿using System.Collections;
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
		//Debug.Log("Trying to place the piece...");
		//Debug.Log("Gap: " + GapSize + ", piece:" + p.Value + ", filled: " + _gapFilled);
		bool successful = false;

		if (p.Value + _gapFilled < GapSize)
		{
			successful = true;
			SnapPiece(p);
			_piecesInZone.Add(p);
		}
		else{
			//Debug.Log("Piece doesn't want to take a fit");
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
		 * pieceSize (1/2) / GapSize (3/2) = Percent to fill 1/3
		 * pieceScale * (PercentToFill * BuildZoneScale) = newScale
		*/
		/* Putting this in a try catch made it run...I have no idea why it did not run without this
				Will look into later (Corwin) */
		try
		{
			float PercentToFill = (float)(p.Value / GapSize);
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
}

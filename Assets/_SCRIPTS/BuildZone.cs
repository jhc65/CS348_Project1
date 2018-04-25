using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildZone : MonoBehaviour {

    public enum PivotType {Left, Center, Right};
    [SerializeField]
    private PivotType SnapPivot;
	/*
	 * Need to have numerator and denominator vars since Inspector won't display Fraction struct by default
	 *
	 * I will write an editor script to resolve this workaround eventually... (Corwin)
	 */
	[SerializeField] private int _gapNumerator;
	[SerializeField] private int _gapDenominator;
    [SerializeField] private Text _equation;
    private FractionTools.Fraction _gapSize;
	private FractionTools.Fraction _gapFilled;
	private List<Placeable> _piecesInZone;

	// Use this for initialization
	void Start () {
		_piecesInZone = new List<Placeable>();
		_gapFilled = FractionTools.Fraction.Zero();
		//_gapSize = new FractionTools.Fraction(_gapNumerator, _gapDenominator);
		//_equation = this.GetComponentInChildren<Text>();
		//UpdateEquationUI(); /* Set the initial equation */
	}
	
	public bool TryPlacePiece(Placeable p)
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
			UpdateEquationUI();
			/* Check if the gap has been filled */
			//Debug.Log("Gap filled: " + _gapFilled + ", gap size: " + _gapSize);
			if (_gapFilled == _gapSize)
				StartCoroutine(GameController.Instance.OnGapFilled());
		}
		else{
			Debug.Log("Piece doesn't want to take a fit! Gap filled: " + _gapFilled + ", piece size: " + p.Value + ", gap size: " + _gapSize);
		}

		return successful;
	}

	private void SnapPiece(Placeable p)
	{
		Vector3 targetPos;
        /* Scale the piece down to within the build area
		 * 
		 * pieceSize (1/2) / _gapSize (3/2) = Percent to fill 1/3
		 */
		float PercentToFill = (float)(p.Value / _gapSize);

        /* Set the previousTransform to either SnapStart, or the previous piece */
		if (_piecesInZone.Count == 0)
		{
            /* Setting target to SnapPoint's local transform */
			Transform t = this.transform.Find("SnapStart").transform;
			targetPos = t.localPosition;
            switch (SnapPivot)
            {
                case PivotType.Left:
                    break;
                case PivotType.Center:
                    targetPos.x += PercentToFill / 2;
                    break;
                case PivotType.Right:
                    targetPos.x += PercentToFill;
                    break;
            }
		}
		else
		{
			/* Setting target to previous piece + previous piece's length */
            Placeable previous = _piecesInZone[_piecesInZone.Count - 1];
            targetPos = previous.transform.localPosition;
            switch (SnapPivot)
            {
                case PivotType.Left:
                    targetPos.x += (float)(previous.Value / _gapSize);
                    break;
                case PivotType.Center:
                    targetPos.x += (float)(previous.Value / _gapSize) / 2;
                    targetPos.x += PercentToFill / 2;
                    break;
                case PivotType.Right:
                    targetPos.x += PercentToFill;
                    break;
            }
		}

		/// TODO: Animate this
		/* Set the piece as a child of this build zone, then move and rotate the piece */
        p.transform.SetParent(this.transform); /* Make the piece a child of the parent */
        p.transform.localRotation = Quaternion.identity; /* Set the local rotation to identity (0,0,0) */
        Debug.Log("<color=blue>" + p.transform.localPosition + ", " + targetPos);
        p.transform.localPosition = targetPos; /* Set the local position to target */
		
        Vector3 scale = p.transform.localScale;
        p.transform.localScale = new Vector3(PercentToFill, scale.y, scale.z);
		
		//yield return null; // For when this becomes a coroutine
	}

	public void SetGapSize(FractionTools.Fraction value)
	{
		_gapNumerator = value.numerator;
		_gapDenominator = value.denominator;
		_gapSize = new FractionTools.Fraction(value);

		UpdateEquationUI();
	}

	public void ClearBuildZone()
	{
		/* Clear out _gapFilled */
		_gapFilled = FractionTools.Fraction.Zero();
		/* Clear out _gapSize */
		_gapSize = FractionTools.Fraction.Zero();
		/* Clear out _piecesInZone */
		if (_piecesInZone.Count > 0)
		{
			for (int i = _piecesInZone.Count - 1; i >= 0; i--)
			{
				GameObject.Destroy(_piecesInZone[i].gameObject);
				_piecesInZone.RemoveAt(i);
			}
		}
		/* Clear out Equation UI */
		UpdateEquationUI();
	}

	private string GapEquation()
	{
		string result = "";
		
		if (_piecesInZone.Count > 0)
		{
			/* Append each piece's fraction using addition */
			foreach(Placeable p in _piecesInZone)
				result += p.Value + " + ";

			/* Remove the last + */
			result = result.Remove(result.Length - 3);

		}
		else
		{
			/* Put some question marks? */
			result += "? + ?";
		}

		if (_gapFilled != _gapSize)
		{
			/* Append the "you aren't done yet" part */
			result += " + ...";
		}

		/* Append the total gap size */
		result += " = " + _gapSize;

		return result;
	}

	private void UpdateEquationUI()
	{
		_equation.text = GapEquation();
	}
}

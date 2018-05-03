﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildZone : MonoBehaviour {

    public enum PivotType {Left, Center, Right};
    [SerializeField]
    private PivotType SnapPivot;
    [SerializeField] Transform snapStart;
    /*
	 * Need to have numerator and denominator vars since Inspector won't display Fraction struct by default
	 *
	 * I will write an editor script to resolve this workaround eventually... (Corwin)
	 */

    [SerializeField] private int gapNumerator;
	[SerializeField] private int gapDenominator;
    [SerializeField] private Text equation;
    [SerializeField] private GameObject slowDownTrigger;
    [SerializeField] private SpriteMask gapMask;
    [SerializeField] private GameObject interactable;
    [SerializeField] private GameObject sparkle;
    [SerializeField] private GameObject dustCloud;
    [SerializeField] private AudioSource explosion;
    public FractionTools.Fraction gapSize;
	private FractionTools.Fraction gapFilled = FractionTools.Fraction.Zero;
    private List<Placeable> piecesInZone = new List<Placeable>();
    private Inventory inv;
	
	public bool TryPlacePiece(Placeable p)
	{
		//Debug.Log("Trying to place the piece...");
		//Debug.Log("Gap: " + gapSize + ", piece:" + p.Value + ", filled: " + gapFilled);
		bool successful = false;

		if (p.Value + gapFilled <= gapSize)
		{
			successful = true;
            EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Snap);
			SnapPiece(p);
			piecesInZone.Add(p);
			gapFilled += p.Value;
            gapMask.transform.localScale = new Vector3(4 * (1f - (float)(gapFilled / gapSize)), 2, 1);
            UpdateEquationUI();
			/* Check if the gap has been filled */
			//Debug.Log("Gap filled: " + gapFilled + ", gap size: " + gapSize);
            if (gapFilled == gapSize)
            {
                /* Disable the slow zone in case the coaster didn't even hit it yet (player was really quick) */
                if (slowDownTrigger != null)
                    slowDownTrigger.SetActive(false);
                /* Notify the GameController that a gap has been filled */
                StartCoroutine(GameController.Instance.OnGapFilled());
            }
		}
		else{
			Debug.Log("Piece doesn't want to take a fit! Gap filled: " + gapFilled + ", piece size: " + p.Value + ", gap size: " + gapSize);
            EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Incorrect);
		}

		return successful;
	}

	private void SnapPiece(Placeable p)
	{
        GameController.Instance.LastInteractedBuildZone = this;
        Vector3 targetPos;
        /* Scale the piece down to within the build area
		 * 
		 * pieceSize (1/2) / gapSize (3/2) = Percent to fill 1/3
		 */
		float PercentToFill = (float)(p.Value / gapSize);

        /* Set the previousTransform to either SnapStart, or the previous piece */
		if (piecesInZone.Count == 0)
		{
            /* Setting target to SnapPoint's local transform */
			targetPos = snapStart.localPosition;
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
            Placeable previous = piecesInZone[piecesInZone.Count - 1];
            targetPos = previous.transform.localPosition;
            switch (SnapPivot)
            {
                case PivotType.Left:
                    targetPos.x += (float)(previous.Value / gapSize);
                    break;
                case PivotType.Center:
                    targetPos.x += (float)(previous.Value / gapSize) / 2;
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
        //Debug.Log("<color=blue>" + p.transform.localPosition + ", " + targetPos);
        p.transform.localPosition = targetPos; /* Set the local position to target */
		
        Vector3 scale = p.transform.localScale;
        /* This black magic is what the local x scale of the piece needs to be to fill the entire build zone
         *  When we get a build zone sprite, it won't have a 4x scale itself, and this will need to change*/
        float scaleToGapWidth = (float)p.Value.denominator / 16f;
        p.transform.localScale = new Vector3(scaleToGapWidth * PercentToFill, scale.y, scale.z);
        p.gameObject.SetActive(false);
		
		//yield return null; // For when this becomes a coroutine
	}

	public void SetGapSize(FractionTools.Fraction value)
	{
		gapNumerator = value.numerator;
		gapDenominator = value.denominator;
		gapSize = new FractionTools.Fraction(value);

		UpdateEquationUI();
	}

	public void ClearBuildZone()
	{
		/* Clear out gapFilled */
		gapFilled = FractionTools.Fraction.Zero;
		/* Clear out gapSize */
		gapSize = FractionTools.Fraction.Zero;
		/* Clear out piecesInZone */
		if (piecesInZone.Count > 0)
		{
			for (int i = piecesInZone.Count - 1; i >= 0; i--)
			{
				GameObject.Destroy(piecesInZone[i].gameObject);
				piecesInZone.RemoveAt(i);
			}
		}
		/* Clear out Equation UI */
		UpdateEquationUI();
	}

    public void Activate()
    {
        interactable.SetActive(true);
    }

    public void Sparkle()
    {
        Instantiate(sparkle,transform.position, Quaternion.identity);
    }

    public void HideBuildZone()
    {
        interactable.SetActive(false);
    }

	private string GapEquation()
	{
		string result = "";

        if (piecesInZone.Count == 0)
        {
            /* Put a question mark */
            result += "?";
        }
        else
        {
            /* Append each piece's fraction using addition */
            foreach (Placeable p in piecesInZone)
                result += p.Value + " + ";

            /* Remove the last + */
            result = result.Remove(result.Length - 3);

            if (gapFilled != gapSize)
            {
                /* Append the "you aren't done yet" part */
                result += " + ...";
            }
        }

        /* Append the total gap size */
        if (gapSize == FractionTools.Fraction.One)
            result += " = 1";
        else
        {
            if(!Constants.gapAllowImproperFractions)
            {
                // if improper fractions not allowed, must display as mixed number
                FractionTools.MixedNumber gapAsMixedNumber = gapSize.ToMixedNumber();
                result += " = " + gapAsMixedNumber;
            }
            else if (Constants.gapAllowImproperFractions && Constants.gapAllowMixedNumbers)
            {
                // if both improper fractions and mixed numbers are allowed, randomly choose how to display
                if (Random.Range(0f, 1f) <= .5f)
                {
                    FractionTools.MixedNumber gapAsMixedNumber = gapSize.ToMixedNumber();
                    result += " = " + gapAsMixedNumber;
                }
            }
            else
            {
                // just display as improper fraction
                result += " = " + gapSize;
            }
        }
        return result;
    }

	private void UpdateEquationUI()
	{
		equation.text = GapEquation();
	}

    public bool IsGapFilled()
    {
        return gapFilled == gapSize;
    }

    public void OnUndoButtonClicked()
    {
        if (piecesInZone.Count > 0 && (gapFilled < gapSize)) {
            gapFilled -= piecesInZone[piecesInZone.Count - 1].Value;
            inv.Increase(piecesInZone[piecesInZone.Count - 1].Length, 1);
            GameObject.Destroy(piecesInZone[piecesInZone.Count - 1].gameObject);
            piecesInZone.RemoveAt(piecesInZone.Count - 1);
            gapMask.transform.localScale = new Vector3(4 * (1f - (float)(gapFilled / gapSize)), 2, 1);
            UpdateEquationUI();
        }
    }

    public void OnClearButtonClicked()
    {
        /* Toggle on the dust cloud particle effect */
        dustCloud.SetActive(true);
        explosion.Play();
        while (piecesInZone.Count > 0 && gapFilled < gapSize) {
            OnUndoButtonClicked();
        }
    }

    private void Awake() {
        inv = Inventory.Instance;
    }
}

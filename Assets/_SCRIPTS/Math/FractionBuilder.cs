using Enum = System.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Fraction = FractionTools.Fraction; // Add shorthand way to use Fraction
using PieceLength = Constants.PieceLength;

public static class FractionBuilder
{
    /// <summary>
    /// Breaks a given fraction into multiple pieces to populate the players inventory
    /// </summary>
    /// <param name="fraction">The fraction to break into pieces</param>
    /// <param name="forbidCutting">If true, all pieces will be less than or equal to the starting fraction</param>
    /// <returns></returns>
    /// <remarks>TODO: Difficulty is not being used yet. Find an appropriate place to include it</remarks>
    public static PieceLength[] BreakMyLifeIntoPieces(Fraction fraction, bool forbidCutting = false)
    {
        /* If given an atomic fraction, return it as-is */
        if (Constants.gapAlwaysAtomic)
            return new PieceLength[1] { (PieceLength)fraction.denominator };

        /* Otherwise convert the fraction into atomic pieces */
        List<Fraction> atoms = ConvertFractionToAtoms(fraction);
        List<Fraction> extraPieces = new List<Fraction>(); /* Used for Hard mode */

        List<PieceLength> finalSelection = new List<PieceLength>();

        /* If not on easy mode, go through changing the atoms into other bases randomly */
        if (Constants.difficulty != Constants.Difficulty.EASY)
        {
            /* for each atom, decide whether to break it down or not based on difficulty */
            for (int i = 0; i < atoms.Count; )
            {
                /* If forbidCutting is true and the atom is bigger than the original denominator, break it */
                if (forbidCutting && atoms[i].denominator < fraction.denominator)
                {
                    atoms[i].numerator *= 2; /* CMB: Changed this to *2 instead of overriding it to 2 Ex. 1/2 -> 2/4 -> 2/8 */
                    atoms[i].denominator *= 2;

                    /* Don't increment i, so that we check this atom again in case it needs further breaking */
                }
                else
                {
                    /* If this piece can be broken, Randomly decide to break it down or not */
                    if (atoms[i].denominator <= 5 && Random.Range(0f, 1f) <= Constants.chanceToBreakPiece)
                    {
                        /* Take one portion out */
                        atoms[i].numerator--;

                        /* Add the removed portion as a new atom onto the end.
                         *  This does mess with the size of the array. I'm not sure if the for loop checks size each loop, or if this will break
                         *  TODO: This feels wrong...find a better way */
                        atoms.Add(new Fraction(2, atoms[i].denominator * 2));

                        /* If this atom reached 0, move along, otherwise, don't increment i, so we try to break again */
                        if (atoms[i].numerator == 0)
                            i++;

                    }
                    /* If on the highest difficulty, randomly decide to give an extra for this piece */
                    else if (
                        (Constants.difficulty == Constants.Difficulty.HARD || Constants.difficulty == Constants.Difficulty.DEIFENBACH)
                        && Random.Range(0f, 1f) <= Constants.chanceToGiveExtraPiece)
                    {
                        /* Decide what base to break the piece into */
                        int chosenDenominator = -1;
                        while (chosenDenominator == -1)
                        {
                            foreach (int factor in FractionTools.GetFactors(atoms[i].denominator))
                            {
                                if (Random.Range(0f, 1f) <= Constants.pieceDistribution[(PieceLength)factor])
                                {
                                    chosenDenominator = factor;
                                    break;
                                }
                            }
                        }
                        /* Create the new atom in the chosen base
                         * Add it to the list of extra pieces so that it doesn't loop on this piece 
                         */
                        extraPieces.Add(new Fraction(1, chosenDenominator));
                    }
                    else
                    {
                        i++; /* Move to the next atom */
                    }
                }
            }
        }
        /* At this point, atoms contains a list of (most likely) improper fractions (and 0/d fractions)
         *      Final step is to iterate this list, building the list of PieceLength pieces */
        for (int i = 0; i < atoms.Count; )
        {
            /* If this atom has been exhausted, move to the next atom */
            if (atoms[i].numerator == 0)
                i++;
            else
            {
                /* Add a new PieceLength, decrementing the numerator of the atom */
                finalSelection.Add((PieceLength)atoms[i].denominator);
                atoms[i].numerator--;
                /* Don't increment i, as this atom may still have a numerator greater than 0 */
            }
        }

        /* Also iterate the list of extra pieces, it is has any */
        for (int i = 0; i < extraPieces.Count; )
        {
            /* If this atom has been exhausted, move to the next atom */
            if (extraPieces[i].numerator == 0)
                i++;
            else
            {
                /* Add a new PieceLength, decrementing the numerator of the atom */
                finalSelection.Add((PieceLength)extraPieces[i].denominator);
                extraPieces[i].numerator--;
                /* Don't increment i, as this atom may still have a numerator greater than 0 */
            }
        }

        return finalSelection.ToArray();
    }

    private static List<Fraction> ConvertFractionToAtoms(Fraction fraction)
    {
        List<Fraction> atoms;

        /* If given 1/1, it needs to be broken down (AtomizeFraction will just give it right back) */
        if (fraction == Fraction.One)
        {
            /* If the gap is always 1, choose a denominator and dish out pieces of it */
            if (Constants.gapAlwaysOne)
            {
                atoms = new List<Fraction>();

                /* Randomly choose a denominator */
                int denominator = -1;
                while (denominator == -1)
                {
                    foreach (PieceLength piece in Enum.GetValues(typeof(Constants.PieceLength)))
                    {
                        if (Random.Range(0f, 1f) <= Constants.pieceDistribution[(PieceLength)piece])
                        {
                            denominator = (int)piece;
                            break;
                        }
                    }
                }
                for (int i = 0; i < denominator; i++)
                    atoms.Add(new Fraction(1, denominator));
            }
            /* Otherwise break the 1/1 into a group of either 1/2's or 1/3'rds */
            else
            {
                /* Either break into halves or thirds */
                if (Random.Range(0f, 1f) <= .5f)
                    atoms = new List<Fraction>() { new Fraction(1, 2), new Fraction(1, 2) };
                else
                    atoms = new List<Fraction>() { new Fraction(1, 3), new Fraction(1, 3), new Fraction(1, 3) };
            }

        }
        /* The fraction is not 1/1, so we can Atomize it */
        else
        {
            /* Get the list of atomic pieces, ignoring base 1 */
            atoms = FractionTools.AtomizeFraction(fraction, false).ToList<Fraction>();
        }

        return atoms;
    }

    /// <summary>
    /// Generates a random fraction to use as a gap using restrictions provided by Constants.cs
    /// </summary>
    /// <returns></returns>
    public static FractionTools.Fraction ChooseGapSize()
    {
        /* If the gap must always be one, return 1/1 */
        if (Constants.gapAlwaysOne)
            return FractionTools.Fraction.One;
        /* If the fraction must always be proper (1/d) */
        else if (Constants.gapAlwaysAtomic)
            return new FractionTools.Fraction(1, Random.Range(2, 10));
        else
        {
            int x = Random.Range(1, 5);
            int y = Random.Range(2, 10);

            /* If the fraction must not be improper or mixed, ensure the numerator is less than the denominator */
            if (!Constants.gapAllowImproperFractions && !Constants.gapAllowMixedNumbers)
                return x < y ? new FractionTools.Fraction(x, y) : new FractionTools.Fraction(y, x);
            /* Otherwise leave the fraction be, improper or not */
            else
                return new FractionTools.Fraction(x, y);
        }
    }
}

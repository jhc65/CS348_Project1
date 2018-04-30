using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Fraction = FractionTools.Fraction; // Add shorthand way to use Fraction
using PieceLength = Constants.PieceLength; // Add shorthand way to use PieceLength

public static class FractionBuilder
{
    /// <summary>
    /// Generates a list of pieceLengths that should be given to the player as extra pieces
    /// </summary>
    /// <param name="difficulty">Higher difficulty will return more pieces
    ///     as defined in <see cref="Constants.difficultyProbabilityDistribution"/></param>
    /// <returns>List of <see cref="Constants.PieceLength"/> to generate</returns>
    /// <remarks>
    ///     TODO: For higher difficulty, invert the probability distribution to make hard pieces more common
    ///     TODO: Involve the gap length in this decision
    /// </remarks>
    public static PieceLength[] GenerateExtraPieces(int difficulty)
    {
        List<PieceLength> pieces = new List<Constants.PieceLength>();
        
        /* Determine the number of pieces to generate */
        int piecesToGenerate = (int) ((float)difficulty * Constants.extraPiecesPerDifficultyLevel);

        for(int i = 0; i < piecesToGenerate; i++)
        {
            /* Use probability to choose a piece to generate */
            bool decided = false;
            while (!decided) /* While loop in case every probability check fails in the loop */
            {
                /* For each piece length, generate a random number and decide if this is the piece to generate */
                foreach(PieceLength pl in Constants.difficultyProbabilityDistribution.Keys)
                {
                    /* Make sure a piece hasn't already been chosen, then perform the coin toss */
                    if (!decided && Random.Range(0,1) <= Constants.difficultyProbabilityDistribution[pl])
                    {
                        decided = true;
                        pieces.Add(pl);
                    }
                }
            }
        }

        return pieces.ToArray();
    }

    /// <summary>
    /// Breaks a given fraction into multiple pieces to populate the players inventory
    /// </summary>
    /// <param name="fraction">The fraction to break into pieces</param>
    /// <param name="difficulty">Not used for anything yet...</param>
    /// <param name="forbidCutting">If true, all pieces will be less than or equal to the starting fraction</param>
    /// <returns></returns>
    /// <remarks>TODO: Difficulty is not being used yet. Find an appropriate place to include it</remarks>
    public static PieceLength[] BreakMyLifeIntoPieces(Fraction fraction, int difficulty, bool forbidCutting = false)
    {
        List<PieceLength> finalSelection = new List<PieceLength>();

        List<Fraction> atoms;
        if (fraction == Fraction.One)
        {
            /* Either break into halves or thirds */
            if (Random.Range(0f, 1f) <= .5f)
                atoms = new List<Fraction>() { new Fraction(2, 2) };
            else
                atoms = new List<Fraction>() { new Fraction(3, 3) };
        }
        else
        {
            /* Get the list of atomic pieces, ignoring base 1 */
            atoms = FractionTools.AtomizeFraction(fraction, false).ToList<Fraction>();
        }

        /* for each atom, decide whether to break it down or not based on difficulty */
        for (int i = 0; i < atoms.Count; )
        {
            //Debug.Log("Atoms: " + atoms.ToString());
            //foreach (Fraction df in atoms)
            //    Debug.Log("    " + df.ToString());
            /* If forbidCutting is true and the atom is bigger than the original denominator, break it */
            if (forbidCutting && atoms[i].denominator < fraction.denominator)
            {
                atoms[i].numerator *= 2; /* CMB: Changed this to *2 instead of overriding it to 2 Ex. 1/2 -> 2/4 -> 2/8 */
                atoms[i].denominator *= 2;

                /* Don't increment i, so that we check this atom again in case it needs further breaking */
            }
            else 
            {
                /* If this piece can be broken, flip a coin */
                if (atoms[i].denominator <= 5) /* TODO: Don't hardcode this */
                {
                    if (Random.Range(0f,1f) <= Constants.difficultyProbabilityDistribution[(PieceLength)atoms[i].denominator])
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
                    else
                    {
                        i++; /* Move to the next atom */
                    }
                }
                else
                {
                    i++; /* Move to the next atom */
                }
            }
        }
        /* At this point, atoms contains a list of (most likely) improper fractions (and 0/d fractions)
         *      Final step is to iterate this list, building the list of PieceLength pieces */
        for (int i = 0; i < atoms.Count;)
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

            return finalSelection.ToArray();
    }
}

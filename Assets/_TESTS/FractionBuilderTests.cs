using UnityEngine;
using NUnit.Framework;
using Fraction = FractionTools.Fraction; // Add shorthand way to use Fraction
using PieceLength = Constants.PieceLength; // Add shorthand way to use PieceLength

public class FractionBuilderTests
{

    [Test]
    public void BreakMyLifeIntoPiecesSingleRun()
    {
        Fraction original = new Fraction(Random.Range(1, 5), Random.Range(2, 10));
        PieceLength[] results = FractionBuilder.BreakMyLifeIntoPieces(original, 1);
        Fraction sum = Fraction.Zero;
        foreach (PieceLength pl in results)
        {
            sum += new Fraction(1, (int)pl);
        }
        Assert.IsTrue(original == sum, "BreakMyLifeIntoPieces re-added to " + sum.ToString() + " which did not add up to the original " + original.ToString());
    }

    [Test]
    public void BreakMyLifeIntoPiecesOne()
    {
        Fraction original = new Fraction(1,1);
        PieceLength[] results = FractionBuilder.BreakMyLifeIntoPieces(original, 0);
        Fraction sum = Fraction.Zero;
        foreach (PieceLength pl in results)
        {
            sum += new Fraction(1, (int)pl);
        }
        Assert.IsTrue(original == sum, "BreakMyLifeIntoPieces re-added to " + sum.ToString() + " which did not add up to the original " + original.ToString());
    }

    [Test]
    public void BreakMyLifeIntoPiecesOneForbidCutting()
    {
        Fraction original = new Fraction(1, 1);
        PieceLength[] results = FractionBuilder.BreakMyLifeIntoPieces(original, 1);
        Fraction sum = Fraction.Zero;
        foreach (PieceLength pl in results)
        {
            sum += new Fraction(1, (int)pl);
        }
        Assert.IsTrue(original == sum, "BreakMyLifeIntoPieces re-added to " + sum.ToString() + " which did not add up to the original " + original.ToString());
    }

    [Test]
    public void BreakMyLifeIntoPiecesMultipleRuns()
    {
        int toGenerate = 100;
        Fraction masterTotal = Fraction.Zero;
        Fraction runningTotal = Fraction.Zero;

        Fraction[] originals = new Fraction[toGenerate];
        for (int i = 0; i < toGenerate; i++)
        {
            originals[i] = new Fraction(Random.Range(1, 5), Random.Range(2, 10));
            masterTotal += originals[i];
            PieceLength[] results = FractionBuilder.BreakMyLifeIntoPieces(originals[i], 1);
            Fraction sum = Fraction.Zero;
            foreach (PieceLength pl in results)
            {
                sum += new Fraction(1, (int)pl);
            }
            Assert.IsTrue(originals[i] == sum, "BreakMyLifeIntoPieces re-added to " + sum.ToString() + " which did not add up to the original " + originals[i].ToString());
            runningTotal += sum;
        }
    }
}

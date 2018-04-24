using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fraction = FractionTools.Fraction; //Add shorthand way to use Fraction 
using PieceLength = Constants.PieceLength; // Add shorthand way to use PieceLength

namespace MathUnitTests.Math
{
    [TestClass]
    public class FractionGapTests
    {
        /* This test method breaks, as FractionBuilder references a Unity library, which prevents this test from building as a standalone */
        //[TestMethod]
        //public void BreakFractionTest()
        //{
        //    Fraction original = new Fraction(16, 12);
        //    PieceLength[] results = FractionBuilder.BreakMyLifeIntoPieces(original, 1);
        //    Console.Out.Write(results.ToString());
        //}
    }
}

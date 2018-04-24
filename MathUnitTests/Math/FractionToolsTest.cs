using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUnitTests
{
    [TestClass]
    public class FractionToolsTest
    {
        [TestMethod]
        public void AtomizeFractionTest()
        {
            FractionTools.Fraction testFraction = new FractionTools.Fraction(16, 12);
            FractionTools.Fraction[] atoms = FractionTools.AtomizeFraction(testFraction);
            Assert.IsNotNull(atoms, "AtomizeFraction did not return anything");
            Assert.IsTrue(testFraction.numerator == 16 && testFraction.denominator == 12, "AtomizeFraction mutated its input");
            Assert.IsTrue(atoms.Length == 2, "AtomizeFraction did not break into largest chunks");
            Assert.IsTrue(atoms[0].numerator == 1 && atoms[0].denominator == 1, "AtomizeFraction didn't return 1/1 as expected");
            Assert.IsTrue(atoms[1].numerator == 1 && atoms[1].denominator == 3, "AtomizeFraction didn't return 1/3 as expected");
        }
    }
}

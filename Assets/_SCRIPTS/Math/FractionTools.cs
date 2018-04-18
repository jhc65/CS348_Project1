﻿
using System.Collections.Generic;
public static class FractionTools {

    public struct Fraction
    {
        public int numerator;
        public int denominator;

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="f">Instance to copy</param>
        public Fraction(Fraction f)
        {
            numerator = f.numerator;
            denominator = f.denominator;
        }

        public Fraction(int n, int d)
        {
            numerator = n;
            denominator = d;
        }

        public static Fraction operator+ (Fraction a, Fraction b)
        {
            Fraction result = new Fraction();

            /* Get a common denominator. Doesn't have to be least, as we'll simplify at the end */
            result.denominator = a.denominator * b.denominator;

            /* Sum the adjusted numerators */
            result.numerator = a.numerator * b.denominator + b.numerator * a.denominator;

            result.Simplify();
            return result;
        }

        public static Fraction operator- (Fraction a, Fraction b)
        {
            Fraction result = new Fraction();

            /* Get a common denominator. Doesn't have to be least, as we'll simplify at the end */
            result.denominator = a.denominator * b.denominator;

            /* Subtract the adjusted numerators */
            result.numerator = a.numerator * b.denominator - b.numerator * a.denominator;

            result.Simplify();
            return result;
        }

        public static Fraction operator* (Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
            
            result.Simplify();
            return result;
        }

        public static Fraction operator/ (Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);

            result.Simplify();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Fraction))
                return false;

            return (this == (Fraction)obj);
        }

        /// <summary>
        /// Blatently stolen from https://stackoverflow.com/a/7813738 as I don't understand how this should work
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + numerator.GetHashCode();
                hash = hash * 23 + denominator.GetHashCode();
                return hash;
            }
        }

        public static bool operator== (Fraction a, Fraction b)
        {
            return (a.numerator == b.numerator && a.denominator == b.denominator);
        }

        public static bool operator!= (Fraction a, Fraction b)
        {
            return !(a == b);
        }

        public static bool operator< (Fraction a, Fraction b)
        {
            return (a.numerator * b.denominator < b.numerator * a.denominator);
        }

        public static bool operator> (Fraction a, Fraction b)
        {
            return (a.numerator * b.denominator > b.numerator * a.denominator);
        }

        /// <summary>
        /// Simplify a fraction, leaving it in improper form
        /// </summary>
        public void Simplify()
        {
            /* While a greatest common factor exists (that is not 1), Reduce */
            int gcf;
            while((gcf = GCF(numerator, denominator)) > 1)
            {
                /* Reduce both numbers by the common factor */
                numerator = numerator / gcf;
                denominator = denominator / gcf;
            }
        }

        /// <summary>
        /// Simplfy & Reduce an improper fraction, returning the whole number portion
        /// </summary>
        /// <returns>The whole number remaining, if any</returns>
        public int Reduce()
        {
            /* Remove the whole number portion */
            int excess = numerator / denominator;
            numerator = numerator % denominator;

            Simplify();

            return excess;
        }

        public override string ToString(){
            return numerator + "/" + denominator;
        }
    }

    public struct MixedNumber
    {
        int wholeNumber;
        Fraction fraction;

        /// <summary>
        /// Copy constuctor
        /// </summary>
        /// <param name="m">Instance to copy</param>
        public MixedNumber(MixedNumber m)
        {
            wholeNumber = m.wholeNumber;
            fraction = new Fraction(m.fraction);
        }

        /// <summary>
        /// Create a mixed number from a (potentially)improper fraction
        /// </summary>
        /// <param name="f">A (potentially) improper fraction</param>
        public MixedNumber(Fraction f)
        {
            fraction = new Fraction(f);
            wholeNumber = fraction.Reduce();
        }

        public MixedNumber(int w, Fraction f)
        {
            wholeNumber = w;
            fraction = f;
        }

        public MixedNumber(int w, int n, int d)
        {
            wholeNumber = w;
            fraction = new Fraction(n, d);
        }

        /// <summary>
        /// Converts a mixed number to an improper fraction
        /// </summary>
        /// <returns>(potentially)Improper fraction equivalent </returns>
        public Fraction ToImproperFraction()
        {
            return new Fraction(wholeNumber * fraction.denominator + fraction.numerator, fraction.denominator);
        }

        public static MixedNumber operator+ (MixedNumber a, MixedNumber b)
        {

            Fraction fractional = a.fraction + b.fraction;
            int wholeNumber = a.wholeNumber + b.wholeNumber;
            wholeNumber += fractional.Reduce();

            return new MixedNumber(wholeNumber, fractional);
        }

        public static MixedNumber operator- (MixedNumber a, MixedNumber b)
        {
            /* Convert both to improper fractions before subtraction */
            Fraction result = a.ToImproperFraction() - b.ToImproperFraction();
            
            return new MixedNumber(result);
        }

        public static MixedNumber operator* (MixedNumber a, MixedNumber b)
        {
            Fraction result = a.ToImproperFraction() * b.ToImproperFraction();

            return new MixedNumber(result);
        }

        public static MixedNumber operator/ (MixedNumber a, MixedNumber b)
        {
            Fraction result = a.ToImproperFraction() / b.ToImproperFraction();

            return new MixedNumber(result);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MixedNumber))
                return false;

            return (this == (MixedNumber)obj);
        }

        /// <summary>
        /// Blatently stolen from https://stackoverflow.com/a/7813738 as I don't understand how this should work
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + wholeNumber.GetHashCode();
                hash = hash * 23 + fraction.GetHashCode();
                return hash;
            }
        }

        public static bool operator== (MixedNumber a, MixedNumber b)
        {
            return (a.ToImproperFraction() == b.ToImproperFraction());
        }

        public static bool operator!= (MixedNumber a, MixedNumber b)
        {
            return !(a == b);
        }

        public static bool operator <(MixedNumber a, MixedNumber b)
        {
            return (a.ToImproperFraction() < b.ToImproperFraction());
        }

        public static bool operator >(MixedNumber a, MixedNumber b)
        {
            return (a.ToImproperFraction() > b.ToImproperFraction());
        }

        public override string ToString(){
            return wholeNumber + " & " + fraction;
        }
    }

    /// <summary>
    /// Greatest Common Factor
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The greatest common factor between a and b, or -1 if none is found</returns>
    public static int GCF (int a, int b)
    {
        /* Get a list of all factors for both numbers */
        int[] aFactors = GetFactors(a);
        int aIndex = aFactors.Length - 1;
        int[] bFactors = GetFactors(b);
        int bIndex = bFactors.Length - 1;

        /* Move backwards through the lists of factors until one is found, or either list is exhausted */
        while(aIndex >= 0 && bIndex >= 0)
        {
            /* If two factors match, return */
            if (aFactors[aIndex] == bFactors[bIndex])
                return aFactors[aIndex];
            /* Otherwise Reduce the index on the list with the larger number */
            else
            {
                if (aFactors[aIndex] > bFactors[bIndex])
                    aIndex--; /* a had the larger factor */
                else
                    bIndex--; /* b had the larger factor, or they were equal */
            }
        }

        /* If no common factor was found return -1 */
        return -1;
    }

    /// <summary>
    /// Gets all factors of a number *including 1 and itself*
    /// Does handle negative numbers
    /// </summary>
    /// <returns>Sorted array of int's containing all factors of "n" including 1 and "n"</returns>
    public static int[] GetFactors(int n)
    {
        /* if 0, return 0 and 1 */
        if (n == 0)
            return new int[] {0, 1};
        List<int> factors = new List<int>();
        int pfact = n;

        /* If factorizing a negative number, pretend it is positive and do additional work at the end */
        bool negative = false;
        if (pfact < 0)
        {
            negative = true;
            pfact *= -1;
        }

        /* Work backwards from n -> 0, checking divisibility */
        while (pfact > 0)
        {
            /* If there is no remainder, then it is a factor */
            if (pfact % n == 0)
                factors.Add(pfact);
            pfact--;
        }

        /* Handle additional processing if negative */
        if (negative)
        {
            /* For each factor, add it's negative */
            for (int i = factors.Count - 1; i >= 0; i--)
            {
                factors.Add(factors[i] * -1);
            }
        }

        /* Sort and return the list */
        factors.Sort();
        return factors.ToArray();
    }

}

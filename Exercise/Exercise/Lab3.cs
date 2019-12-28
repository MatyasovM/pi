using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Exercise1
{
    class Lab3
    {
        private static List <double> myKeys = new List <double>();
        private static List <BitVector32> myValues = new List <BitVector32>();
        
        public static void Do()
        {
            int L = 5;
            var aMaxValue = Math.Pow (2, L) - 1;
            myValues = General.GenerateSearceSpace (aMaxValue);
            //#3
            //variant 2
            Console.WriteLine ("Computing of landscape...");
            foreach (var aVal in myValues) {
                var aU = Math.Pow (aVal.Data - Math.Pow (2, L - 1), 2);
                myKeys.Add (aU);
            }

            //#4
            Console.WriteLine ("Applying of algorithm...");
            Algorithm();
        }

        static void Algorithm()
        {           
            var maxS = myValues[4]; 
            double max = myKeys[4];

            int N = 10;
            for (int i = 0; i < N; i++) {
                Console.WriteLine ($"Step # {i + 1}, current max = {max}, current maxS = {General.BitSetAsString (maxS)}");
                var aNeighborhood = FindNeighborhood (max);
                PrintNeighborhood (aNeighborhood);
                if (aNeighborhood.Count != 0) {
                    var aMax = aNeighborhood.Max();
                    Console.WriteLine ($"The best code in neighborhood: {aMax}");
                    if (aMax > max) {
                        maxS = myValues [myKeys.IndexOf (aMax)];
                        max = aMax;
                        Console.WriteLine ($"New maximum: {max}");
                    } else {
                        break;
                    }
                }

                Console.Write (Environment.NewLine);
            }

            Console.WriteLine ($"max : {max}");
            Console.WriteLine ($"maxS : {General.BitSetAsString (maxS)}");
        }

        static List <double> FindNeighborhood (double theKey)
        {
            int aCodeIndex = myKeys.IndexOf (theKey);
            var aCode = myValues[aCodeIndex];
            string aStringCode = General.BitSetAsString (aCode);

            List <double> aRes = new List <double>();
            for (int i = 0; i < aStringCode.Length; i++) {
                aStringCode = InvertBit (aStringCode, i);

                int anIntegerValue = Convert.ToInt32 (aStringCode, 2);
                BitVector32 aCurBitVector = new BitVector32 (anIntegerValue);

                int anIndex = myValues.IndexOf (aCurBitVector);
                if (anIndex >= 0) {
                    aRes.Add (myKeys[anIndex]);
                }
                aStringCode = InvertBit (aStringCode, i);
            }

            return aRes;
        }

        static string ToString (List <bool> theSource)
        {
            int[] anIntegers = new int[theSource.Count];
            for (int i = 0; i < anIntegers.Length; i++) {
                anIntegers[i] = Convert.ToInt32 (theSource[i]);
            }

            return string.Join ("", anIntegers);
        }

        static string InvertBit (string theBitSet, int theBitIndex)
        {
            var anIntVal = Convert.ToInt32 (theBitSet[theBitIndex] - '0');
            bool aBitValue = Convert.ToBoolean (anIntVal);
            bool anInvertBit = !aBitValue;
            int anInvertedInteger = Convert.ToInt32 (anInvertBit);
            
            string aStart = theBitSet.Substring (0, theBitIndex);

            int aLenght = theBitSet.Length - theBitIndex - 1;
            string anEnd = theBitSet.Substring (theBitIndex + 1, aLenght);
            return aStart + anInvertedInteger + anEnd;
        }

        static void PrintNeighborhood (List <double> theSource)
        {
            Console.WriteLine ("Current Neighborhood:");
            foreach (var aVal in theSource) {
                int aKeyIndex = myKeys.IndexOf (aVal);
                var aCurrentCode = myValues [aKeyIndex];
                Console.WriteLine ("\t" + General.BitSetAsString (aCurrentCode));
            }
        }
    }
}

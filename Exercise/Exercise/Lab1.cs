using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Exercise1
{
    class Lab1
    {
        public static void Do()
        {
            int L = 15;
            var aMaxValue = Math.Pow (2, L) - 1;
            var aSearchSpace = General.GenerateSearceSpace (aMaxValue);
            //#3
            //variant 2
            Console.WriteLine ("Computing of landscape...");
            Hashtable aLandscape = new Hashtable();
            foreach (var aVal in aSearchSpace) {
                aLandscape.Add (aVal.Data, aVal);
            }

            //#4
            Console.WriteLine ("Applying of algorithm...");
            Algorithm (aLandscape);
        }

        static void Algorithm (Hashtable theLandscape)
        {
            var aReverseList = General.GetReversedHashtableKeys (theLandscape);
            
            int max = 0;
            BitVector32 maxS = new BitVector32 (0);
            foreach (DictionaryEntry anEntry in aReverseList) {

                int aCurrentKey = (int)anEntry.Key;
                if (max < aCurrentKey) {
                    max = aCurrentKey;
                    maxS = (BitVector32) theLandscape [aCurrentKey];
                    Console.WriteLine ($"New max value is {max}");
                }
            }


            Console.WriteLine ($"max : {max}");

            
            Console.Write ("maxS : ");
            for (int i = 0; i < 32; i++) {
                Console.Write (Convert.ToInt32 (maxS[i]));
            }
            Console.WriteLine ("");
        }
    }
}

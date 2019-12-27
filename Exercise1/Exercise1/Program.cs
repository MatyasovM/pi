using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Exercise1
{
    class Program
    {
        static void Main(string[] args)
        {
            int L = 15;
            List <BitVector32> aSearchSpace = new List <BitVector32>();

            //#2
            Console.WriteLine ("Computing of search space...");
            var aMaxValue = Math.Pow (2, L) - 1;
            for (int i = 0; i < aMaxValue; i++) {
                aSearchSpace.Add (new BitVector32 (i));
            }

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
            object[] aReverseArray = new object[theLandscape.Count];
            theLandscape.CopyTo (aReverseArray, 0);
            List <object> aReverseList = new List<object> (aReverseArray);
            aReverseList.Reverse();
            
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

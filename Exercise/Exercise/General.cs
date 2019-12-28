using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1
{
    class General
    {
        public static List <BitVector32> GenerateSearceSpace (double theMaxValue)
        {
            List <BitVector32> aSearchSpace = new List <BitVector32>();

            Console.WriteLine ("Computing of search space...");
            for (int i = 0; i < theMaxValue; i++) {
                aSearchSpace.Add (new BitVector32 (i));
            }

            return aSearchSpace;
        }

        public static List <object> GetReversedHashtableKeys (Hashtable theTable)
        {
            object[] aReverseArray = new object[theTable.Count];
            theTable.CopyTo (aReverseArray, 0);
            List <object> aReverseList = new List<object> (aReverseArray);
            aReverseList.Reverse();

            return aReverseList;
        }

        public static string BitSetAsString (BitVector32 theVec)
        {
            var aString = theVec.ToString();
            string aStartTemplate = "{BitVector32{";
            string anEndTemplate = "}}";
            return aString.Substring (aStartTemplate.Length - 1, aString.Length - (aStartTemplate.Length + anEndTemplate.Length) + 2);
        }
    }
}

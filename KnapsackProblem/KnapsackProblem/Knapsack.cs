using System.Collections.Generic;
using System.IO;

namespace KnapsackProblem
{
    public class KnapsackObject
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
    }
    
    public class Knapsack
    {
        List <KnapsackObject> myKnapsack = new List<KnapsackObject>();

        public List <KnapsackObject> Knapsck { get => myKnapsack; set => myKnapsack = value; }

        public bool Read (string theFileName)
        {
            var aFileStream = File.OpenRead (theFileName);
            if (aFileStream == null) {
                return false;
            }
            StreamReader aReader = new StreamReader (aFileStream);

            string aLine = "";
            while ((aLine = aReader.ReadLine()) != null) {
                
                KnapsackObject anObj = new KnapsackObject();
                int i = 0;
                anObj.ID = GetNextValue (ref i, aLine);
                anObj.Price = GetNextValue (ref i, aLine);
                anObj.Weight = GetNextValue (ref i, aLine);
                
                myKnapsack.Add (anObj);
            }
            return true;
        }

        int GetNextValue (ref int theCurPos, string theCurrentLine)
        {
            string aCurrentValue = "";
            for (; theCurPos < theCurrentLine.Length; theCurPos++) {
                if (theCurrentLine[theCurPos] == ' ') {
                    theCurPos++;
                    break;
                }
                aCurrentValue += theCurrentLine[theCurPos];
            }
            return System.Convert.ToInt32 (aCurrentValue);
        }
    }
}

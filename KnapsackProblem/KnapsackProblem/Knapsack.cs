using System.Collections.Generic;
using System.IO;

namespace KnapsackProblem
{
    public class KnapsackObject
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
        public int Check { get; set; }
    }
    
    public class Knapsack
    {
        List <KnapsackObject> myKnapsack = new List<KnapsackObject>();
        public int WeightLimit { get; set; }

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

        public List <int> ResolveProblem()
        {
            List <int> S = new List <int>();
            for (int i = 0; i < myKnapsack.Count; i++) {
                S.Add (0);
            }
            int Q = 0;
            int SumW = 0;

            KnapsackObject[] aCopy = new KnapsackObject [myKnapsack.Count];
            myKnapsack.CopyTo (aCopy);
            List <KnapsackObject> aCopyList = new List <KnapsackObject>();
            aCopyList.AddRange (aCopy);

            for (int i = 0; i < myKnapsack.Count; i++) {

                var aCurrentMaxCi = GetMax_C (aCopyList);
                if ((SumW + aCurrentMaxCi.Weight) <= WeightLimit) {

                    int aCurrentIndex = myKnapsack.IndexOf (aCurrentMaxCi);
                    aCopyList.Remove (aCurrentMaxCi);

                    Q = Q + aCurrentMaxCi.Price;
                    SumW = SumW + aCurrentMaxCi.Weight;
                    S[aCurrentIndex] =  1;
                } else {
                    break;
                }
            }

            return S;
        }

        private KnapsackObject GetMax_C (List <KnapsackObject> theKnap)
        {
            int aMax = -1;
            KnapsackObject aRes = new KnapsackObject();
            foreach (var anObj in theKnap) {
                if (anObj.Price > aMax) {
                    aMax = anObj.Price;
                    aRes = anObj;
                }
            }

            return aRes;
        }
    }
}

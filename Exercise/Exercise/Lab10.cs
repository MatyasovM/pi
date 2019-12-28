using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercise1
{
    public class KnapsackObject
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
        public int Check { get; set; }
    }

    class Lab10
    {
        List <KnapsackObject> myKnapsack = new List<KnapsackObject>();

        public void Do()
        {
            Read ("Knapsack#1.txt");
            var aParent1 = GenerateParent (106);

            Read ("Knapsack#2.txt");
            var aParent2 = GenerateParent (88);

            int aNumberOfChilds = 10;
            var aStartPopulation = MakePopulation (aParent1, aParent2, aNumberOfChilds);
            var aMutatedPopulation = ApplyMutation (aStartPopulation);
            aNumberOfChilds = 0;
        }

        public bool Read (string theFileName)
        {
            myKnapsack.Clear();
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

        public List <int> GenerateParent (int theWeightLimit)
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
                if ((SumW + aCurrentMaxCi.Weight) <= theWeightLimit) {

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

        private List <List <int>> MakePopulation (List <int> theParent1, List <int> theParent2, int theNumberChilds)
        {
            List <List <int>> aRes = new List <List <int>>();
            List <int> aPreviouseChild = new List <int> (0);
            List <int> aCurrentChild = new List<int> (1);
            for (int i = 0; i < theNumberChilds; i++) {
                while (Enumerable.SequenceEqual (aPreviouseChild.OrderBy (t => t), aCurrentChild.OrderBy(t => t))) {
                    aCurrentChild = MakeChild (theParent1, theParent2);
                }
                aPreviouseChild = aCurrentChild;
                aRes.Add (aCurrentChild);   
            }

            return aRes;
        }

        private List <int> MakeChild (List <int> theParent1, List <int> theParent2)
        {
            List <int> aChild = new List <int>();
            Random aRand = new Random((int) DateTime.UtcNow.Ticks);
            int aFirstDelim = aRand.Next (0, theParent1.Count);
            int aSecondDelim = aRand.Next (0, theParent1.Count);
            if (aFirstDelim > aSecondDelim) {
                int aTmp = aFirstDelim;
                aFirstDelim = aSecondDelim;
                aSecondDelim = aTmp;
            }

            for (int i = 0; i < theParent1.Count; i++) {
                    
                if (i > aFirstDelim && i < aSecondDelim) {
                    aChild.Add (theParent1[i]);
                } else {
                    aChild.Add (theParent2[i]);
                }   
            }

            return aChild;
        }

        private List <List <int>> ApplyMutation (List <List <int>> theSourcePopulation)
        {
            List<List<int>> aMutatedPopulation = new List<List<int>>();
            foreach (var aChild in theSourcePopulation) {
                List <int> aMutatedChild = new List<int>();
                foreach (var aGen in aChild) {
                    Random aRand = new Random(); ;
                    if (aRand.Next (0, 1000) > 500) {
                        aMutatedChild.Add (Convert.ToInt32 (!Convert.ToBoolean (aGen)));
                    } else {
                        aMutatedChild.Add (aGen);
                    }
                }
                aMutatedPopulation.Add (aMutatedChild);
            }

            return aMutatedPopulation;
        }
    }
}

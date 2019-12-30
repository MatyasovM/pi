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
        int myLimitWeight = 106;

        public void Do()
        {
            myKnapsack.Sort ((theLeft, theRight) => {
                if (theLeft.Price > theRight.Price) {
                    return 1;
                } else if (theLeft.Price < theRight.Price) {
                    return -1;
                }
                return 0;
            });
            Read ("Knapsack#1.txt");
            int aCurrentMaxPrice = 0;
            var aParent1 = GenerateParent (myLimitWeight);
            var aParent2 = GenerateRandomParent (myLimitWeight);
            int aParent1Price = GetPrice (aParent1);
            int aParent2Price = GetPrice (aParent2);
            if (aParent1Price > aParent2Price) {
                aCurrentMaxPrice = aParent1Price;
            } else {
                aCurrentMaxPrice = aParent2Price;
            }

            int aNumberAttempts = 0;
            int aNumberOfGeneration = 0;
            while (true) {
                Console.WriteLine ($"A generation #{aNumberOfGeneration}");
                if (aNumberAttempts > 2) {
                    break;
                }
                
                var aNextPop = GetNextPopulation (aParent1, aParent2);
                var aNewMaxPrice = GetPrice (aNextPop[0]);
                if (aNewMaxPrice > aCurrentMaxPrice) {
                    aCurrentMaxPrice = aNewMaxPrice;
                    aParent1 = aNextPop[0];
                    aParent2 = aNextPop[1];
                    aNumberAttempts = 0;
                } else {
                    aNumberAttempts++;
                }
                aNumberOfGeneration++;
            }

            Console.WriteLine ($"Max price: {aCurrentMaxPrice}");
            Console.Write ("Knapsack: ");
            PrintApplicant (aParent1);

        }

        void PrintApplicant (List <int> thePplicant)
        {
            foreach (var anItem in thePplicant) {
                Console.Write (anItem + " ");
            }
            Console.WriteLine ("");
        }

        private List <List <int>> GetNextPopulation (List <int> theParent1, List <int> theParent2)
        {
            Console.WriteLine ("Please enter number of childs in new population");
            int aNumberOfChilds = Convert.ToInt32 (Console.ReadLine());
            var aStartPopulation = MakePopulation (theParent1, theParent2, aNumberOfChilds);
            var aMutatedPopulation = ApplyMutation (aStartPopulation);
            Console.WriteLine ("New population: \n");
            foreach (var anApplic in aMutatedPopulation) {
                PrintApplicant (anApplic);
            }
            var aBestChilds = ChooseTheBest (aMutatedPopulation, myLimitWeight);
            
            Console.Write ("The best child: ");
            if (aBestChilds.Count != 0) {
                PrintApplicant (aBestChilds[0]);   
            } else {
                Console.Write ("Non\n");
            }

            List <List <int>> aNextApplicants = new List <List <int>>();
            aNextApplicants.Add (theParent1);
            aNextApplicants.Add (theParent2);
            aNextApplicants.AddRange (aBestChilds);
            return ChooseTheBest (aNextApplicants, myLimitWeight);            
        }

        private List <List <int>> ChooseTheBest (List <List <int>> theApplicants, int theLimitWeight)
        {
            theApplicants.Sort ((theLeft, theRight) => {
                int aLeftPrice = GetPrice (theLeft);
                int aRightPrice = GetPrice (theRight);
                if (aLeftPrice > aRightPrice) {
                    return 1;
                } else if (aLeftPrice < aRightPrice) {
                    return -1;
                }

                return 0;
            });

            List <List <int>> aRes = new List <List <int>>();
            foreach (var aChild in theApplicants) {
                if (GetWeigth (aChild) <= theLimitWeight) {
                    aRes.Add (aChild);
                }
            }
            if (aRes.Count > 1) {
                return aRes.GetRange (0, 2);   
            }
            return aRes;
        }

        private int GetPrice (List <int> theKnap)
        {
            int aPrice = 0;
            for (int i = 0 ; i < theKnap.Count; i++) {
                if (theKnap[i] == 1) {
                    aPrice += myKnapsack[i].Price;
                }
            }

            return aPrice;
        }

        private int GetWeigth (List <int> theKnap)
        {
            int aWeight = 0;
            for (int i = 0 ; i < theKnap.Count; i++) {
                if (theKnap[i] == 1) {
                    aWeight += myKnapsack[i].Weight;
                }
            }

            return aWeight;
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

        public List <int> GenerateRandomParent (int theWeightLimit)
        {
            List <int> aRes = Enumerable.Repeat (0, myKnapsack.Count).ToList();
            int aCurrentWeight = 0;
            for (int i = 0 ; i < myKnapsack.Count; i++) {
                Random aRand = new Random ((int) DateTime.UtcNow.Ticks);
                int aNext = aRand.Next (0, 2);
                if (aNext == 1) {
                    if (aCurrentWeight + myKnapsack[i].Weight > theWeightLimit) {
                        break;
                    }
                    aCurrentWeight += myKnapsack[i].Weight;
                }
                aRes[i] = aNext;
            }

            return aRes;
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

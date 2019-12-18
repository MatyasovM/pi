using System;
using System.Collections.Generic;

namespace CoreFinder
{
    public struct Initialized { }
    public class Matrix
    {
        MathNet.Numerics.LinearAlgebra.Matrix <Single> M;
        MathNet.Numerics.LinearAlgebra.Matrix <Single> mySourceM;
        List <Single> myVertices;
        Stack <int> myPath;
        List <List <int>> myLoops;

        public MathNet.Numerics.LinearAlgebra.Matrix <Single> Mat { get => mySourceM; }

        public Matrix (Initialized theInit)
        {
            M = MathNet.Numerics.LinearAlgebra.Matrix<Single>.Build.Dense (10, 10);
            M[0, 0] = 0; M[0, 1] = 1; M[0, 2] = 0; M[0, 3] = 0; M[0, 4] = 0; M[0, 5] = 1; M[0, 6] = 0; M[0, 7] = 0; M[0, 8] = 0; M[0, 9] = 0;
            M[1, 0] = 0; M[1, 1] = 0; M[1, 2] = 1; M[1, 3] = 1; M[1, 4] = 0; M[1, 5] = 0; M[1, 6] = 0; M[1, 7] = 0; M[1, 8] = 0; M[1, 9] = 0;
            M[2, 0] = 1; M[2, 1] = 1; M[2, 2] = 0; M[2, 3] = 1; M[2, 4] = 0; M[2, 5] = 1; M[2, 6] = 0; M[2, 7] = 0; M[2, 8] = 0; M[2, 9] = 0;
            M[3, 0] = 0; M[3, 1] = 0; M[3, 2] = 0; M[3, 3] = 0; M[3, 4] = 1; M[3, 5] = 1; M[3, 6] = 0; M[3, 7] = 0; M[3, 8] = 0; M[3, 9] = 0;
            M[4, 0] = 0; M[4, 1] = 0; M[4, 2] = 0; M[4, 3] = 0; M[4, 4] = 0; M[4, 5] = 0; M[4, 6] = 1; M[4, 7] = 0; M[4, 8] = 0; M[4, 9] = 1;
            M[5, 0] = 0; M[5, 1] = 0; M[5, 2] = 0; M[5, 3] = 1; M[5, 4] = 1; M[5, 5] = 0; M[5, 6] = 0; M[5, 7] = 0; M[5, 8] = 0; M[5, 9] = 0;
            M[6, 0] = 0; M[6, 1] = 0; M[6, 2] = 0; M[6, 3] = 0; M[6, 4] = 0; M[6, 5] = 0; M[6, 6] = 0; M[6, 7] = 1; M[6, 8] = 0; M[6, 9] = 0;
            M[7, 0] = 0; M[7, 1] = 0; M[7, 2] = 0; M[7, 3] = 0; M[7, 4] = 0; M[7, 5] = 0; M[7, 6] = 0; M[7, 7] = 0; M[7, 8] = 1; M[7, 9] = 0;
            M[8, 0] = 0; M[8, 1] = 0; M[8, 2] = 0; M[8, 3] = 0; M[8, 4] = 0; M[8, 5] = 0; M[8, 6] = 0; M[8, 7] = 0; M[8, 8] = 0; M[8, 9] = 0;
            M[9, 0] = 0; M[9, 1] = 0; M[9, 2] = 0; M[9, 3] = 0; M[9, 4] = 0; M[9, 5] = 0; M[9, 6] = 0; M[9, 7] = 0; M[9, 8] = 1; M[9, 9] = 0;
            mySourceM = GetCopy();
        }
        public Matrix() { }
        public Matrix (int theSize)
        {
            M = MathNet.Numerics.LinearAlgebra.Matrix <Single>.Build.Dense (theSize, theSize);
            Random aRand = new Random();
            for (int i = 0; i < theSize; i++) {
                for (int j = 0; j < theSize; j++) {
                    M [i, j] = aRand.Next (0, 2);
                }
            }
            mySourceM = GetCopy();
        }
        public void Read()
        {
            Console.WriteLine ("Please enter matrix values. For example: 1 0 0 1");
            string aLine = Console.ReadLine();
            string[] aStringMatrix = aLine.Split (' ');

            double aMatrixSize = Math.Sqrt (aStringMatrix.Length);
            int aMatrixDimension = (int) aMatrixSize;

            if (aMatrixDimension != aMatrixSize) {
                Console.WriteLine ("Matrix is not square");
                Environment.Exit (-1);
            }

            M = MathNet.Numerics.LinearAlgebra.Matrix <Single>.Build.Dense (aMatrixDimension, aMatrixDimension);
            for (int i = 0, j = 0; i < aStringMatrix.Length; i++) {
                M[j, i % aMatrixDimension] = Convert.ToInt32 (aStringMatrix[i]);
                if ((i + 1) % aMatrixDimension == 0) {
                    j++;
                }
            }
        }

        public void MakeReflexive()
        {
            for (int i = 0, j = 0; i < M.RowCount; i++, j++) {
                M[i, j] = 1;
            }
        }


        public MathNet.Numerics.LinearAlgebra.Matrix <Single> IsTransite()
        {
            var anOld = M;

            while (true) {
                var anOther = M.Multiply (M);
                bool anIsSame = anOther.Equals (M);

                if (!anIsSame) {
                    M = anOther;
                } else {
                    break;
                }
            }

            return anOld;
        }

        public HelperSet GetHelperSets (MathNet.Numerics.LinearAlgebra.Matrix <Single> theMatrix)
        {
            HelperSet aRes = new HelperSet();

            for (int i = 0; i < theMatrix.ColumnCount; i++) {
                var aColumn = theMatrix.Column (i);

                int aNumberNull = 0;
                foreach (var aVal in aColumn) {
                    if (aVal == 0) {
                        aNumberNull++;
                    }
                }


                Vector aVec = new Vector();
                aVec.Vec = aColumn;
                aVec.Index = i;

                if (aNumberNull == aColumn.Count) {
                    aRes.Null.Add (aVec);
                } else {
                    aRes.Ones.Add (aVec);
                }
            }

            return aRes;
        }

        public void Print (MathNet.Numerics.LinearAlgebra.Matrix<Single> theMat)
        {
            for (int i = 0; i < theMat.RowCount; i++) {
                for (int j = 0; j < theMat.ColumnCount ; j++) {
                    Console.Write (theMat[i, j]);
                }
                Console.WriteLine ("");
            }
            Console.WriteLine ("");
        }

        bool VisitVertex (int theIndex)
        {
            if (myPath.Contains (theIndex)) { 
                return true;
            }

            int aLastVertex = 0;
            if (myPath.Count != 0) { 
                aLastVertex = myPath.Peek();
            }

            myPath.Push (theIndex);
            var aRow = M.Row (theIndex);
            List <int> aOnes = new List <int>();

            for (int i = 0; i < aRow.Count; i++) {
                if (aRow[i] == 1 && i != theIndex) {
                    if (aLastVertex == i && myPath.Count == 2) {
                        return true;
                    } else {
                        aOnes.Add (i);
                    }
                }
            }

            foreach (var anIndex in aOnes) {
                bool aRes = VisitVertex (anIndex);
                if (aRes) {
                    return true;
                }
            }

            myPath.Pop();
            return false;
        }

        public void FineKernel()
        {
            //Read(); // uncomment to enter matrix manualy
            MakeReflexive();

            M = IsTransite();

            myVertices = new List <Single>();
            for (int i = 0; i < M.ColumnCount; i++) { 
                myVertices.Add (i);
            }
            myPath = new Stack <int>();
            myLoops = new List <List <int>>();
            var aVisitedVetices = new List <int>();


            for (int i = 0; i < M.ColumnCount; i++) {

                if (!aVisitedVetices.Contains (i)) {

                    bool aRes = VisitVertex (i);
                    if (aRes) {
                        myLoops.Add (new List <int >(myPath.ToArray()));
                        aVisitedVetices.AddRange (myPath.ToArray());
                        myPath.Clear();
                    } else {
                        aVisitedVetices.Add (i);
                        myLoops.Add (new List<int> { i });
                    }
                }
            }

            MathNet.Numerics.LinearAlgebra.Matrix <Single> aRelationMatrix = 
                MathNet.Numerics.LinearAlgebra.Matrix <Single>.Build.Dense (myLoops.Count, myLoops.Count);

            FillRelationMatrix (myLoops, aRelationMatrix);
            var aSets = GetHelperSets  (aRelationMatrix);

            
            var aKernels = TryFindKernels (aSets).Null;

            foreach (var aKernel in aKernels) {

                Console.Write ($"Kernel #{aKernel.Index}\n");
                foreach (var aRowIndex in myLoops[aKernel.Index]) {
                    
                    Console.Write ("\t");
                    foreach (var aVal in Mat.Row (aRowIndex)) { 
                        Console.Write ($"{aVal} ");
                    }

                    Console.Write (Environment.NewLine);
                }

                Console.Write (Environment.NewLine);
            }
            
        }

        private void FillRelationMatrix (List <List <int>> theSource, MathNet.Numerics.LinearAlgebra.Matrix <Single> theTarget)
        {
            for (int i = 0; i < theSource.Count; i++) {

                List <Single> anOtherVertices = new List <Single>();
                for (int j = 0; j < theSource[i].Count; j++) {

                    var aRow = M.Row (theSource[i][j]);
                    for (int k = 0; k < aRow.Count; k++) {

                        int anElem = (int) aRow[k];
                        if (anElem == 1 && !theSource[i].Contains (k) && !anOtherVertices.Contains (k)) {
                            anOtherVertices.Add (k);
                        }
                    }
                }

                // fill row
                for (int j = 0; j < theSource.Count; j++) {
                    if (i != j) { 

                        for (int k = 0; k < anOtherVertices.Count; k++) { 

                            if (theSource[j].Contains ((int) anOtherVertices[k])) { 
                                theTarget[i, j] = 1;
                            }
                        }
                    }
                }
            }
        }

        HelperSet TryFindKernels (HelperSet theSource)
        {
            HelperSet aResSet = theSource.GetCopy();

            for (int i = 0; i < theSource.Ones.Count; i++) { 

                var aColumn = theSource.Ones[i].Vec;
                for (int j = 0; j < aColumn.Count; j++) {
                    if (aColumn[j] == 1) { 

                        for (int k = 0; k < aResSet.Null.Count; k++) {

                            if (aResSet.Null[k].Index == j) {
                                
                                Vector aVec = new Vector();
                                aVec.Vec = aColumn;
                                aVec.Index = theSource.Ones[i].Index;
                                aResSet.Empty.Add (aVec);

                                DeleteVectorElementByIndex (aResSet.Ones, aVec.Index);
                            }
                        }

                        for (int k = 0; k < aResSet.Empty.Count; k++)
                        {

                            if (aResSet.Empty[k].Index == j)
                            {

                                Vector aVec = new Vector();
                                aVec.Vec = aColumn;
                                aVec.Index = theSource.Ones[i].Index;
                                aResSet.Null.Add (aVec);

                                DeleteVectorElementByIndex (aResSet.Ones, aVec.Index);
                            }
                        }
                    }
                }
            }

            return aResSet;
        }

        void DeleteVectorElementByIndex (List <Vector> theVec, int theIndex)
        {
            for (int i = 0; i < theVec.Count; i++) {
                
                if (theVec[i].Index == theIndex) {
                    theVec.RemoveAt (i);
                    return;
                }
            }
        }

        MathNet.Numerics.LinearAlgebra.Matrix <Single> GetCopy()
        {
            MathNet.Numerics.LinearAlgebra.Matrix<Single> aRes = MathNet.Numerics.LinearAlgebra.Matrix<Single>.Build.Dense (M.ColumnCount, M.ColumnCount);
            for (int i = 0; i < M.RowCount; i++) {
                for (int j = 0; j < M.ColumnCount; j++) {
                    aRes[i, j] = M[i, j];
                }
            }

            return aRes;
        }
    }
}

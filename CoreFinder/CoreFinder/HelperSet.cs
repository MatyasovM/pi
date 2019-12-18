using System;
using System.Collections.Generic;

namespace CoreFinder
{
    public class Vector
    {
        public MathNet.Numerics.LinearAlgebra.Vector<Single> Vec { get; set; }
        public int Index;
    }
    public class HelperSet
    {
        public List <Vector> Empty { get; set; }
        public List <Vector> Null { get; set; }
        public List <Vector> Ones { get; set; }

        public HelperSet()
        {
            Empty = new List <Vector>();
            Null = new List <Vector>();
            Ones = new List <Vector>();
        }

        public HelperSet GetCopy()
        {
            HelperSet aSet = new HelperSet();
            foreach (var aV in Empty) {
                aSet.Empty.Add (aV);
            }

            foreach (var aV in Null) {
                aSet.Null.Add (aV);
            }

            foreach (var aV in Ones) {
                aSet.Ones.Add (aV);
            }

            return aSet;
        }
    }
}

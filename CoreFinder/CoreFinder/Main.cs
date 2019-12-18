using MathNet.Numerics.LinearAlgebra;
using System;

namespace CoreFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix aMatrix = new Matrix (new Initialized());
            Console.WriteLine ("Source matrix... \n");
            aMatrix.Print (aMatrix.Mat);

            aMatrix.FineKernel();
        }
    }
}

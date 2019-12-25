using System;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Knapsack aKnap = new Knapsack();

            if (aKnap.Read (@"C:\Users\mmatyasov\Desktop\pi\KnapsackProblem\KnapsackProblem\Knapsack#1.txt")) {
                aKnap.WeightLimit = 106;
                var anAnswer = aKnap.ResolveProblem();

                Console.WriteLine ("");
                foreach (var aC in anAnswer) {
                    Console.Write (aC + " ");
                }   
            }

            Console.ReadKey();
            
        }
    }
}

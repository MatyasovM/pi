

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Knapsack aKnap = new Knapsack();
            aKnap.Read (@"C:\Users\mmatyasov\Desktop\pi\KnapsackProblem\KnapsackProblem\Knapsack#1.txt");

            aKnap.WeightLimit = 106;
            
        }
    }
}

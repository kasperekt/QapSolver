using System;
using ArrayExtensions;

namespace QapSolver.Solvers
{
    public class QapSimulatedAnnealing : QapProblemSolver
    {
        public QapSimulatedAnnealing(QapProblemInstance instance) : base(instance)
        {
        }

        public override string Name => "simulated-annealing";
        private const double Eps = 1e-3;
        private const double Alpha = 0.98;
        private const double InitialTemperature = 850.0;
        private readonly Random _random = new Random();
        
        protected override QapProblemSolution Solve()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            var bestAssignments = GetRandomAssignments(Instance.Size);
            var initialCost = GetCost(bestAssignments);
            var bestCost = initialCost;
            
            CalcDeltaTable(bestAssignments);
            
            var swaps = GetSwaps();

            var visited = 0;
            var steps = 0;
            var temperature = Instance.Size * InitialTemperature;
            
            while (temperature > Eps)
            {
                var (randomI, randomJ) = swaps[_random.Next(0, swaps.Count)];
                var delta = DeltaTable[randomI, randomJ];
                
                if (delta < 0)
                {
                    bestAssignments.Swap(randomI, randomJ);
                    bestCost += delta;
                    CalcDeltaTable(bestAssignments);
                    steps++;
                }
                else
                {
                    var prob = _random.NextDouble();
                    if (prob < Math.Exp(-delta / temperature))
                    {
                        bestAssignments.Swap(randomI, randomJ);
                        bestCost += delta;
                        CalcDeltaTable(bestAssignments);
                        steps++;
                    }
                }
                
                visited++;
                temperature *= Alpha;
            }
            
            watch.Stop();
            
            return new QapProblemSolution(
                bestAssignments,
                initialCost,
                bestCost,
                steps,
                visited,
                watch.ElapsedMilliseconds
            );
        }
    }
}
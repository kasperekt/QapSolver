using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArrayExtensions;

namespace QapSolver.Solvers
{
    public class QapTabooSearch : QapProblemSolver
    {
        private class TabooList
        {
            private List<(int, int)> Taboo { get; set; }

            public TabooList(int capacity)
            {
                Taboo = new List<(int, int)>(capacity);
            }

            public void Add((int, int) tuple)
            {
                Taboo.Insert(0, tuple);
            }

            public bool Has((int, int) tuple)
            {
                return Taboo.Contains(tuple);
            }
        }
        
        public QapTabooSearch(QapProblemInstance instance) : base(instance) { }

        public override string Name => "taboo";
        private const int AttemptsLimit = 5;

        protected override QapProblemSolution Solve()
        {
            var attempts = 0;
            
            var bestAssignments = GetRandomAssignments(Instance.Size);
            var bestCost = GetCost(bestAssignments);
            
            var tabooList = new TabooList(5);
            var bestCandidateAssignments = bestAssignments.Clone() as int[];
            var bestCandidateCost = bestCost;
            CalcDeltaTable(bestCandidateAssignments);   

            while (attempts < AttemptsLimit)
            {
                attempts += 1;
                var swaps = GetSwaps();
                (int, int)? bestSwap = null;
                
                foreach (var swap in swaps)
                {
                    var deltaSwapValue = DeltaTable[swap.Item1, swap.Item2];
                    
                    if (!tabooList.Has(swap) && deltaSwapValue < 0)
                    {
                        bestSwap = swap;
                        bestCandidateAssignments.Swap(swap.Item1, swap.Item2);
                        bestCandidateCost += deltaSwapValue;
                        CalcDeltaTable(bestCandidateAssignments);
                    }
                }

                if (!bestSwap.HasValue)
                {
                    continue;
                }
                
                tabooList.Add(bestSwap.Value);

                if (bestCost > bestCandidateCost && bestCandidateAssignments != null)
                {
                    bestAssignments = bestCandidateAssignments.Clone() as int[];
                    bestCost = bestCandidateCost;
                    attempts = 0;
                }
            }
            

            return new QapProblemSolution(bestAssignments, GetCost(bestAssignments));
        }
    }
}
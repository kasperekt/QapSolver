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
            
            var tabooList = new TabooList(1);
            var bestCandidateAssignments = bestAssignments.Clone() as int[];
            var bestCandidateCost = bestCost;
            CalcDeltaTable(bestCandidateAssignments);   

            while (attempts < AttemptsLimit)
            {
                attempts += 1;
                var swaps = GetSwaps();
                
                foreach (var (swapI, swapJ) in swaps)
                {
                    var deltaSwapValue = DeltaTable[swapI, swapJ];
                    if (!tabooList.Has((swapI, swapJ)) && deltaSwapValue < 0)
                    {
                        bestCandidateAssignments.Swap(swapI, swapJ);
                        bestCandidateCost += deltaSwapValue;
                        CalcDeltaTable(bestCandidateAssignments);
                    }
                    
                    tabooList.Add((swapI, swapJ));

                    if (bestCost > bestCandidateCost)
                    {
                        bestAssignments = bestCandidateAssignments.Clone() as int[];
                        bestCost = bestCandidateCost;
                        attempts = 0;
                    }
                }
            }
            

            return new QapProblemSolution(bestAssignments, GetCost(bestAssignments));
        }
    }
}
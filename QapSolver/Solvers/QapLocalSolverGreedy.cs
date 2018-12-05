using System;
using System.Collections.Generic;
using System.Diagnostics;
using QapSolver;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  class QapLocalSolverGreedy : QapProblemSolver
  {
    public override string Name
    {
      get { return "local-greedy-solver"; }
    }

    public QapLocalSolverGreedy(QapProblemInstance instance) : base(instance) { }

    protected override QapProblemSolution Solve()
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);
      var initialCost = cost;
      CalcDeltaTable(assignments);

      bool progress = true;
      int iterationCounter = 0;
      int visited = 0;

      while (progress)
      {
        progress = false;
        iterationCounter++;

        var swaps = GetSwaps();

        foreach (var (i, j) in swaps)
        {
          visited++;

          if (DeltaTable[i, j] < 0)
          {
            assignments.Swap(i, j);
            cost += DeltaTable[i, j];
            CalcDeltaTable(assignments);
            progress = true;
            break;
          }
        }
      }

      watch.Stop();

      return new QapProblemSolution(
        assignments,
        initialCost,
        cost,
        iterationCounter,
        visited,
        watch.ElapsedMilliseconds
      );
    }

    private List<int[]> GetNeighbours(int[] assignments)
    {
      var neighbours = new List<int[]>();

      for (var i = 0; i < assignments.Length; i++)
      {
        for (var j = i + 1; j < assignments.Length; j++)
        {
          var neighbour = (assignments.Clone() as int[]);
          neighbour.Swap(i, j);

          neighbours.Add(neighbour);
        }
      }

      return neighbours;
    }
  }
}
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  class QapHeuristicSolver : QapProblemSolver
  {
    public override string Name
    {
      get { return "heuristic-solver"; }
    }

    public QapHeuristicSolver(QapProblemInstance instance) : base(instance) { }

    public override QapProblemSolution Solve()
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();

      // TODO: Here!!!
      var assignments = GetRandomAssignments(Instance.Size);
      bool progress = true;
      int iterationCounter = 0;

      while (progress)
      {
        progress = false;
        iterationCounter++;

        var worstIdx = GetWorstIndex(assignments);
        var bestReplacement = GetBestReplacement(worstIdx, assignments);

        if (bestReplacement != worstIdx)
        {
          assignments.Swap(worstIdx, bestReplacement);
          progress = true;
        }
      }

      watch.Stop();

      return new QapProblemSolution(
        solution: assignments,
        cost: GetCost(assignments),
        steps: iterationCounter,
        visited: iterationCounter,
        timeMs: watch.ElapsedMilliseconds
      );
    }

    private int GetBestReplacement(int i, int[] assignments)
    {
      var bestReplacement = i;
      var bestReplacementCost = int.MaxValue;

      for (int j = 0; j < Instance.Size; j++)
      {
        var nextAssignments = (assignments.Clone() as int[]);
        nextAssignments.Swap(i, j);
        var cost = GetCost(nextAssignments);

        if (cost < bestReplacementCost)
        {
          bestReplacement = j;
          bestReplacementCost = cost;
        }
      }

      return bestReplacement;
    }

    private int GetWorstIndex(int[] assignments)
    {
      int size = Instance.Size;

      int worstIdx = 0;
      int worstValue = int.MinValue;

      for (int i = 0; i < size; i++)
      {
        var iValue = 0;

        for (int j = 0; j < size; j++)
        {
          if (i == j)
          {
            continue;
          }

          iValue += Instance.Flows[i, j] * Instance.Distances[assignments[i], assignments[j]];
        }

        if (iValue > worstValue)
        {
          worstIdx = i;
          worstValue = iValue;
        }
      }

      return worstIdx;
    }
  }
}
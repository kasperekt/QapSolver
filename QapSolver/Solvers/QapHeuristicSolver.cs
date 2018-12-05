using System.Diagnostics;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  class QapHeuristicSolver : QapProblemSolver
  {
    public override string Name => "heuristic-solver";
    private int IterationLimit = 1000;

    public QapHeuristicSolver(QapProblemInstance instance) : base(instance) { }

    protected override QapProblemSolution Solve()
    {
      var watch = Stopwatch.StartNew();

      var assignments = GetRandomAssignments(Instance.Size);
      var initialCost = GetCost(assignments);
      var progress = true;
      var iterationCounter = 0;

      while (progress && iterationCounter < IterationLimit)
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
        initialCost: initialCost,
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

      for (var j = 0; j < Instance.Size; j++)
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
      var size = Instance.Size;
      var worstIdx = 0;
      var worstValue = int.MinValue;

      for (var i = 0; i < size; i++)
      {
        var iValue = 0;

        for (var j = 0; j < size; j++)
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
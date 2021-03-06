using System;
using System.Linq;
using System.Collections.Generic;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  public abstract class QapProblemSolver
  {
    protected QapProblemInstance Instance { get; }
    protected int[,] DeltaTable { get; private set; }
    public abstract string Name { get; }

    protected QapProblemSolver(QapProblemInstance instance)
    {
      Instance = instance;
    }

    /// Should set value to Solution variable 
    protected abstract QapProblemSolution Solve();

    public QapProblemSolution SolveNTimes(int n, QapResultsWriter writer = null)
    {
      QapProblemSolution bestSolution = Solve();

      for (int i = 0; i < n; i++)
      {
        var solution = Solve();
        writer?.WriteResultLine(ref solution);

        if (solution.Cost < bestSolution.Cost)
        {
          bestSolution = solution;
        }
      }

      return bestSolution;
    }

    protected int GetCost(int[] assignments)
    {
      int sum = 0;

      for (int i = 0; i < Instance.Size; i++)
      {
        for (int j = 0; j < Instance.Size; j++)
        {
          if (i == j) continue;
          sum += Instance.Distances[i, j] * Instance.Flows[assignments[i], assignments[j]];
        }
      }

      return sum;
    }

    protected int[] GetRandomAssignments(int size)
    {
      var assignments = Enumerable.Range(0, size).Select(s => s).ToArray();
      assignments.Shuffle();
      return assignments;
    }

    private int CalcDelta(int[] assignments, int i, int j)
    {
      var distances = Instance.Distances;
      var flows = Instance.Flows;
      var size = Instance.Size;

      var Ai = assignments[i];
      var Aj = assignments[j];

      var gSum = 0;
      for (var g = 0; g < size; g++)
      {
        if (g == i || g == j)
        {
          continue;
        }

        var Ag = assignments[g];

        gSum += (distances[g, i] - distances[g, j]) * (flows[Ag, Aj] - flows[Ag, Ai]) +
                (distances[i, g] - distances[j, g]) * (flows[Aj, Ag] - flows[Ai, Ag]);
      }

      return (distances[i, i] - distances[j, j]) * (flows[Aj, Aj] - flows[Ai, Ai]) +
             (distances[i, j] - distances[j, i]) * (flows[Aj, Ai] - flows[Ai, Aj]) +
             gSum;
    }

    /// TODO
    protected void CalcDeltaSwap(int[] assignments, int i, int j)
    {
      var ijSwap = -DeltaTable[i, j];
    }

    protected void CalcDeltaTable(int[] assignments)
    {
      var size = Instance.Size;
      var distances = Instance.Distances;
      var flows = Instance.Flows;

      DeltaTable = new int[size, size];

      for (var i = 0; i < size; i++)
      {
        for (var j = 0; j < size; j++)
        {
          DeltaTable[i, j] = CalcDelta(assignments, i, j);
        }
      }
    }

    protected List<(int, int)> GetSwaps()
    {
      var swaps = new List<(int, int)>();

      for (var i = 0; i < Instance.Size; i++)
      {
        for (var j = i + 1; j < Instance.Size; j++)
        {
          swaps.Add((i, j));
        }
      }

      return swaps;
    }
  }
}
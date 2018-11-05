using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using QapSolver;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  abstract class QapProblemSolver
  {
    public QapProblemInstance Instance { get; private set; }
    protected int[,] DeltaTable { get; private set; }
    public abstract string Name { get; }

    public QapProblemSolver(QapProblemInstance instance)
    {
      Instance = instance;
    }

    /// Should set value to Solution variable 
    public abstract QapProblemSolution Solve();
    public abstract QapProblemSolution SolveFast();

    public QapProblemSolution SolveNTimes(int n)
    {
      QapProblemSolution bestSolution = SolveFast();

      for (int i = 0; i < n; i++)
      {
        var solution = SolveFast();

        if (solution.Cost < bestSolution.Cost)
        {
          bestSolution = solution;
        }
      }

      return bestSolution;
    }

    public QapProblemSolution SolveNTimes(int n, ref QapResultsWriter writer)
    {
      QapProblemSolution bestSolution = SolveFast();

      for (int i = 0; i < n; i++)
      {
        var solution = SolveFast();
        writer.WriteSolution(solution: ref solution);

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

    protected int CalcDelta(int[] assignments, int i, int j)
    {
      var distances = Instance.Distances;
      var flows = Instance.Flows;
      var size = Instance.Size;

      int Ai = assignments[i];
      int Aj = assignments[j];

      var gSum = 0;
      for (int g = 0; g < size; g++)
      {
        if (g == i || g == j)
        {
          continue;
        }

        int Ag = assignments[g];

        gSum += (distances[g, i] - distances[g, j]) * (flows[Ag, Aj] - flows[Ag, Ai]) +
                (distances[i, g] - distances[j, g]) * (flows[Aj, Ag] - flows[Ai, Ag]);
      }

      return (distances[i, i] - distances[j, j]) * (flows[Aj, Aj] - flows[Ai, Ai]) +
             (distances[i, j] - distances[j, i]) * (flows[Aj, Ai] - flows[Ai, Aj]) +
             gSum;
    }

    protected void CalcDeltaSwap(int[] assignments, int i, int j)
    {
      int ijSwap = -DeltaTable[i, j];
    }

    protected void CalcDeltaTable(int[] assignments)
    {
      var size = Instance.Size;
      var distances = Instance.Distances;
      var flows = Instance.Flows;

      DeltaTable = new int[size, size];

      for (int i = 0; i < size; i++)
      {
        for (int j = 0; j < size; j++)
        {
          DeltaTable[i, j] = CalcDelta(assignments, i, j);
        }
      }
    }

    protected List<(int, int)> GetSwaps()
    {
      var swaps = new List<(int, int)>();

      for (int i = 0; i < Instance.Size; i++)
      {
        for (int j = i + 1; j < Instance.Size; j++)
        {
          swaps.Add((i, j));
        }
      }

      return swaps;
    }
  }
}
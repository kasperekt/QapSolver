using System;
using System.Linq;
using System.Runtime.InteropServices;
using QapSolver;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  abstract class QapProblemSolver
  {
    public QapProblemInstance Instance { get; private set; }
    public abstract string Name { get; }

    public QapProblemSolver(QapProblemInstance instance)
    {
      Instance = instance;
    }

    /// Should set value to Solution variable 
    public abstract QapProblemSolution Solve();

    public QapProblemSolution SolveNTimes(int n)
    {
      QapProblemSolution bestSolution = Solve();

      for (int i = 0; i < n; i++)
      {
        var solution = Solve();

        if (solution.Cost < bestSolution.Cost)
        {
          bestSolution = solution;
        }
      }

      return bestSolution;
    }

    public QapProblemSolution SolveNTimes(int n, ref QapResultsWriter writer)
    {
      QapProblemSolution bestSolution = Solve();

      for (int i = 0; i < n; i++)
      {
        var solution = Solve();
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
          sum += Instance.Flows[i, j] * Instance.Distances[assignments[i], assignments[j]];
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
  }
}
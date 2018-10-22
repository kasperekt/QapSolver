using System;
using QapSolver;

namespace QapSolver.Solvers
{
  abstract class QapProblemSolver
  {
    public QapProblemInstance Instance { get; private set; }

    public QapProblemSolver(QapProblemInstance instance)
    {
      Instance = instance;
    }

    /// Should set value to Solution variable 
    public abstract QapProblemSolution Solve();

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
  }
}
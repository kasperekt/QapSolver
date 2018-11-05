using System;
using System.Collections.Generic;
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

    public override QapProblemSolution Solve()
    {
      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);
      bool progress = true;
      int iterationCounter = 0;

      while (progress)
      {
        progress = false;
        iterationCounter++;

        var neighbours = GetNeighbours(assignments);

        foreach (var neighbour in neighbours)
        {
          var neighbourCost = GetCost(neighbour);

          if (neighbourCost < cost)
          {
            assignments = neighbour;
            cost = neighbourCost;
            progress = true;
            break;
          }
        }
      }

      return new QapProblemSolution(assignments, cost, iterationCounter);
    }

    public override QapProblemSolution SolveFast()
    {
      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);
      CalcDeltaTable(assignments);

      bool progress = true;
      int iterationCounter = 0;

      while (progress)
      {
        progress = false;
        iterationCounter++;

        var swaps = GetSwaps();

        foreach (var (i, j) in swaps)
        {
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

      return new QapProblemSolution(assignments, cost, iterationCounter);
    }

    private List<int[]> GetNeighbours(int[] assignments)
    {
      List<int[]> neighbours = new List<int[]>();

      for (int i = 0; i < assignments.Length; i++)
      {
        for (int j = i + 1; j < assignments.Length; j++)
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
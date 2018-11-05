using System;
using System.Collections.Generic;
using System.Diagnostics;
using QapSolver;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  class QapLocalSolverSteepest : QapProblemSolver
  {
    public override string Name
    {
      get { return "local-steepest-solver"; }
    }

    public QapLocalSolverSteepest(QapProblemInstance instance) : base(instance) { }

    public override QapProblemSolution Solve()
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();

      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);
      bool progress = true;
      int iterationCounter = 0;
      int visited = 0;

      while (progress)
      {
        progress = false;
        iterationCounter++;

        var neighbours = GetNeighbours(assignments);

        foreach (var neighbour in neighbours)
        {
          visited++;
          var neighbourCost = GetCost(neighbour);

          if (neighbourCost < cost)
          {
            assignments = neighbour;
            cost = neighbourCost;
            progress = true;
          }
        }
      }

      watch.Stop();

      return new QapProblemSolution(
        assignments,
        cost,
        iterationCounter,
        visited,
        watch.ElapsedMilliseconds
      );
    }

    public override QapProblemSolution SolveFast()
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();

      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);
      CalcDeltaTable(assignments);

      bool progress = true;
      int iterationCounter = 0;
      int visited = 0;

      while (progress)
      {
        progress = false;
        iterationCounter++;

        var swaps = GetSwaps();
        var (bestI, bestJ) = (0, 0);
        var bestSwapCost = 0;

        foreach (var (i, j) in swaps)
        {
          visited++;

          if (DeltaTable[i, j] < bestSwapCost)
          {
            (bestI, bestJ) = (i, j);
            bestSwapCost = DeltaTable[i, j];
            progress = true;
          }
        }

        if (bestI != bestJ)
        {
          assignments.Swap(bestI, bestJ);
          cost += bestSwapCost;
          CalcDeltaTable(assignments);
        }
      }

      watch.Stop();

      return new QapProblemSolution(
        assignments,
        cost,
        iterationCounter,
        visited,
        watch.ElapsedMilliseconds
      );
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
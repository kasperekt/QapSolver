using System;
using System.Collections.Generic;
using QapSolver;
using ArrayExtensions;

namespace QapSolver.Solvers
{
  class QapLocalSolverSteepest : QapProblemSolver
  {
    public QapLocalSolverSteepest(QapProblemInstance instance) : base(instance) { }

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
          }
        }
      }

      return new QapProblemSolution(assignments, cost);
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
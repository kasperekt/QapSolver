using System;
using System.Linq;
using System.Runtime.InteropServices;
using QapSolver;

namespace QapSolver.Solvers
{
  static class ArrayExtension
  {
    public static void Shuffle(this int[] array)
    {
      Random random = new Random();
      int n = array.Length;

      while (n > 1)
      {
        n--;
        int i = random.Next(n + 1);
        int tmp = array[i];
        array[i] = array[n];
        array[n] = tmp;
      }
    }
  }

  class QapRandomSolver : QapProblemSolver
  {
    public int Rounds { get; set; }

    public QapRandomSolver(QapProblemInstance instance) : base(instance)
    {
      Rounds = 100;
    }

    public QapRandomSolver(QapProblemInstance instance, int rounds) : base(instance)
    {
      Rounds = rounds;
    }

    public override QapProblemSolution Solve()
    {
      int bestCost = int.MaxValue;
      int[] bestAssignments = null;

      for (int it = 0; it < Rounds; it++)
      {
        var assignments = GetAssignments(Instance.Size);
        var cost = GetCost(assignments);

        if (cost < bestCost)
        {
          bestCost = cost;
          bestAssignments = assignments;
        }
      }

      return new QapProblemSolution(bestAssignments, bestCost);
    }

    private int[] GetAssignments(int n)
    {
      var assignments = Enumerable.Range(0, n).Select(s => s).ToArray();
      assignments.Shuffle();
      return assignments;
    }
  }
}
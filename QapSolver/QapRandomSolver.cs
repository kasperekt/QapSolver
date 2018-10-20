using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace QapSolver
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

  class QapRandomSolver : IQapProblem
  {
    public QapProblemSolution Solve(QapProblemInstance instance)
    {
      var assignments = GetAssignments(instance.Size);
      var cost = GetCost(instance, assignments);

      return new QapProblemSolution(assignments, cost);
    }

    private int[] GetAssignments(int n)
    {
      var assignments = Enumerable.Range(0, n).Select(s => s).ToArray();
      assignments.Shuffle();
      return assignments;
    }

    private int GetCost(QapProblemInstance instance, int[] assignments)
    {
      int sum = 0;

      for (int i = 0; i < instance.Size; i++)
      {
        for (int j = 0; j < instance.Size; j++)
        {
          if (i == j) continue;
          sum += instance.Flows[i, j] * instance.Distances[assignments[i], assignments[j]];
        }
      }

      return sum;
    }
  }
}
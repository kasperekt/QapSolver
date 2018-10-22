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
    public QapRandomSolver(QapProblemInstance instance) : base(instance) { }

    public override QapProblemSolution Solve()
    {
      var assignments = GetAssignments(Instance.Size);
      var cost = GetCost(assignments);
      return new QapProblemSolution(assignments, cost);
    }

    private int[] GetAssignments(int n)
    {
      var assignments = Enumerable.Range(0, n).Select(s => s).ToArray();
      assignments.Shuffle();
      return assignments;
    }
  }
}
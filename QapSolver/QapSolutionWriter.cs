using System;
using System.IO;
using System.Linq;

namespace QapSolver
{
  class QapSolutionWriter
  {
    public static void WriteSolution(string toPath, QapProblemInstance instance, QapProblemSolution solution)
    {
      using (var writer = new StreamWriter(toPath))
      {
        writer.WriteLine($"{instance.Size} {solution.Cost}");
        string permutation = String.Join(" ", solution.Solution.Select(s => (s + 1).ToString()).ToArray());
        writer.WriteLine(permutation);
      }
    }
  }
}
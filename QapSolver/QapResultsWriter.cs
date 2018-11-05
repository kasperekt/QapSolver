using System;
using System.IO;
using System.Linq;

namespace QapSolver
{
  class QapResultsWriter
  {
    private string Path { get; set; }
    private StreamWriter Writer { get; set; }
    private int Counter = 0;

    public QapResultsWriter(string path)
    {
      Path = path;
      Writer = new StreamWriter(path);
      Writer.WriteLine("i,cost,steps,visited,time,permutation");
    }

    public void CloseWriter()
    {
      Writer.Close();
    }

    public void WriteResultLine(ref QapProblemSolution solution)
    {
      if (Writer == null)
      {
        return;
      }

      string permutation = string.Join("-",
        solution.Solution
          .Select(s => (s + 1).ToString())
          .ToArray()
      );

      string line = $"{Counter++},{solution.Cost},{solution.Steps},{solution.Visited},{solution.TimeMs},{permutation}";
      Writer.WriteLine(line);
    }
  }
}
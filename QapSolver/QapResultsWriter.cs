using System;
using System.IO;

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
      Writer.WriteLine("i,cost");
    }

    ~QapResultsWriter()
    {
      if (Writer != null)
      {
        Writer.Close();
      }
    }

    public void WriteSolution(ref QapProblemSolution solution)
    {
      if (Writer == null)
      {
        return;
      }

      string line = $"{Counter++},{solution.Cost}";
      Writer.WriteLine(line);
    }
  }
}
using System;
using System.IO;
using QapSolver.Solvers;

namespace QapSolver
{
  class QapFileLoader
  {
    private string BasePath { get; set; }

    public QapFileLoader(string basePath)
    {
      BasePath = basePath;
    }

    public QapProblemInstance Load(string name)
    {
      string filePath = $"{BasePath}/{name}";

      using (StreamReader sr = new StreamReader(filePath))
      {
        String line = sr.ReadLine();

        int size = Int32.Parse(line);
        sr.ReadLine(); // First empty line

        var flows = new int[size, size];
        var distances = new int[size, size];

        for (int i = 0; i < size; i++)
        {
          var lines = sr.ReadLine().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

          for (int j = 0; j < size; j++)
          {
            flows[i, j] = Int32.Parse(lines[j]);
          }
        }

        sr.ReadLine(); // Second empty line

        for (int i = 0; i < size; i++)
        {
          var lines = sr.ReadLine().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

          for (int j = 0; j < size; j++)
          {
            distances[i, j] = Int32.Parse(lines[j]);
          }
        }

        return new QapProblemInstance(size, flows, distances);
      }
    }
  }
}
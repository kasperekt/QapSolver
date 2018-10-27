using System;
using System.IO;
using System.Collections.Generic;
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
          var allLines = new List<int>();
          while (allLines.Count < size)
          {
            var lines = sr.ReadLine().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var element in lines)
            {
              allLines.Add(Int32.Parse(element));
            }
          }

          for (int j = 0; j < size; j++)
          {
            flows[i, j] = allLines[j];
          }
        }

        sr.ReadLine(); // Second empty line

        for (int i = 0; i < size; i++)
        {
          var allLines = new List<int>();
          while (allLines.Count < size)
          {
            var lines = sr.ReadLine().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var element in lines)
            {
              allLines.Add(Int32.Parse(element));
            }
          }

          for (int j = 0; j < size; j++)
          {
            distances[i, j] = allLines[j];
          }
        }

        return new QapProblemInstance(size, flows, distances);
      }
    }
  }
}
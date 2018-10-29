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

    private int[] GetRowValues(ref StreamReader sr, int size)
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

      return allLines.ToArray();
    }

    public QapProblemInstance Load(string name)
    {
      string filePath = $"{BasePath}/{name}";
      StreamReader sr = new StreamReader(filePath);

      int size = Int32.Parse(sr.ReadLine());
      var flows = new int[size, size];
      var distances = new int[size, size];

      for (int i = 0; i < size; i++)
      {
        var allLines = GetRowValues(ref sr, size);
        for (int j = 0; j < size; j++)
        {
          flows[i, j] = allLines[j];
        }
      }

      for (int i = 0; i < size; i++)
      {
        var allLines = GetRowValues(ref sr, size);
        for (int j = 0; j < size; j++)
        {
          distances[i, j] = allLines[j];
        }
      }

      sr.Close();

      return new QapProblemInstance(size, flows, distances);

    }
  }
}
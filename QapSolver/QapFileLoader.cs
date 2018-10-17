using System;
using System.IO;

namespace QapSolver
{
  class QapFileLoader
  {
    private string basePath;

    public QapFileLoader(string basePath)
    {
      this.basePath = basePath;
    }

    public QapProblemInstance load(string name)
    {
      string filePath = $"{basePath}/{name}";
      QapProblemInstance instance = new QapProblemInstance();

      using (StreamReader sr = new StreamReader(filePath))
      {
        String line = sr.ReadLine();

        instance.size = Int32.Parse(line);
        sr.ReadLine(); // First empty line

        instance.facilities = new int[instance.size, instance.size];

        for (int i = 0; i < instance.size; i++)
        {
          var lines = sr.ReadLine().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

          for (int j = 0; j < instance.size; j++)
          {
            instance.facilities[i, j] = Int32.Parse(lines[j]);
          }
        }

        sr.ReadLine(); // Second empty line

        instance.locations = new int[instance.size, instance.size];

        for (int i = 0; i < instance.size; i++)
        {
          var lines = sr.ReadLine().Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

          for (int j = 0; j < instance.size; j++)
          {
            instance.locations[i, j] = Int32.Parse(lines[j]);
          }
        }
      }

      return instance;
    }
  }
}
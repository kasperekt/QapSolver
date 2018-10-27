using System;
using System.IO;
using System.Linq;
using QapSolver.Solvers;

namespace QapSolver
{
  class Program
  {
    static QapFileLoader Loader { get; set; }

    static void Main(string[] args)
    {
      Loader = new QapFileLoader("./Data");
      var fileNames = GetFileNames("./Data");
      // TestAllFiles(fileNames);
      TestFile("tai35b.dat");
    }

    static string[] GetFileNames(string directoryPath)
    {
      DirectoryInfo dInfo = new DirectoryInfo(directoryPath);
      FileInfo[] datFiles = dInfo.GetFiles("*.dat");
      string[] fileNames = datFiles.Select(d => d.Name).ToArray();
      return fileNames;
    }

    static void TestAllFiles(string[] fileNames)
    {
      int rounds = 500;
      foreach (var fileName in fileNames)
      {
        Console.WriteLine($"----- {fileName} (Rounds: {rounds}) -----");
        var problemInstance = Loader.Load(fileName);
        var solver = new QapRandomSolver(problemInstance);
        Console.WriteLine($"Cost = {solver.SolveNTimes(rounds).Cost}");
      }
    }

    static void TestFile(string fileName)
    {
      var problemInstance = Loader.Load(fileName);

      // var solver = new QapRandomSolver(problemInstance, 1000);
      var solver = new QapLocalSolverGreedy(problemInstance);
      var problemSolution = solver.SolveNTimes(1000);

      Console.WriteLine($"Cost = {problemSolution.Cost}");
    }
  }
}

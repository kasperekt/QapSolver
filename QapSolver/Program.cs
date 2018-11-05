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
      // TestAllFiles(fileNames.Take(3).ToArray());
      TestAllFiles(new string[] { "chr20a.dat", "bur26a.dat" });
      // TestFile("tai12a.dat");
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
      foreach (var fileName in fileNames)
      {
        int[] differentRounds = { 200, 250, 300, 350, 400 };
        foreach (var rounds in differentRounds)
        {
          Console.WriteLine($"----- {fileName} (Rounds: {rounds}) -----");
          var problemInstance = Loader.Load(fileName);

          QapProblemSolver[] solvers = {
            new QapRandomSolver(problemInstance),
            new QapLocalSolverGreedy(problemInstance),
            new QapLocalSolverSteepest(problemInstance)
          };

          foreach (var solver in solvers)
          {
            // var solver = new QapLocalSolverGreedy(problemInstance);
            string resultsFileName = $"results/{solver.Name}-{rounds}.csv";
            Console.WriteLine($"Cost = {solver.SolveNTimes(rounds).Cost}");
          }
        }
      }
    }

    static void TestFile(string fileName)
    {
      var problemInstance = Loader.Load(fileName);
      var solver = new QapLocalSolverSteepest(problemInstance);
      var problemSolution = solver.SolveNTimes(500);

      Console.WriteLine($"Cost = {problemSolution.Cost}");
    }
  }
}

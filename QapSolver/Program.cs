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
      TestAllFiles(fileNames);
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
        int[] differentRounds = { 100, 150, 200, 250, 300 };

        foreach (var rounds in differentRounds)
        {
          var problemInstance = Loader.Load(fileName);

          QapProblemSolver[] solvers = {
            new QapRandomSolver(problemInstance),
            new QapHeuristicSolver(problemInstance),
            new QapLocalSolverGreedy(problemInstance),
            new QapLocalSolverSteepest(problemInstance)
          };

          foreach (var solver in solvers)
          {
            string csvFileName = $"results/{problemInstance.Name}_{solver.Name}_{rounds}.csv";
            string slnFileName = $"results/{problemInstance.Name}_{solver.Name}_{rounds}.sln";

            QapResultsWriter resultsWriter = new QapResultsWriter(csvFileName);
            var solution = solver.SolveNTimes(rounds, resultsWriter);
            resultsWriter.CloseWriter();

            QapSolutionWriter.WriteSolution(slnFileName, problemInstance, solution);
            Console.WriteLine($"({problemInstance.Name}, {solver.Name}): Rounds = {rounds}, Cost = {solution.Cost}");
          }
        }
      }
    }
  }
}

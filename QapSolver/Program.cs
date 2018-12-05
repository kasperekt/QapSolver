using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QapSolver.Solvers;

namespace QapSolver
{
  static class Program
  {
    private static QapFileLoader Loader { get; set; }

    private static void Main(string[] args)
    {
      Loader = new QapFileLoader("./Data");
      var fileNames = GetFileNames("./Data");
//      TestAllFiles(fileNames);
      
      TestSingleSolver<QapTabooSearch>("chr18a.dat", 100);
      TestSingleSolver<QapLocalSolverSteepest>("chr18a.dat", 100);
    }
    

    private static IEnumerable<string> GetFileNames(string directoryPath)
    {
      var dInfo = new DirectoryInfo(directoryPath);
      var datFiles = dInfo.GetFiles("*.dat");
      var fileNames = datFiles.Select(d => d.Name).ToArray();
      return fileNames;
    }

    private static void TestSingleSolver<T>(string fileName, int rounds = 300) where T: QapProblemSolver
    {
      var problemInstance = Loader.Load(fileName);
      
      QapProblemSolver solver = Activator.CreateInstance(typeof(T), problemInstance) as T;
      
      var solution = solver?.SolveNTimes(rounds);
      
      Console.WriteLine($"({problemInstance.Name}, {solver?.Name}): Cost = {solution?.Cost}");
    }

    private static void TestAllFiles(IEnumerable<string> fileNames)
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
            var csvFileName = $"results/{problemInstance.Name}_{solver.Name}_{rounds}.csv";
            var slnFileName = $"results/{problemInstance.Name}_{solver.Name}_{rounds}.sln";

            var resultsWriter = new QapResultsWriter(csvFileName);
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

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
      // var fileNames = GetFileNames("./Data");
      // var anyFile = fileNames[0];
      TestFile("chr12a.dat");
    }

    static string[] GetFileNames(string directoryPath)
    {
      DirectoryInfo dInfo = new DirectoryInfo(directoryPath);
      FileInfo[] datFiles = dInfo.GetFiles("*.dat");
      string[] fileNames = datFiles.Select(d => d.Name).ToArray();
      return fileNames;
    }

    static void TestFile(string fileName)
    {
      var problemInstance = Loader.Load(fileName);

      var randomSolver = new QapRandomSolver(problemInstance);
      var problemSolution = randomSolver.Solve();

      Console.WriteLine($"Done with {problemSolution.Cost}");
    }
  }
}

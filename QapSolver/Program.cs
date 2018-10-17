using System;

namespace QapSolver
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      var loader = new QapFileLoader("./Data");
      var instance = loader.load("bur26a.dat");
    }
  }
}

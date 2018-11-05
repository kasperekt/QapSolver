using System;

namespace QapSolver
{
  public class QapProblemInstance
  {
    public string Name { get; set; }
    public int Size { get; set; }
    public int[,] Flows { get; set; }
    public int[,] Distances { get; set; }

    public QapProblemInstance(string name, int size, int[,] flows, int[,] distances)
    {
      Name = name;
      Size = size;
      Flows = flows;
      Distances = distances;
    }
  }
}
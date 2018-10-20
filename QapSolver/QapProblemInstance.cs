namespace QapSolver
{
  public class QapProblemInstance
  {
    public int Size { get; set; }
    public int[,] Flows { get; set; }
    public int[,] Distances { get; set; }

    public QapProblemInstance(int size, int[,] flows, int[,] distances)
    {
      Size = size;
      Flows = flows;
      Distances = distances;
    }
  }
}
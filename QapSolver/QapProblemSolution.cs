using System;

namespace QapSolver
{
  public class QapProblemSolution
  {
    public int[] Solution { get; set; }
    public int Cost { get; set; }

    public QapProblemSolution(int[] solution, int cost)
    {
      Solution = solution;
      Cost = cost;
    }
  }
}
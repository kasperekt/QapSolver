using System;

namespace QapSolver
{
  public class QapProblemSolution
  {
    public int[] Solution { get; set; }
    public int Cost { get; set; }
    public int Steps { get; set; }

    public QapProblemSolution(int[] solution, int cost)
    {
      Solution = solution;
      Cost = cost;
      Steps = 0;
    }

    public QapProblemSolution(int[] solution, int cost, int steps)
    {
      Solution = solution;
      Cost = cost;
      Steps = steps;
    }
  }
}
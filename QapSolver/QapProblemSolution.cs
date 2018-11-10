using System;

namespace QapSolver
{
  public class QapProblemSolution
  {
    public int[] Solution { get; private set; }
    public int Cost { get; private set; }
    public int InitialCost { get; private set; }
    public int Steps { get; private set; }
    public int Visited { get; private set; }
    public long TimeMs { get; private set; }

    public QapProblemSolution(int[] solution, int cost)
    {
      Solution = solution;
      InitialCost = cost;
      Cost = cost;
      Steps = 0;
      Visited = 0;
      TimeMs = 0;
    }

    public QapProblemSolution(int[] solution, int initialCost, int cost, int steps, int visited, long timeMs)
    {
      Solution = solution;
      InitialCost = initialCost;
      Cost = cost;
      Steps = steps;
      Visited = visited;
      TimeMs = timeMs;
    }
  }
}
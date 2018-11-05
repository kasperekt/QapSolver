using System;
using System.Diagnostics;

namespace QapSolver.Solvers
{
  class QapHeuristicSolver : QapProblemSolver
  {
    public override string Name
    {
      get { return "heuristic-solver"; }
    }

    public QapHeuristicSolver(QapProblemInstance instance) : base(instance) { }

    public override QapProblemSolution Solve()
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();

      // TODO: Here!!!
      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);

      watch.Stop();

      return new QapProblemSolution(assignments, cost, 0, 0, watch.ElapsedMilliseconds);
    }
  }
}
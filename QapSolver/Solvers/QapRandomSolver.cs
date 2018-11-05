using System;
using System.Linq;
using ArrayExtensions;
using QapSolver;

namespace QapSolver.Solvers
{
  class QapRandomSolver : QapProblemSolver
  {
    public override string Name
    {
      get { return "random-solver"; }
    }

    public QapRandomSolver(QapProblemInstance instance) : base(instance)
    {
    }

    public override QapProblemSolution Solve()
    {
      var assignments = GetRandomAssignments(Instance.Size);
      var cost = GetCost(assignments);
      return new QapProblemSolution(assignments, cost);
    }
  }
}
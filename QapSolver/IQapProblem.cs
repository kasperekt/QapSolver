namespace QapSolver
{
  interface IQapProblem
  {
    QapProblemSolution Solve(QapProblemInstance instance);
  }
}
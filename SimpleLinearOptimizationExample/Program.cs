using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLinearOptimizationExample
{

    // https://developers.google.com/optimization/introduction/using#csharp
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---- Linear programming example with GLOP (recommended) ----");
            RunLinearExample("GLOP_LINEAR_PROGRAMMING");

            Console.WriteLine("---- Linear programming example with CLP ----");
            RunLinearExample("CLP_LINEAR_PROGRAMMING");
        }

        private static void RunLinearExample(string solverType)
        {
            // * Declare the solver.
            Solver solver = Solver.CreateSolver("LinearExample", solverType);

            // * Create the variables.

            // x and y are continuous non-negative variables.
            Variable x = solver.MakeNumVar(0.0, double.PositiveInfinity, "x");
            Variable y = solver.MakeNumVar(0.0, double.PositiveInfinity, "y");

            // * Define the constraints.

            // x + 2y <= 14.
            Constraint c0 = solver.MakeConstraint(double.NegativeInfinity, 14.0);
            c0.SetCoefficient(x, 1);
            c0.SetCoefficient(y, 2);

            // 3x - y >= 0.
            Constraint c1 = solver.MakeConstraint(0.0, double.PositiveInfinity);
            c1.SetCoefficient(x, 3);
            c1.SetCoefficient(y, -1);

            // x - y <= 2.
            Constraint c2 = solver.MakeConstraint(double.NegativeInfinity, 2.0);
            c2.SetCoefficient(x, 1);
            c2.SetCoefficient(y, -1);

            // Define the objective function.

            // Objective function: 3x + 4y.
            Objective objective = solver.Objective();
            objective.SetCoefficient(x, 3);
            objective.SetCoefficient(y, 4);
            objective.SetMaximization();

          

            Console.WriteLine("Number of variables = " + solver.NumVariables());
            Console.WriteLine("Number of constraints = " + solver.NumConstraints());

            // * Invoke the solver and display the results.
            solver.Solve();

            // The value of each variable in the solution.
            Console.WriteLine("Solution:");

            Console.WriteLine("x = " + x.SolutionValue());
            Console.WriteLine("y = " + y.SolutionValue());

            // The objective value of the solution.
            Console.WriteLine("Optimal objective value = " +
                              solver.Objective().Value());


            Console.WriteLine("Problem solved in " + solver.WallTime() + " milliseconds");
            Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");

            double[] activities = solver.ComputeConstraintActivities();

            Console.WriteLine("Advanced usage:");
            Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");
            Console.WriteLine("x: reduced cost = " + x.ReducedCost());
            Console.WriteLine("y: reduced cost = " + y.ReducedCost());
            Console.WriteLine("c0: dual value = " + c0.DualValue());
            Console.WriteLine("    activity = " + activities[c0.Index()]);
            Console.WriteLine("c1: dual value = " + c1.DualValue());
            Console.WriteLine("    activity = " + activities[c1.Index()]);
            Console.WriteLine("c2: dual value = " + c2.DualValue());
            Console.WriteLine("    activity = " + activities[c2.Index()]);



        }
    }
}

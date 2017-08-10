using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;

namespace TSPExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }

        private static void Test()
        {
            string[] city_names = { "New York", "Los Angeles", "Chicago", "Minneapolis", "Denver", "Dallas", "Seattle",
                "Boston", "San Francisco", "St. Louis", "Houston", "Phoenix", "Salt Lake City" };

            int tsp_size = city_names.Length;

            // The number of routes, which is 1 in the TSP.
            int num_routes = 1;

            // The depot is the starting node of the route.
            int depot = 0;



            // Create routing model
            if (tsp_size>0)
            {
                RoutingModel routing = new RoutingModel(tsp_size, num_routes, depot);
                var search_parameters = RoutingModel.DefaultSearchParameters();

                // Setting first solution heuristic: the
                // method for finding a first solution to the problem.
                search_parameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

                // Setting the cost function

                // Create the distance callback, which takes two arguments (the from and to node indices)
                // and returns the distance between these nodes.

                //  routing.SetCost(new ConstantCallback());

                routing.SetArcCostEvaluatorOfAllVehicles(new DistanceCallback());

                var assignment = routing.SolveWithParameters(search_parameters);


                Console.WriteLine("Status = {0}", routing.Status());

                if (assignment != null)
                {
                    // Solution cost.
                    Console.WriteLine("Total distance: = {0}", assignment.ObjectiveValue());
                    // Inspect solution.
                    
                    // Only one route here; otherwise iterate from 0 to routing.vehicles() - 1
                    int route_number = 0;

                    long index = routing.Start(route_number); // Index of the variable for the starting node.

                    string route = string.Empty;

                    while (!routing.IsEnd(index))
                    {
                        // Convert variable indices to node indices in the displayed route.

                        route += city_names[routing.IndexToNode(index)] + " -> ";

                        index = assignment.Value(routing.NextVar(index));

                        route += city_names[routing.IndexToNode(index)];
                    }


                    Console.WriteLine($"Route: {route}");

                }
                else
                {
                    Console.WriteLine("No solution found.");
                }


            }
        }
    }
}

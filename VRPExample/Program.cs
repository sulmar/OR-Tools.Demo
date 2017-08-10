using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }

        private static void Test()
        {
            int[,] locations =
            {
               {82, 76}, {96, 44}, {50, 5}, {49, 8}, {13, 7}, {29, 89}, {58, 30}, {84, 39},
               {14, 24}, {12, 39}, {3, 82}, {5, 10}, {98, 52}, {84, 25}, {61, 59}, {1, 65},
               {88, 51}, {91, 2}, {19, 32}, {93, 3}, {50, 93}, {98, 14}, {5, 42}, {42, 9},
               {61, 62}, {9, 97}, {80, 55}, {57, 69}, {23, 15}, {20, 70}, {85, 60}, {98, 5}
            };


            int[] demands = {0, 19, 21, 6, 19, 7, 12, 16, 6, 16, 8, 14, 21, 16, 3, 22, 18,
             19, 1, 24, 8, 12, 4, 8, 24, 24, 2, 20, 15, 2, 14, 9 };

            int num_locations = locations.Length;
            int depot = 0;    // The depot is the start and end point of each route.
            int num_vehicles = 5;



            // Create routing model
            if (locations.Length > 0)
            {
                RoutingModel routing = new RoutingModel(num_locations, num_vehicles, depot);
                var search_parameters = RoutingModel.DefaultSearchParameters();

                // Setting first solution heuristic: the
                // method for finding a first solution to the problem.
                search_parameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

                routing.SetArcCostEvaluatorOfAllVehicles(new DistanceLocationsCallback(locations));


            }
        }
    }
}

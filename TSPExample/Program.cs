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
            VRPTest();

            Test();
        }

        private static void VRPTest()
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

            int num_locations = locations.GetLength(0);
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

                var distanceCallback = new DistanceLocationsCallback(locations);

                Display(distanceCallback.matrix);

                
                routing.SetArcCostEvaluatorOfAllVehicles(distanceCallback);


                //  Add a dimension for demand.

                long slack_max = 0;
                long vehicle_capacity = 100;
                bool fix_start_cumul_to_zero = true;
                string demand = "Demand";

                routing.AddDimension(new DemandCallback(demands), slack_max, vehicle_capacity, fix_start_cumul_to_zero, demand);

                // Solve, displays a solution if any.
                var assignment = routing.SolveWithParameters(search_parameters);


                Console.WriteLine("Status = {0}", routing.Status());

                if (assignment != null)
                {
                    // Solution cost.
                    Console.WriteLine("Total distance of all routes: {0}", assignment.ObjectiveValue());
                    // Inspect solution.

                    // Only one route here; otherwise iterate from 0 to routing.vehicles() - 1

                    for (int vehicle_nbr = 0; vehicle_nbr  < num_vehicles; vehicle_nbr ++)
                    {
                        long index = routing.Start(vehicle_nbr);
                        long index_next = assignment.Value(routing.NextVar(index));
                        string route = string.Empty;
                        long route_dist = 0;
                        long route_demand = 0;

                        int node_index;
                        int node_index_next;

                        while (!routing.IsEnd(index_next))
                        {
                            // Convert variable indices to node indices in the displayed route.

                            node_index = routing.IndexToNode(index);
                            node_index_next = routing.IndexToNode(index_next);

                            route += node_index + " -> ";

                            // Add the distance to the next node.

                            route_dist += distanceCallback.Run(node_index, node_index_next);
                            // # Add demand.
                            route_demand += demands[node_index_next];
                            index = index_next;
                            index_next = assignment.Value(routing.NextVar(index));
                        }

                        node_index = routing.IndexToNode(index);
                        node_index_next = routing.IndexToNode(index_next);
                        route += node_index + " -> " + node_index_next;
                        route_dist += distanceCallback.Run(node_index, node_index_next);

                        Console.WriteLine("Route for vehicle " + vehicle_nbr + ":\n\n" + route + "\n");
                        Console.WriteLine("Distance of route " + vehicle_nbr + ": " + route_dist);
                        Console.WriteLine("Demand met by vehicle " + vehicle_nbr + ": " + route_demand + "\n");

                        // Console.WriteLine($"Route: {route}");

                    }





                }
                else
                {
                    Console.WriteLine("No solution found.");
                }


            }
        }

        private static void Display(int[,] array)
        {
            int rowLength = array.GetLength(0);
            int colLength = array.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", array[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
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

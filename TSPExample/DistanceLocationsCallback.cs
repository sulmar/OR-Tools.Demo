using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace TSPExample
{
    class DistanceLocationsCallback : NodeEvaluator2
    {
        public int[,] matrix;


        public DistanceLocationsCallback(int[,] locations)
        {
            int size = locations.GetLength(0);

            this.matrix = new int[size, size];

            for (int from_node = 0; from_node < size; ++from_node)
            {
                for (int to_node = 0; to_node < size; to_node++)
                {
                    var x1 = locations[from_node, 0];
                    var y1 = locations[from_node, 1];
                    var x2 = locations[to_node, 0];
                    var y2 = locations[to_node, 1];

                    this.matrix[from_node, to_node] = Distance(x1, y1, x2, y2);
                }

            }
        }

        public override long Run(int i, int j)
        {
            return matrix[i, j];
        }

        //  Manhattan distance. 
        private int Distance(int x1, int y1, int x2, int y2) => Abs(x1 - x2) + Abs(y1 - y2);


    }
}

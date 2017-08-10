using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPExample
{
    class DemandCallback : NodeEvaluator2
    {
        public int[] matrix;

        public DemandCallback(int[] demands)
        {
            this.matrix = demands;
        }

        public override long Run(int i, int j)
        {
            return this.matrix[i];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberCortex
{
    class Strategy
    {
        public static double[] transformDataForStrategy(double[,] data)
        {
            double[] tranformedData = new double[] { 5.1, 2.5, 3, 1.1};

            /*
            for (int i = 0; i < tranformedData.GetLength(0); i++)
            {
                tranformedData[i] = data[0, i];
            }*/

            return tranformedData;
        }
    }
}

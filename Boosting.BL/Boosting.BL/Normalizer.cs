using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boosting.BL
{
    public static class Normalizer
    {
        private static double[] _sigma;
        private static double[] _average;
        private static double _size = 0;

        public static void GetСharacteristicsData(double[,] dataForСharacteristics)
        {
            _sigma = new double[dataForСharacteristics.GetLength(1)];
            _average = new double[dataForСharacteristics.GetLength(1)];
            _size = Convert.ToDouble(dataForСharacteristics.GetLength(0));

            for (int j = 0; j < dataForСharacteristics.GetLength(1); j++)
            {
                _average[j] = 0;
                for (int i = 0; i < dataForСharacteristics.GetLength(0); i++)
                {
                    _average[j] += dataForСharacteristics[i, j];
                }
                _average[j] = _average[j] / _size;

                _sigma[j] = 0;
                for (int i = 0; i < dataForСharacteristics.GetLength(0); i++)
                {
                    _sigma[j] += (dataForСharacteristics[i, j] - _average[j]) * (dataForСharacteristics[i, j] - _average[j]);
                }
                _sigma[j] = Math.Sqrt(_sigma[j] / (_size - 1));
            }
        }

        public static void GetNormalData(double[,] dataForNormalizer)
        {
            for (int j = 0; j < dataForNormalizer.GetLength(1); j++)
            {
                for (int i = 0; i < dataForNormalizer.GetLength(0); i++)
                {
                    dataForNormalizer[i, j] = (dataForNormalizer[i, j] - _average[j]) / _sigma[j];
                    dataForNormalizer[i, j] = (Math.Exp(dataForNormalizer[i, j]) - Math.Exp(-dataForNormalizer[i, j])) / (Math.Exp(dataForNormalizer[i, j]) + Math.Exp(-dataForNormalizer[i, j]));
                }
            }
        }

        public static void GetNormalForNewData(double[] dataForNormalizer)
        {
            for (int i = 0; i < dataForNormalizer.Length; i++)
            {
                dataForNormalizer[i] = (dataForNormalizer[i] - _average[i]) / _sigma[i];
                dataForNormalizer[i] = (Math.Exp(dataForNormalizer[i]) - Math.Exp(-dataForNormalizer[i])) / (Math.Exp(dataForNormalizer[i]) + Math.Exp(-dataForNormalizer[i]));
            }
        }
    }
}

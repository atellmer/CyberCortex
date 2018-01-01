using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boosting.BL
{
    public class StrongClassifier
    {
        private WeakClassifier _weak;
        private double[,] _classifiers;
        private int _numberClassifiers;
        private double[,] _data;
        private double[,] _answer;
        private double[] _weights;
        private int _length;
        private int _size;

        public StrongClassifier(int size, int length, int numberClassifiers)
        {
            this._weak = new WeakClassifier(size, length);
            this._classifiers = new double[numberClassifiers, 4];
            this._numberClassifiers = numberClassifiers;
            this._data = new double[size, length];
            this._answer = new double[size, 1];
            this._weights = new double[size];
            this._length = length;
            this._size = size;
        }

        public double[,] Train(double[,] samples, double[,] answers, string nameThread)
        {
            double[] simpleClassifier = new double[3];
            double[] pattern = new double[_length];
            double[] outer = new double[_size];
            double weightsSum = 0;
            double epsilon = 0;
            double alfa = 0;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    _data[i, j] = samples[i, j];
                }
                _answer[i, 0] = answers[i, 0];
            }

            ProbSelection selection = new ProbSelection(_size, _length);

            for (int i = 0; i < _size; i++)
            {
                _weights[i] = 1 / Convert.ToDouble(_size);
            }

            for (int K = 0; K < _numberClassifiers; K++)
            {
                double[] simple = _weak.Train(_data, _answer);

                for (int i = 0; i < simpleClassifier.Length; i++)
                {
                    simpleClassifier[i] = simple[i];
                }

                epsilon = 0;
                weightsSum = 0;
                alfa = 0;

                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _length; j++)
                    {
                        pattern[j] = samples[i, j];
                    }
                    outer[i] = GetWeakPredict(simpleClassifier, pattern);

                    if (answers[i, 0] != outer[i])
                    {
                        epsilon += _weights[i];
                    }
                }

                if (epsilon == 0)
                {
                    epsilon = 0.000000000000001;
                }
                if (epsilon == 1)
                {
                    epsilon = 0.999999999999999;
                }

                alfa = 0.5 * Math.Log((1 - epsilon) / epsilon);

                for (int j = 0; j < simpleClassifier.Length; j++)
                {
                    _classifiers[K, j] = simpleClassifier[j];
                }
                _classifiers[K, 3] = alfa;

                //////////////////////////////////////////             
                if (EmergenceLog!=null)
                {                  
                    EmergenceLog(this, e="Поток № "+nameThread +" | Классификатор № " + Convert.ToString(K + 1) +" | Вероятность ошибки: " + Convert.ToString(Math.Round(epsilon, 4)) + " | Надежность: " + Convert.ToString(Math.Round(alfa, 4))+"\r\n");
                }             
                //////////////////////////////////////////

                for (int i = 0; i < _size; i++)
                {
                    weightsSum += _weights[i] * Math.Exp(-1 * alfa * answers[i, 0] * outer[i]);
                }

                for (int i = 0; i < _size; i++)
                {
                    _weights[i] = (_weights[i] * Math.Exp(-1 * alfa * answers[i, 0] * outer[i])) / weightsSum;
                }

                selection.RunSelector(_weights, _data, _answer, samples, answers);
            }
            return _classifiers;
        }

        public double GetWeakPredict(double[] classifier, double[] pattern)
        {
            return _weak.GetPredict(pattern[Convert.ToInt32(classifier[0])], classifier[1], classifier[2]);
        }

        public double GetPredict(double[] pattern, double[,] classifiers)
        {
            double[] simpleClassifier = new double[3];
            double Out = 0;

            for (int K = 0; K < classifiers.GetLength(0); K++)
            {
                for (int i = 0; i < simpleClassifier.Length; i++)
                {
                    simpleClassifier[i] = classifiers[K, i];
                }
                // out += alfa * out_weak_classifier
                Out += classifiers[K, 3] * GetWeakPredict(simpleClassifier, pattern);
            }

            if (Out > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public event EventHandler<string> EmergenceLog;

        string e { get; set; }
    }
}

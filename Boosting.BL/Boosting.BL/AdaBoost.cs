using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Boosting.BL
{
    public interface iAdaBoost
    {
        double[,] GetSample();
        double[,] GetAnswer();
        double[,] GetTestSample();
        double[,] GetTestAnswer();
        void SetSample(double[,] value);
        void SetAnswer(double[,] value);
        void SetTestSample(double[,] value);
        void SetTestAnswer(double[,] value);
        void SetSize(int value);
        void SetTestSize(int value);
        void SetLength(int value);
        void SetNumberClassifiers(int value);
        void SetNumberClass(int value);
        void SetClassifiers(double[,] value);
        double[,] GetClassifiers();
        string RunTrain();
        string GetTest();
        double GetNewPredict(double[] newPattern);
        void GetСharacteristicData(double[,] dataForСharacteristic);
        void GetNormalData(double[,] dataForNormalizer);
        void GetNormalForNewData(double[] dataForNormalizer);

        event EventHandler<string> EmergenceLog;
    }

    public class AdaBoost : iAdaBoost
    {
        private static int _length = 0;
        private static int _size = 0;
        private static int _testSize = 0;
        private static int _numberClassifiers = 0;
        private static int _numberClass = 0;

        private static double[,] _samples;
        private static double[,] _answers;
        private static double[,] _testSamples;
        private static double[,] _testAnswers;
        private static double[,] _classifiers;

        struct TrainStruct
        {
            internal int numberClass;
            internal string nameThread;
        }

        public string RunTrain()
        {
            string errorInfo = null;
            Thread[] thread = new Thread[_numberClass];

            for (int K = 0; K < _numberClass; K++)
            {
                thread[K] = new Thread(Train);
                thread[K].Name = Convert.ToString(K + 1);

                TrainStruct trainStruct = new TrainStruct();
                trainStruct.numberClass = K;
                trainStruct.nameThread = thread[K].Name;

                thread[K].IsBackground = true;
                thread[K].Start(trainStruct);
            }

            while (true)
            {
                int countDeadThreads = 0;

                for (int K = 0; K < _numberClass; K++)
                {
                    if (!thread[K].IsAlive)
                    {
                        countDeadThreads++;
                    }
                }

                if (countDeadThreads == _numberClass)
                {
                    errorInfo = GetTest();
                    break;
                }
            }

            return errorInfo;
        }

        double[,] TransformData(double[,] AnswersForTransforms, int K)
        {
            double[,] transformAnswers = new double[AnswersForTransforms.GetLength(0), 1];

            for (int i = 0; i < AnswersForTransforms.GetLength(0); i++)
            {
                if (AnswersForTransforms[i, 0] != K)
                {
                    transformAnswers[i, 0] = -1;
                }
                else
                {
                    transformAnswers[i, 0] = 1;
                }
            }

            return transformAnswers;
        }

        void Train(object K)
        {
            if (K.GetType() != typeof(TrainStruct))
            {
                return;
            }
            TrainStruct trainStruct = (TrainStruct)K;

            double[,] classifiers = new double[_numberClassifiers, 4];
            double[,] transformAnswers = new double[_size, 1];

            transformAnswers = TransformData(_answers, trainStruct.numberClass + 1);
            StrongClassifier strong = new StrongClassifier(_size, _length, _numberClassifiers);

            strong.EmergenceLog += strong_EmergenceLog;

            classifiers = strong.Train(_samples, transformAnswers, trainStruct.nameThread);
            int m = 0;
            for (int i = _numberClassifiers * trainStruct.numberClass; i < _numberClassifiers + _numberClassifiers * trainStruct.numberClass; i++)
            {
                for (int j = 0; j < _classifiers.GetLength(1); j++)
                {
                    _classifiers[i, j] = classifiers[m, j];
                }
                m++;
            }
        }

        void strong_EmergenceLog(object sender, string e)
        {
            string informer = null;
            informer = e;

            if (EmergenceLog != null)
            {
                EmergenceLog(this, e = informer);
            }
        }

        public string GetTest()
        {
            string errorInfo = null;
            double[] pattern = new double[_length];
            double[] Out = new double[_numberClass];
            double predict = 0;
            double errorInLearn = 0;
            double errorInTest = 0;
            double[,] classifiers = new double[_numberClassifiers, 4];

            StrongClassifier strongForTestInSample = new StrongClassifier(_size, _length, _numberClassifiers);
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    pattern[j] = _samples[i, j];
                }

                Array.Clear(Out, 0, Out.Length);
                for (int K = 0; K < _numberClass; K++)
                {
                    int m = 0;
                    Array.Clear(classifiers, 0, classifiers.Length);
                    for (int l = _numberClassifiers * K; l < _numberClassifiers + _numberClassifiers * K; l++)
                    {
                        for (int j = 0; j < _classifiers.GetLength(1); j++)
                        {
                            classifiers[m, j] = _classifiers[l, j];
                        }
                        m++;
                    }
                    Out[K] = strongForTestInSample.GetPredict(pattern, classifiers);
                }
                predict = Array.IndexOf(Out, Out.Max()) + 1;

                if ((Math.Abs(_answers[i, 0] - predict)) != 0)
                {
                    errorInLearn++;
                }
            }

            StrongClassifier strongForTestInOutSample = new StrongClassifier(_testSize, _length, _numberClassifiers);
            for (int i = 0; i < _testSize; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    pattern[j] = _testSamples[i, j];
                }

                Array.Clear(Out, 0, Out.Length);
                for (int K = 0; K < _numberClass; K++)
                {
                    int m = 0;
                    Array.Clear(classifiers, 0, classifiers.Length);
                    for (int l = _numberClassifiers * K; l < _numberClassifiers + _numberClassifiers * K; l++)
                    {
                        for (int j = 0; j < _classifiers.GetLength(1); j++)
                        {
                            classifiers[m, j] = _classifiers[l, j];
                        }
                        m++;
                    }
                    Out[K] = strongForTestInOutSample.GetPredict(pattern, classifiers);
                }
                predict = Array.IndexOf(Out, Out.Max()) + 1;

                if ((Math.Abs(_testAnswers[i, 0] - predict)) != 0)
                {
                    errorInTest++;
                }
            }


            errorInfo = new string('*', 90) + "\r\n" + "Ошибка на обучении: " + Convert.ToString(Math.Round((errorInLearn / Convert.ToDouble(_size) * 100), 4)) + " %" + "\r\n";
            errorInfo += "Ошибка на тестировании: " + Convert.ToString(Math.Round((errorInTest / Convert.ToDouble(_testSize) * 100), 4)) + " %" + "\r\n";

            return errorInfo;
        }

        public double GetNewPredict(double[] newPattern)
        {
            double[,] classifiers = new double[_numberClassifiers, 4];
            double[] Out = new double[_numberClass];
            double predict = 0;

            StrongClassifier strongForNewPredict = new StrongClassifier(_size, _length, _numberClassifiers);

            for (int K = 0; K < _numberClass; K++)
            {
                int m = 0;
                Array.Clear(classifiers, 0, classifiers.Length);
                for (int l = _numberClassifiers * K; l < _numberClassifiers + _numberClassifiers * K; l++)
                {
                    for (int j = 0; j < _classifiers.GetLength(1); j++)
                    {
                        classifiers[m, j] = _classifiers[l, j];
                    }
                    m++;
                }
                Out[K] = strongForNewPredict.GetPredict(newPattern, classifiers);
            }
            predict = Array.IndexOf(Out, Out.Max()) + 1;

            return predict;
        }

        public void GetСharacteristicData(double[,] dataForСharacteristic)
        {
            Normalizer.GetСharacteristicsData(dataForСharacteristic);
        }

        public void GetNormalData(double[,] dataForNormalizer)
        {
            Normalizer.GetNormalData(dataForNormalizer);
        }

        public void GetNormalForNewData(double[] dataForNormalizer)
        {
            Normalizer.GetNormalForNewData(dataForNormalizer);
        }

        public double[,] GetSample()
        {
            return _samples;
        }

        public double[,] GetAnswer()
        {
            return _answers;
        }

        public void SetSample(double[,] value)
        {
            _samples = value;
        }

        public void SetAnswer(double[,] value)
        {
            _answers = value;
        }

        public double[,] GetTestSample()
        {
            return _testSamples;
        }

        public double[,] GetTestAnswer()
        {
            return _testAnswers;
        }

        public void SetTestSample(double[,] value)
        {
            _testSamples = value;
        }

        public void SetTestAnswer(double[,] value)
        {
            _testAnswers = value;
        }

        public void SetSize(int value)
        {
            _size = value;
        }

        public void SetTestSize(int value)
        {
            _testSize = value;
        }

        public void SetLength(int value)
        {
            _length = value;
        }

        public void SetNumberClassifiers(int value)
        {
            _numberClassifiers = value;
        }

        public void SetNumberClass(int value)
        {
            _numberClass = value;
        }

        public void SetClassifiers(double[,] value)
        {
            _classifiers = value;
        }

        public double[,] GetClassifiers()
        {
            return _classifiers;
        }

        public event EventHandler<string> EmergenceLog;
    }
}

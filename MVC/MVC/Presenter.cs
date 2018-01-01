using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileWorker.BL;
using Boosting.BL;
using System.Threading;


namespace CyberCortex
{
    public class Presenter
    {
        private readonly IFileManager _fileManager;
        private readonly IView _view;
        private readonly iAdaBoost _adaboost;
        private Thread[] _threadForPipe = new Thread[2];
        private Thread _threadForLearn;
        private Thread _threadForLoading;
        private Thread _threadForClassifiers;
        private bool _fetchFlag = true;
        Api api = new Api();

        public Presenter(IFileManager fileManager, IView view, iAdaBoost adaboost)
        {
            this._fileManager = fileManager;
            this._view = view;
            this._adaboost = adaboost;

            _view.FileLoadClick += _view_FileLoadClick;
            _view.StartLearnClick += _view_StartLearn;
            _view.SaveClassifiersClick += _view_SaveClassifiersClick;
            _view.LoadClassifiersClick += _view_LoadClassifiersClick;
            _view.StartConnectionClick += _view_StartConnectionClick;
            _adaboost.EmergenceLog += _adaboost_EmergenceLog;
        }

        #region Обработка нажатия кнопок в потоках
        void _view_LoadClassifiersClick(object sender, EventArgs e)
        {
            _threadForClassifiers = new Thread(LoadClassifiers);
            _threadForClassifiers.IsBackground = true;
            _threadForClassifiers.Start();
        }

        void _view_SaveClassifiersClick(object sender, EventArgs e)
        {
            _threadForClassifiers = new Thread(SaveClassifiers);
            _threadForClassifiers.IsBackground = true;
            _threadForClassifiers.Start();
        }

        void _view_FileLoadClick(object sender, EventArgs e)
        {
            _threadForLoading = new Thread(FileLoad);
            _threadForLoading.IsBackground = true;
            _threadForLoading.Start();
        }

        void _view_StartLearn(object sender, EventArgs e)
        {
            _threadForLearn = new Thread(LearningAlgorithm);
            _threadForLearn.IsBackground = true;
            _threadForLearn.Start();
        }

        private void _view_StartConnectionClick(object sender, EventArgs e)
        {
            if (_view.GetRunDetector())
            {
                _fetchFlag = true;
                GetAPIData();
                periodicFetchAPI();
            } 
            else
            {
                _fetchFlag = false;
            }
        }
        #endregion

        #region Методы
        private async Task periodicFetchAPI()
        {
            while (_fetchFlag)
            {
                GetAPIData();
                await Task.Delay(1000);
            }
        }

        void GetAPIData()
        {
            Console.WriteLine("rrr");
            string[] symbolList = new string[] { "BTC", "ETH", "LTC" };
            string[] timeframeList = new string[] { "histominute", "histohour", "histoday" };
            string[] exchangeList = new string[] { "Exmo", "CCCAGG", "Poloniex" };

            int symbolIndex = _view.GetSelectedSymbol();
            int timeframeIndex = _view.GetSelectedTimeframe();
            int exchangeIndex = _view.GetSelectedExchange();
  
            var result = api.GetInstrument(symbolList[symbolIndex], timeframeList[timeframeIndex], exchangeList[exchangeIndex], 100);

                int resultLength = result["Data"].Count;
                APIObject[] apiObjectList = new APIObject[resultLength];

                for (int i = 0; i < resultLength; i++)
                {
                    double open = Convert.ToDouble(result["Data"][i]["open"]);
                    double low = Convert.ToDouble(result["Data"][i]["low"]);
                    double high = Convert.ToDouble(result["Data"][i]["high"]);
                    double close = Convert.ToDouble(result["Data"][i]["close"]);
                    int volume = Convert.ToInt32(result["Data"][i]["volumeto"]);

                    apiObjectList[i] = new APIObject(open, low, high, close, volume);
                }
                //strategyConveerter

                //prediction
            


            Console.WriteLine("GetAPIData: {0}", apiObjectList[99].close);
        }

        void FileLoad()
        {
            ///////////////////////////////////////////
            _view.ActivationButtonSelectSample(false);
            _view.ActivationButtonSelectAnswer(false);
            _view.ActivationButtonSelectTestSample(false);
            _view.ActivationButtonSelectTestAnswer(false);
            _view.ActivationButtonLoad(false);
            _view.ActivationButtonLearn(false);
            _view.ActivationButtonLoadClassifiers(false);
            _view.ActivationButtonSaveClassifiers(false);
            _view.ActivationNumLengthSample(false);
            _view.ActivationNumSizeSample(false);
            _view.ActivationNumSizeTestSample(false);
            _view.ActivationNumClassCount(false);
            _view.ActivationNumClassifiers(false);
            /////////////////////////////////////////////

            _view.SetContent(new string('*', 90) + "\r\n");

            if (_view.GetSize() > 0 && _view.GetLength() > 0 && _view.GetTestSize() > 0)
            {
                _adaboost.SetLength(_view.GetLength());
                _adaboost.SetSize(_view.GetSize());
                _adaboost.SetTestSize(_view.GetTestSize());

                _adaboost.SetSample(new double[_view.GetSize(), _view.GetLength()]);
                _adaboost.SetAnswer(new double[_view.GetSize(), 1]);
                _adaboost.SetTestSample(new double[_view.GetTestSize(), _view.GetLength()]);
                _adaboost.SetTestAnswer(new double[_view.GetTestSize(), 1]);

                double[,] samples = _adaboost.GetSample();
                double[,] answers = _adaboost.GetAnswer();
                double[,] testSamples = _adaboost.GetTestSample();
                double[,] testAnswers = _adaboost.GetTestAnswer();
                bool successLoad = true;
                int countSuccess = 0;
                _view.SetContent("Начинаю загрузку файлов...\r\n");

                if (_fileManager.isExist(_view.GetPathToSamples()))
                {
                    successLoad = _fileManager.GetContent(_view.GetPathToSamples(), samples);
                    if (successLoad)
                    {
                        _view.SetContent("Загружен файл обучающей выборки.\r\n");
                        _adaboost.GetСharacteristicData(samples);//вычисление дисперсий и средних признаков
                        _adaboost.GetNormalData(samples);//нормировка
                        _adaboost.SetSample(samples);
                        _view.SetContent("Данные приведены к единичной дисперсии и нулевому мат. ожиданию.\r\n");
                        countSuccess++;
                    }
                    else
                    {
                        _view.SetContent("Не удалось загрузить файл обучающей выборки.\r\n");
                    }
                }

                if (_fileManager.isExist(_view.GetPathToAnswers()))
                {
                    successLoad = _fileManager.GetContent(_view.GetPathToAnswers(), answers);
                    if (successLoad)
                    {
                        _view.SetContent("Загружен файл обучающих ответов.\r\n");
                        _adaboost.SetAnswer(answers);
                        countSuccess++;
                    }
                    else
                    {
                        _view.SetContent("Не удалось загрузить файл обучающих ответов.\r\n");
                    }
                }

                if (_fileManager.isExist(_view.GetPathToTestSamples()))
                {
                    successLoad = _fileManager.GetContent(_view.GetPathToTestSamples(), testSamples);
                    if (successLoad)
                    {
                        _view.SetContent("Загружен файл тестовой выборки.\r\n");
                        _adaboost.GetNormalData(testSamples);//нормировка
                        _adaboost.SetTestSample(testSamples);
                        _view.SetContent("Данные приведены к единичной дисперсии и нулевому мат. ожиданию.\r\n");
                        countSuccess++;
                    }
                    else
                    {
                        _view.SetContent("Не удалось загрузить файл тестовой выборки.\r\n");
                    }
                }

                if (_fileManager.isExist(_view.GetPathToTestAnswers()))
                {
                    successLoad = _fileManager.GetContent(_view.GetPathToTestAnswers(), testAnswers);
                    if (successLoad)
                    {
                        _view.SetContent("Загружен файл тестовых ответов.\r\n");
                        _adaboost.SetTestAnswer(testAnswers);
                        countSuccess++;
                    }
                    else
                    {
                        _view.SetContent("Не удалось загрузить файл тестовых ответов.\r\n");
                    }
                }

                if (countSuccess == 4)
                {
                    _view.ActivationButtonLearn(true);
                    _view.ActivationButtonLoadClassifiers(true);
                }
            }
            else
            {
                _view.SetContent("Не заданы параметры, либо не выбраны файлы данных!\r\n");
            }
            ////////////////////////////////////////
            _view.ActivationButtonSelectSample(true);
            _view.ActivationButtonSelectAnswer(true);
            _view.ActivationButtonSelectTestSample(true);
            _view.ActivationButtonSelectTestAnswer(true);
            _view.ActivationButtonLoad(true);
            _view.ActivationNumLengthSample(true);
            _view.ActivationNumSizeSample(true);
            _view.ActivationNumSizeTestSample(true);
            _view.ActivationNumClassCount(true);
            _view.ActivationNumClassifiers(true);
            /////////////////////////////////////////

            //убийство потока, если поток жив
            if (_threadForLoading.IsAlive)
            {
                _threadForLoading.Abort();
            }
        }

        void LearningAlgorithm()
        {
            ///////////////////////////////////////////////
            _view.ActivationButtonSelectSample(false);
            _view.ActivationButtonSelectAnswer(false);
            _view.ActivationButtonSelectTestSample(false);
            _view.ActivationButtonSelectTestAnswer(false);
            _view.ActivationButtonLoad(false);
            _view.ActivationButtonLearn(false);
            _view.ActivationButtonLoadClassifiers(false);
            _view.ActivationButtonSaveClassifiers(false);
            _view.ActivationNumLengthSample(false);
            _view.ActivationNumSizeSample(false);
            _view.ActivationNumSizeTestSample(false);
            _view.ActivationNumClassCount(false);
            _view.ActivationNumClassifiers(false);
            ////////////////////////////////////////////////

            _view.SetContent(new string('*', 90) + "\r\n");

            if (_view.GetNumberClassifiers() > 0)
            {
                _view.SetContent("Инициализирую алгоритм обучения...\r\n\r\n");

                _adaboost.SetNumberClassifiers(_view.GetNumberClassifiers());
                _adaboost.SetNumberClass(_view.GetNumberClass());
                _adaboost.SetClassifiers(new double[_view.GetNumberClass() * _view.GetNumberClassifiers(), 4]);

                _view.SetContent(_adaboost.RunTrain());

                ////////////////////////////////////////////////
                _view.ActivationButtonLoad(true);
                _view.ActivationButtonLearn(true);
                _view.ActivationButtonSaveClassifiers(true);
                ////////////////////////////////////////////////          
            }
            else
            {
                _view.SetContent("Количество классификаторов не должно быть равно нулю!\r\n");
                _view.ActivationButtonLoadClassifiers(true);
            }

            //////////////////////////////////////////////// 
            _view.ActivationButtonSelectSample(true);
            _view.ActivationButtonSelectAnswer(true);
            _view.ActivationButtonSelectTestSample(true);
            _view.ActivationButtonSelectTestAnswer(true);
            _view.ActivationButtonLearn(true);
            _view.ActivationButtonLoad(true);
            _view.ActivationNumLengthSample(true);
            _view.ActivationNumSizeSample(true);
            _view.ActivationNumSizeTestSample(true);
            _view.ActivationNumClassCount(true);
            _view.ActivationNumClassifiers(true);
            //////////////////////////////////////////////// 

            //убийство потока, если поток жив
            if (_threadForLearn.IsAlive)
            {
                _threadForLearn.Abort();
            }
        }

        void SaveClassifiers()
        {
            ////////////////////////////////////////////
            _view.ActivationButtonSelectSample(false);
            _view.ActivationButtonSelectAnswer(false);
            _view.ActivationButtonSelectTestSample(false);
            _view.ActivationButtonSelectTestAnswer(false);
            _view.ActivationButtonLoad(false);
            _view.ActivationButtonLearn(false);
            _view.ActivationButtonLoadClassifiers(false);
            _view.ActivationNumLengthSample(false);
            _view.ActivationNumSizeSample(false);
            _view.ActivationNumSizeTestSample(false);
            _view.ActivationNumClassCount(false);
            _view.ActivationNumClassifiers(false);
            ////////////////////////////////////////////

            _view.SetContent(new string('*', 90) + "\r\n");
            string path = GetPathForClassifiers();
            bool success = true;
            _view.SetContent("Сохраняю классификаторы в файл...\r\n");
            success = _fileManager.SaveData(path, _adaboost.GetClassifiers());
            if (success)
            {
                _view.SetContent("Файл классификаторов сохранен по адресу:\r\n");
                _view.SetContent(path + "\r\n");
                _view.ActivationButtonSaveClassifiers(false);
            }
            else
            {
                _view.SetContent("Не удается сохранить классификаторы.\r\n");
            }

            ////////////////////////////////////////////
            _view.ActivationButtonSelectSample(true);
            _view.ActivationButtonSelectAnswer(true);
            _view.ActivationButtonSelectTestSample(true);
            _view.ActivationButtonSelectTestAnswer(true);
            _view.ActivationButtonLoad(true);
            _view.ActivationButtonLearn(true);
            _view.ActivationButtonLoadClassifiers(true);
            _view.ActivationNumLengthSample(true);
            _view.ActivationNumSizeSample(true);
            _view.ActivationNumSizeTestSample(true);
            _view.ActivationNumClassCount(true);
            _view.ActivationNumClassifiers(true);
            ////////////////////////////////////////////

            //убийство потока, если поток жив
            if (_threadForClassifiers.IsAlive)
            {
                _threadForClassifiers.Abort();
            }
        }

        void LoadClassifiers()
        {
            ////////////////////////////////////////////
            _view.ActivationButtonSelectSample(false);
            _view.ActivationButtonSelectAnswer(false);
            _view.ActivationButtonSelectTestSample(false);
            _view.ActivationButtonSelectTestAnswer(false);
            _view.ActivationButtonLoad(false);
            _view.ActivationButtonLearn(false);
            _view.ActivationButtonLoadClassifiers(false);
            _view.ActivationButtonSaveClassifiers(false);
            _view.ActivationNumLengthSample(false);
            _view.ActivationNumSizeSample(false);
            _view.ActivationNumSizeTestSample(false);
            _view.ActivationNumClassCount(false);
            _view.ActivationNumClassifiers(false);
            ////////////////////////////////////////////

            _view.SetContent(new string('*', 90) + "\r\n");

            if (_view.GetNumberClassifiers() > 0)
            {
                string path = GetPathForClassifiers();

                if (_fileManager.isExist(path))
                {
                    _view.SetContent("Загружаю файл классификаторов...\r\n");
                    _adaboost.SetNumberClass(_view.GetNumberClass());
                    _adaboost.SetNumberClassifiers(_view.GetNumberClassifiers());
                    _adaboost.SetClassifiers(new double[_view.GetNumberClass() * _view.GetNumberClassifiers(), 4]);
                    double[,] loadClassifiers = new double[_view.GetNumberClass() * _view.GetNumberClassifiers(), 4];
                    bool success = true;

                    success = _fileManager.GetContent(path, loadClassifiers);

                    if (success)
                    {
                        _view.SetContent("Загружен файл классификаторов.\r\n");
                        _adaboost.SetClassifiers(loadClassifiers);
                        _view.SetContent("Провожу тест классификаторов...\r\n");
                        _view.SetContent(_adaboost.GetTest());
                    }
                    else
                    {
                        _view.SetContent("Не удалось загрузить файл классификаторов.\r\n");
                    }
                }
                else
                {
                    _view.SetContent("Не найден файл классификаторов.\r\n");
                }
            }
            else
            {
                _view.SetContent("Количество классификаторов не должно быть равно нулю!\r\n");
            }

            ////////////////////////////////////////////
            _view.ActivationButtonSelectSample(true);
            _view.ActivationButtonSelectAnswer(true);
            _view.ActivationButtonSelectTestSample(true);
            _view.ActivationButtonSelectTestAnswer(true);
            _view.ActivationButtonLoad(true);
            _view.ActivationButtonLearn(true);
            _view.ActivationButtonLoadClassifiers(true);
            _view.ActivationNumLengthSample(true);
            _view.ActivationNumSizeSample(true);
            _view.ActivationNumSizeTestSample(true);
            _view.ActivationNumClassCount(true);
            _view.ActivationNumClassifiers(true);
            ////////////////////////////////////////////

            //убийство потока, если поток жив
            if (_threadForClassifiers.IsAlive)
            {
                _threadForClassifiers.Abort();
            }
        }

        string GetPathForClassifiers()
        {
            string path = _view.GetPathToSamples();
            string delimetr = "\\";
            string pathForClassifiers = null;

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == delimetr[0])
                {
                    for (int j = 0; j <= i; j++)
                    {
                        pathForClassifiers += path[j];
                    }
                    pathForClassifiers += "Классификаторы.csv";
                    break;
                }
            }
            return pathForClassifiers;
        }
        #endregion

        void _adaboost_EmergenceLog(object sender, string e)
        {
            if (_view.GetChekLogs())
            {
                _view.SetContent(e);
            }
        }
    }
}

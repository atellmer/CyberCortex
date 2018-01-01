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
        private Thread _threadForNotification;
        private int _countRecursion = 0;
        
        public Presenter(IFileManager fileManager, IView view, iAdaBoost adaboost)
        {
            this._fileManager = fileManager;
            this._view = view;
            this._adaboost = adaboost;

            _view.FileLoadClick += _view_FileLoadClick;
            _view.StartPipeServerClick += _view_StartPipeServer;
            _view.StartLearnClick += _view_StartLearn;
            _view.SaveClassifiersClick += _view_SaveClassifiersClick;
            _view.LoadClassifiersClick += _view_LoadClassifiersClick;
            _adaboost.EmergenceLog += _adaboost_EmergenceLog;
            Resolution();
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

        void _view_StartPipeServer(object sender, EventArgs e)
        {
            if (_view.GetRunDetector())
            {
                //////////////////////////////////////////
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
                _view.SetContent("Создаю соединение и ожидаю связи с клиентом...\r\n");
                _threadForPipe[0] = new Thread(StartPipeServer);
                _threadForPipe[0].IsBackground = true;
                _threadForPipe[0].Start();
            }
            else
            {
                StopPipeServer();
            }
        }
        #endregion

        #region Методы
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
            _view.ActivationButtonStart(false);
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
            _view.ActivationButtonStart(false);
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
                _view.ActivationButtonStart(true);
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

        void StartPipeServer()
        {
            double[] dataFromClient = new double[_view.GetLength()];
            double dataToClient = 0;

            for (int i = 0; i < dataFromClient.Length; i++)
            {
                dataFromClient[i] = PipeServer.StartServer(0);
                //_view.SetContent("Клиент: " + Convert.ToString(dataFromClient[i]) + "\r\n");
            }

            _adaboost.GetNormalForNewData(dataFromClient); //нормировка
            dataToClient = _adaboost.GetNewPredict(dataFromClient);

            PipeServer.StartServer(dataToClient);
            //_view.SetContent("Сервер: " + Convert.ToString(dataToClient) + "\r\n");
            PipeRecursion();
        }

        void PipeRecursion()
        {
            _countRecursion++;

            if (_countRecursion < 10)
            {
                PipeServer.StopServer();
                StartPipeServer();
            }
            else
            {
                if (_threadForPipe[0].IsAlive)
                {
                    _countRecursion = 0;
                    _threadForPipe[1] = new Thread(StartPipeServer);
                    _threadForPipe[1].IsBackground = true;
                    _threadForPipe[1].Start();
                    _threadForPipe[0].Abort();
                }
                if (_threadForPipe[1].IsAlive)
                {
                    _countRecursion = 0;
                    _threadForPipe[0] = new Thread(StartPipeServer);
                    _threadForPipe[0].IsBackground = true;
                    _threadForPipe[0].Start();
                    _threadForPipe[1].Abort();
                }
            }
        }

        void StopPipeServer()
        {
            _threadForNotification = new Thread(SendNotificationToClientAboutCloseServer);
            _threadForNotification.IsBackground = true;
            _threadForNotification.Start();

            Thread.Sleep(10);
            PipeServer.StopServer();
            _view.SetContent("Cоединение закрыто.\r\n");

            //////////////////////////////////////////
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
            //////////////////////////////////////////

            //убийство потока, если поток жив
            if (_threadForPipe[0] != null)
            {
                if (_threadForPipe[0].IsAlive)
                {
                    _threadForPipe[0].Abort();
                }
            }
            if (_threadForPipe[1] != null)
            {
                if (_threadForPipe[1].IsAlive)
                {
                    _threadForPipe[1].Abort();
                }
            }

            if (_threadForNotification.IsAlive)
            {
                _threadForNotification.Abort();
            }
        }

        void SendNotificationToClientAboutCloseServer()
        {
            PipeServer.StartServer(-12345);
        }

        void Resolution()
        {
            if (!Searcher.GetResolution())
            {
                _view.ActivationButtonStart(false);
                _view.ActivationButtonLearn(false);
                _view.ActivationButtonLoad(false);
                _view.ActivationButtonSelectSample(false);
                _view.ActivationButtonSelectAnswer(false);
                _view.ActivationButtonSelectTestSample(false);
                _view.ActivationButtonSelectTestAnswer(false);
                _view.ActivationButClean(false);
                _view.ActivationNumLengthSample(false);
                _view.ActivationNumSizeSample(false);
                _view.ActivationNumSizeTestSample(false);
                _view.ActivationNumClassCount(false);
                _view.ActivationNumClassifiers(false);
                _view.ActivationCheckLogs(false);
                _view.ActivationFieldToPathSamples(false);
                _view.ActivationFieldToPathAnswers(false);
                _view.ActivationFieldToPathTestSamples(false);
                _view.ActivationFieldToPathTestAnswers(false);
                _view.SetContent("Внимание! Данная программа не зарегестрирована.\r\n");
                _view.SetContent("Для того, чтобы зарегестрировать программу, свяжитесь с разработчиком.\r\n");
                _view.SetContent("Электронная почта: cybercortex.store@gmail.com\r\n");
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
            _view.ActivationButtonStart(false);
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
            _view.ActivationButtonStart(true);
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
            _view.ActivationButtonStart(false);
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
                        //////////////////////////////////
                        _view.ActivationButtonStart(true);
                        //////////////////////////////////
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace CyberCortex
{
    public interface IView
    {
        void SetContent(string value);
        string GetPathToSamples();
        string GetPathToAnswers();
        string GetPathToTestSamples();
        string GetPathToTestAnswers();
        int GetLength();
        int GetSize();
        int GetTestSize();
        int GetNumberClassifiers();
        int GetNumberClass();
        void ActivationButtonLearn(bool activation);
        void ActivationButtonLoad(bool activation);
        void ActivationButtonSelectSample(bool activation);
        void ActivationButtonSelectAnswer(bool activation);
        void ActivationButtonSelectTestSample(bool activation);
        void ActivationButtonSelectTestAnswer(bool activation);
        void ActivationButtonSaveClassifiers(bool activation);
        void ActivationButtonLoadClassifiers(bool activation);
        void ActivationNumLengthSample(bool activation);
        void ActivationNumSizeSample(bool activation);
        void ActivationNumSizeTestSample(bool activation);
        void ActivationNumClassCount(bool activation);
        void ActivationNumClassifiers(bool activation);
        void ActivationButClean(bool activation);
        void ActivationCheckLogs(bool activation);
        void ActivationFieldToPathSamples(bool activation);
        void ActivationFieldToPathAnswers(bool activation);
        void ActivationFieldToPathTestSamples(bool activation);
        void ActivationFieldToPathTestAnswers(bool activation);
        bool GetRunDetector();
        bool GetChekLogs();
        int GetSelectedExchange();
        int GetSelectedSymbol();
        int GetSelectedTimeframe();
        void setChartData(double[,] data);

        event EventHandler FileLoadClick;
        event EventHandler StartLearnClick;
        event EventHandler SaveClassifiersClick;
        event EventHandler LoadClassifiersClick;
        event EventHandler StartConnectionClick;
    }

    public partial class View : Form, IView
    {
        private bool _runDetector = false;

        public View()
        {
            InitializeComponent();

            butSelectSamples.Click += butSelectSamples_Click;
            butSelectAnswers.Click += butSelectAnswers_Click;
            butSelectTestSamples.Click += butSelectTestSamples_Click;
            butSelectTestAnswers.Click += butSelectTestAnswers_Click;
            butLoad.Click += butLoad_Click;
            butClean.Click += butClean_Click;
            butLearn.Click += butLearn_Click;
            butSaveClassifiers.Click += butSaveClassifiers_Click;
            butLoadClassifiers.Click += butLoadClassifiers_Click;

            exchange.SelectedIndex = 0;
            symbol.SelectedIndex = 0;
            timeframe.SelectedIndex = 1;
        }

        public void setChartData(double[,] data)
        {
            ChartValues<OhlcPoint> chartValues = new ChartValues<OhlcPoint>();
            
            for (int i = 0; i < data.GetLength(0); i++)
            {
                chartValues.Add(new OhlcPoint(data[i, 1], data[i, 3], data[i, 2], data[i, 4]));
            }

            cartesianChart1.Series = new SeriesCollection
            {
                new OhlcSeries
                {
                    Values = chartValues
                }
            };
        }

        void butLoadClassifiers_Click(object sender, EventArgs e)
        {
            if (LoadClassifiersClick != null)
            {
                LoadClassifiersClick(this, e);
            }
        }

        void butSaveClassifiers_Click(object sender, EventArgs e)
        {
            if (SaveClassifiersClick != null)
            {
                SaveClassifiersClick(this, e);
            }
        }

        void butLearn_Click(object sender, EventArgs e)
        {
            if (StartLearnClick != null)
            {
                StartLearnClick(this, e);
            }
        }

        void butSelectTestAnswers_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы CSV|*.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fieldToPathTestAnswers.Text = dialog.FileName;
            }
        }

        void butSelectTestSamples_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы CSV|*.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fieldToPathTestSamples.Text = dialog.FileName;
            }
        }

        void butSelectAnswers_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы CSV|*.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fieldToPathAnswers.Text = dialog.FileName;
            }
        }

        void butSelectSamples_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы CSV|*.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fieldToPathSamples.Text = dialog.FileName;
            }
        }

        void butLoad_Click(object sender, EventArgs e)
        {
            if (FileLoadClick != null)
            {
                FileLoadClick(this, e);
            }
        }

        void butClean_Click(object sender, EventArgs e)
        {
            fieldForContent.Text = null;
        }

        public void SetContent(string value)
        {
            if (fieldForContent.InvokeRequired)
            {
                fieldForContent.Invoke(new Action<string>((s) => fieldForContent.Text += s), value);
                fieldForContent.Invoke(new Action(() => fieldForContent.SelectionStart = fieldForContent.Text.Length));
                fieldForContent.Invoke(new Action(() => fieldForContent.ScrollToCaret()));
            }
            else
            {
                fieldForContent.Text += value;
                fieldForContent.SelectionStart = fieldForContent.Text.Length;
                fieldForContent.ScrollToCaret();
            }
        }

        public string GetPathToSamples()
        {
            return fieldToPathSamples.Text;
        }

        public string GetPathToAnswers()
        {
            return fieldToPathAnswers.Text;
        }

        public string GetPathToTestSamples()
        {
            return fieldToPathTestSamples.Text;
        }

        public string GetPathToTestAnswers()
        {
            return fieldToPathTestAnswers.Text;
        }

        public int GetLength()
        {
            return Convert.ToInt32(numLengthSample.Value);
        }

        public int GetSize()
        {
            return Convert.ToInt32(numSizeSample.Value);
        }

        public int GetTestSize()
        {
            return Convert.ToInt32(numSizeTestSample.Value);
        }

        public int GetNumberClassifiers()
        {
            return Convert.ToInt32(numClassifiers.Value);
        }

        public int GetNumberClass()
        {
            return Convert.ToInt32(numClassCount.Value);
        }

        public void ActivationButtonLearn(bool activation)
        {
            if (InvokeRequired)
            {
                butLearn.Invoke(new Action(() => butLearn.Enabled = activation));
            }
            else
            {
                butLearn.Enabled = activation;
            }
        }

        public void ActivationButtonLoad(bool activation)
        {
            if (InvokeRequired)
            {
                butLoad.Invoke(new Action(() => butLoad.Enabled = activation));
            }
            else
            {
                butLoad.Enabled = activation;
            }
        }

        public void ActivationButtonSelectSample(bool activation)
        {
            if (InvokeRequired)
            {
                butSelectSamples.Invoke(new Action(() => butSelectSamples.Enabled = activation));
            }
            else
            {
                butSelectSamples.Enabled = activation;
            }
        }

        public void ActivationButtonSelectAnswer(bool activation)
        {
            if (InvokeRequired)
            {
                butSelectAnswers.Invoke(new Action(() => butSelectAnswers.Enabled = activation));
            }
            else
            {
                butSelectAnswers.Enabled = activation;
            }
        }

        public void ActivationButtonSelectTestSample(bool activation)
        {
            if (InvokeRequired)
            {
                butSelectTestSamples.Invoke(new Action(() => butSelectTestSamples.Enabled = activation));
            }
            else
            {
                butSelectTestSamples.Enabled = activation;
            }
        }

        public void ActivationButtonSelectTestAnswer(bool activation)
        {
            if (InvokeRequired)
            {
                butSelectTestAnswers.Invoke(new Action(() => butSelectTestAnswers.Enabled = activation));
            }
            else
            {
                butSelectTestAnswers.Enabled = activation;
            }
        }

        public void ActivationButtonSaveClassifiers(bool activation)
        {
            if (InvokeRequired)
            {
                butSaveClassifiers.Invoke(new Action(() => butSaveClassifiers.Enabled = activation));
            }
            else
            {
                butSaveClassifiers.Enabled = activation;
            }
        }

        public void ActivationButtonLoadClassifiers(bool activation)
        {
            if (InvokeRequired)
            {
                butLoadClassifiers.Invoke(new Action(() => butLoadClassifiers.Enabled = activation));
            }
            else
            {
                butLoadClassifiers.Enabled = activation;
            }
        }

        public void ActivationNumLengthSample(bool activation)
        {
            if (InvokeRequired)
            {
                numLengthSample.Invoke(new Action(() => numLengthSample.Enabled = activation));
            }
            else
            {
                numLengthSample.Enabled = activation;
            }
        }

        public void ActivationNumSizeSample(bool activation)
        {
            if (InvokeRequired)
            {
                numSizeSample.Invoke(new Action(() => numSizeSample.Enabled = activation));
            }
            else
            {
                numSizeSample.Enabled = activation;
            }
        }

        public void ActivationNumSizeTestSample(bool activation)
        {
            if (InvokeRequired)
            {
                numSizeTestSample.Invoke(new Action(() => numSizeTestSample.Enabled = activation));
            }
            else
            {
                numSizeTestSample.Enabled = activation;
            }
        }

        public void ActivationNumClassCount(bool activation)
        {
            if (InvokeRequired)
            {
                numClassCount.Invoke(new Action(() => numClassCount.Enabled = activation));
            }
            else
            {
                numClassCount.Enabled = activation;
            }
        }

        public void ActivationNumClassifiers(bool activation)
        {
            if (InvokeRequired)
            {
                numClassifiers.Invoke(new Action(() => numClassifiers.Enabled = activation));
            }
            else
            {
                numClassifiers.Enabled = activation;
            }
        }

        public void ActivationButClean(bool activation)
        {
            if (InvokeRequired)
            {
                butClean.Invoke(new Action(() => butClean.Enabled = activation));
            }
            else
            {
                butClean.Enabled = activation;
            }
        }

        public void ActivationCheckLogs(bool activation)
        {
            if (InvokeRequired)
            {
                checkLogs.Invoke(new Action(() => checkLogs.Enabled = activation));
            }
            else
            {
                checkLogs.Enabled = activation;
            }
        }

        public void ActivationFieldToPathSamples(bool activation)
        {
            if (InvokeRequired)
            {
                fieldToPathSamples.Invoke(new Action(() => fieldToPathSamples.Enabled = activation));
            }
            else
            {
                fieldToPathSamples.Enabled = activation;
            }
        }

        public void ActivationFieldToPathAnswers(bool activation)
        {
            if (InvokeRequired)
            {
                fieldToPathAnswers.Invoke(new Action(() => fieldToPathAnswers.Enabled = activation));
            }
            else
            {
                fieldToPathAnswers.Enabled = activation;
            }
        }

        public void ActivationFieldToPathTestSamples(bool activation)
        {
            if (InvokeRequired)
            {
                fieldToPathTestSamples.Invoke(new Action(() => fieldToPathTestSamples.Enabled = activation));
            }
            else
            {
                fieldToPathTestSamples.Enabled = activation;
            }
        }

        public void ActivationFieldToPathTestAnswers(bool activation)
        {
            if (InvokeRequired)
            {
                fieldToPathTestAnswers.Invoke(new Action(() => fieldToPathTestAnswers.Enabled = activation));
            }
            else
            {
                fieldToPathTestAnswers.Enabled = activation;
            }
        }

        public bool GetRunDetector()
        {
            return _runDetector;
        }

        public bool GetChekLogs()
        {
            return checkLogs.Checked;
        }

        public int GetSelectedExchange()
        {
            return exchange.SelectedIndex;
        }

        public int GetSelectedSymbol()
        {
            return symbol.SelectedIndex;
        }

        public int GetSelectedTimeframe()
        {
            return timeframe.SelectedIndex;
        }

        private void timeframe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void exchange_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void symbol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void startBtn_Click(object sender, EventArgs e)
        {
            if (_runDetector == false)
            {
                startBtn.Text = "Стоп";
                _runDetector = true;
            }
            else
            {
                startBtn.Text = "Старт";
                _runDetector = false;
            }
      
            if (StartConnectionClick != null)
            {
                StartConnectionClick(this, e);
            }
        }

        public event EventHandler FileLoadClick;
        public event EventHandler StartLearnClick;
        public event EventHandler SaveClassifiersClick;
        public event EventHandler LoadClassifiersClick;
        public event EventHandler StartConnectionClick;
    }
}

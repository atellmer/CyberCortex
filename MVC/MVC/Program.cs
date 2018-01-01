using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileWorker.BL;
using Boosting.BL;

namespace CyberCortex
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FileManager fileManager = new FileManager();
            View view = new View();
            AdaBoost adaboost = new AdaBoost();

            Presenter presenter = new Presenter(fileManager, view, adaboost);

            Application.Run(view);
        }
    }
}

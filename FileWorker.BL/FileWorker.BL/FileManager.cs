using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace FileWorker.BL
{
    public interface IFileManager
    {
        bool GetContent(string filePath, double[,] data);
        bool SaveData(string filePath, double[,] data);
        bool isExist(string filePath);
    }

    public class FileManager : IFileManager
    {
        public bool isExist(string filePath)
        {
            bool isExist = File.Exists(filePath);
            return isExist;
        }

        public bool GetContent(string filePath, double[,] data)
        {
            bool success = true;
            try
            {
                string[] textContent = File.ReadAllLines(filePath);
                string[] splitContent = new string[data.GetLength(1)];
                string[,] bufferContent = new string[data.GetLength(0), data.GetLength(1)];
                char[] Delimetr = ";".ToCharArray();

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    splitContent = textContent[i].Split(Delimetr);
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        bufferContent[i, j] = splitContent[j];
                    }
                }

                Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        if (bufferContent[i, j] != null)
                        {
                            data[i, j] = Convert.ToDouble(bufferContent[i, j].Replace('.', separator));
                        }
                    }
                }
            }
            catch
            {
                string message = "Программа не может прочитать файл!\r\nВозможно файл уже открыт в другой программе, либо формат данных не совпадает.";
                string caption = "Ошибка!";
                MessageBox.Show(message, caption);
                success = false;
            }

            return success;
        }

        public bool SaveData(string filePath, double[,] data)
        {
            bool success = true;
            List<string> textData = new List<string>();
            string buffer = null;

            try
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        buffer += data[i, j];
                        if (j != data.GetLength(1) - 1)
                        {
                            buffer += ";";
                        }
                    }
                    textData.Add(buffer);
                    buffer = null;
                }

                File.WriteAllLines(filePath, textData);
            }
            catch
            {
                string message = "Программа не может сохранить файл!";
                string caption = "Ошибка!";
                MessageBox.Show(message, caption);
                success = false;
            }

            return success;
        }
    }
}

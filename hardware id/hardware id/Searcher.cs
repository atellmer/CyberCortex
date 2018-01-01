using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Generator
{
    public static class Searcher
    {
        private static ManagementObjectSearcher searcher;

        public static string GetHardware()
        {
            string data = null;
            searcher = new ManagementObjectSearcher("SELECT * FROM CIM_Card");

            foreach (ManagementObject mb in searcher.Get())
            {
                data = Convert.ToString(mb["SerialNumber"]);
            }
            return data;
        }
    }
}

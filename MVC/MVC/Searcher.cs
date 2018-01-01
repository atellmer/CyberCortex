using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace CyberCortex
{
    public static class Searcher
    {
        private static ManagementObjectSearcher _searcher;

        public static bool GetResolution()
        {
            _searcher = new ManagementObjectSearcher("SELECT * FROM CIM_Card");

            string internalValue = "130713618500469";
            string currentValue = null;

            foreach (ManagementObject mb in _searcher.Get())
            {
                currentValue = Convert.ToString(mb["SerialNumber"]);
            }
            if (internalValue == currentValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

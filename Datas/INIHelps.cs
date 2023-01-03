using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DeviceDatas
{
    public  class INIHelps
    {
        public string inipath;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIHelps(string INIPath)
        {
            this.inipath = INIPath;
        }

        public bool WriteIniData(string section, string key, string value)
        {
            bool result;
            try
            {
                INIHelps.WritePrivateProfileString(section, key, value, this.inipath);
            }
            catch (Exception var_0_14)
            {
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        public string ReadIniData(string section, string key)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            string result;
            try
            {
                INIHelps.GetPrivateProfileString(section, key, "", stringBuilder, 255, this.inipath);
            }
            catch (Exception var_1_29)
            {
                result = "";
                return result;
            }
            result = stringBuilder.ToString();
            if (result == "")
            {
                result = "0";
            }
            return result;
        }
    }
}

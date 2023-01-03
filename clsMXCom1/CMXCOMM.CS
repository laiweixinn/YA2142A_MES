using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace clsMXCom
{
    public class CMXComm
    {
        private AxActUtlTypeLib.AxActUtlType axActUtlType1;

        static CMXComm mr;
        public static CMXComm GetInstance()
        {
            if (mr == null)
            {
                mr = new CMXComm();
            }
            return mr;
        }

        public CMXComm()
        {
            axActUtlType1 = new AxActUtlTypeLib.AxActUtlType();
            axActUtlType1.ActLogicalStationNumber = 0;
        }

        static private bool isconnected = false;
        static public bool IsConnected
        {
            get
            {
                return isconnected;
            }
        }
        public bool Open(int logstnum, string pwd)
        {
            Dispose();
            axActUtlType1.ActLogicalStationNumber = logstnum;
            axActUtlType1.ActPassword = pwd;
            if (axActUtlType1.Open() == 0)
            {
                isconnected = true;
            }
            return isconnected;
        }

        ~CMXComm()
        {
            Dispose();
        }

        public void Dispose()
        {
            //if (isconnected)
            {
                axActUtlType1.ActLogicalStationNumber = 0;
                axActUtlType1.Close();
                isconnected = false;
            }
        }

        public bool Read(string szDeviceName, int iNumberOfData, out short[] arrDeviceValue)
        {
            arrDeviceValue = new short[iNumberOfData];
            int iReturnCode = axActUtlType1.ReadDeviceRandom2(szDeviceName,
                                                    iNumberOfData,
                                                    out arrDeviceValue[0]);
            iReturnCode = axActUtlType1.ReadDeviceBlock2(szDeviceName,
                                                  iNumberOfData,
                                                  out arrDeviceValue[0]);
            return iReturnCode == 0;
        }

        public bool Write(string szDeviceName, int iNumberOfData, short[] arrDeviceValue)
        {
            int iReturnCode = axActUtlType1.WriteDeviceRandom2(szDeviceName,
                                                  iNumberOfData,
                                                  ref arrDeviceValue[0]);
            return iReturnCode == 0;
        }
    }
}

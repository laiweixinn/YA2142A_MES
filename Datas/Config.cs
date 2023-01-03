using DeviceManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DeviceDatas
{
    public class CIMConfig
    {
        private INIHelps INI;
        public string ServerIP;
        public string Station;
        public string UserId;
        public string QueueName;
        public string QueueDesc;
        public string QueueDescAuto;
        public int Port;
        public int Timeout;
        public int MaxSize;
        public int MaxCount;
        public int AlarmCount;
        public int AlarmSize;
        public int QueueType;
        public bool QueueDescDisabled;
        public string CorrelationID;
        public bool AutoStart;
        public bool BubbleFrm;
        /// <summary>
        /// 最小化等待收到NromalTest后再显示窗口
        /// </summary>
        public bool CallUp;
        public int FileSaveDays;

        public bool TestMode;

        public static readonly string path = Application.StartupPath + "\\config.ini";

        static CIMConfig _Instance;


        private CIMConfig()
        {
            INI = new INIHelps(path);
        }

        static object obj = new object();
        public static CIMConfig Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (obj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new CIMConfig();
                        }
                    }
                }

                return _Instance;
            }

        }

        public bool LoadConfig()
        {
            try
            {
                ServerIP = INI.ReadIniData("CIM", "ServerIP");
                Station = INI.ReadIniData("CIM", "Station");
                UserId = INI.ReadIniData("CIM", "UserId");
                QueueName = INI.ReadIniData("CIM", "QueueName");
                QueueDesc = INI.ReadIniData("CIM", "QueueDesc");
                QueueDescAuto = INI.ReadIniData("CIM", "QueueDescAuto");
                int.TryParse(INI.ReadIniData("CIM", "Port"), out Port);
                int.TryParse(INI.ReadIniData("CIM", "Timeout"), out Timeout);
                int.TryParse(INI.ReadIniData("CIM", "MaxSize"), out MaxSize);
                int.TryParse(INI.ReadIniData("CIM", "MaxCount"), out MaxCount);
                int.TryParse(INI.ReadIniData("CIM", "AlarmCount"), out AlarmCount);
                int.TryParse(INI.ReadIniData("CIM", "AlarmSize"), out AlarmSize);
                int.TryParse(INI.ReadIniData("CIM", "QueueType"), out QueueType);
                bool.TryParse(INI.ReadIniData("CIM", "QueueDescDisabled"), out QueueDescDisabled);
                CorrelationID = INI.ReadIniData("CIM", "CorrelationID");
                bool.TryParse(INI.ReadIniData("CIM", "AutoStart"), out AutoStart);
                bool.TryParse(INI.ReadIniData("CIM", "BubbleFrm"), out BubbleFrm);
                bool.TryParse(INI.ReadIniData("CIM", "CallUp"), out CallUp);
                int.TryParse(INI.ReadIniData("CIM", "FileSaveDays"), out FileSaveDays);


                return true;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:LoadConfig_" + ex.ToString(), true);
                return false;
            }
        }

        public bool SaveConfig()
        {
            try
            {

                INI.WriteIniData("CIM", "ServerIP", ServerIP);
                INI.WriteIniData("CIM", "Station", Station);
                INI.WriteIniData("CIM", "UserId", UserId);
                INI.WriteIniData("CIM", "QueueName", QueueName);
                INI.WriteIniData("CIM", "QueueDesc", QueueDesc);
                INI.WriteIniData("CIM", "QueueDescAuto", QueueDescAuto);
                INI.WriteIniData("CIM", "Port", Port.ToString());
                INI.WriteIniData("CIM", "Timeout", Timeout.ToString());
                INI.WriteIniData("CIM", "MaxSize", MaxSize.ToString());
                INI.WriteIniData("CIM", "MaxCount", MaxCount.ToString());
                INI.WriteIniData("CIM", "AlarmCount", AlarmCount.ToString());
                INI.WriteIniData("CIM", "AlarmSize", AlarmSize.ToString());
                INI.WriteIniData("CIM", "QueueType", QueueType.ToString());
                INI.WriteIniData("CIM", "QueueDescDisabled", QueueDescDisabled.ToString());
                INI.WriteIniData("CIM", "CorrelationID", CorrelationID);


                INI.WriteIniData("CIM", "AutoStart", AutoStart.ToString());
                INI.WriteIniData("CIM", "BubbleFrm", BubbleFrm.ToString());
                INI.WriteIniData("CIM", "CallUp", CallUp.ToString());
                INI.WriteIniData("CIM", "FileSaveDays", FileSaveDays.ToString());

                return true;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:SaveConfig_" + ex.ToString(), true);
                return false;
            }
        }

    }

    public delegate void CIMDataChangeDel();
    public class CIMProcessData
    {
        public static CIMDataChangeDel OnCIMDataChange;
        public static readonly string path = Application.StartupPath + "\\Data";
        private DataSet _ds;
        static CIMProcessData _Instance;
        private CIMProcessData()
        {

        }
        public static CIMProcessData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CIMProcessData();
                }
                return _Instance;
            }

        }



        public void AddData(string sn, string ValidateLotResult, string TrackOutResult, string AOIRes, string info)
        {
            try
            {
                if (_ds == null || _ds.Tables.Count == 0 || _ds.Tables[0].TableName != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    Load();
                }
                DataRow dr = _ds.Tables[0].NewRow();
                dr["Index"] = _ds.Tables[0].Rows.Count + 1;
                dr["SN"] = sn;
                dr["DateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dr["ValidateLot"] = ValidateLotResult;
                dr["TrackOut"] = TrackOutResult;
                dr["AOI"] = AOIRes;
                dr["Info"] = info;

                _ds.Tables[0].Rows.Add(dr);

                SaveAnsy();

                OnCIMData();
            }
            catch (Exception ex)
            {

                MachineContext.Print("Error:AddData_" + ex.ToString(), true);
            }
        }

        private void OnCIMData()
        {
            if (OnCIMDataChange != null)
            {
                OnCIMDataChange();
            }
        }




        public DataSet Load()
        {
            try
            {


                DataSet ds = new DataSet();
                string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
                string fullname = Path.Combine(path, filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(fullname))
                {
                    ds.ReadXml(fullname);
                }

                if (ds == null || ds.Tables.Count == 0)
                {
                    ds = new DataSet();
                    DataTable dt = new DataTable();
                    dt.TableName = DateTime.Now.ToString("yyyy-MM-dd");
                    DataColumn dc = new DataColumn("Index", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("SN", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("DateTime", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("ValidateLot", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("TrackOut", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("AOI", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("Info", typeof(string));
                    dt.Columns.Add(dc);
                    ds.Tables.Add(dt);
                }
                _ds = ds;
                return _ds;
            }
            catch (Exception ex)
            {

                MachineContext.Print("Error:Load_" + ex.ToString(), true);
                return null;
            }
        }

        public DataTable Load(DateTime time)
        {
            try
            {
                DataSet ds = new DataSet();
                string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
                string fullname = Path.Combine(path, filename);
                ds.ReadXml(fullname);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:Load_" + ex.ToString(), true);
                return null;
            }
        }

        public DataTable GetData()
        {

            if (_ds.Tables.Count > 0)
            {
                return _ds.Tables[0];
            }
            return null;
        }

        object obj = new object();
        private void SaveAnsy()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    lock (obj)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
                        string fullname = Path.Combine(path, filename);
                        _ds.WriteXml(fullname);
                    }
                }
                catch (Exception ex)
                {
                    MachineContext.Print("Error:SaveAnsy_" + ex.ToString(), true);
                }

            });

        }
    }
}

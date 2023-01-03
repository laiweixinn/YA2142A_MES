
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.Services.Description;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeviceManager
{
    public class Config
    {
        private INIHelps INI;
        public string ServerIP;
        public string ClassName;
        public string MethodName;
        public string Port;
        public string  ProjectName;
        public string DeviceName;//设备类型
        public string DeviceCode;//设备代码

        public double BendingMaxSize;
        public double BendingMinSize;

        public string QueueName;
        public string QueueDesc;


        public bool AutoStart;
        public bool BubbleFrm;
        /// <summary>
        /// 最小化等待收到NromalTest后再显示窗口
        /// </summary>
        public bool CallUp;
        public int FileSaveDays;

        public bool TestMode;

        public static readonly string path = Application.StartupPath + "\\config.ini";

        static Config _Instance;


        private Config()
        {
            INI = new INIHelps(path);
        }

        static object obj = new object();
        public static Config Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (obj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Config();
                        }
                    }
                }

                return _Instance;
            }

        }

        public CompilerResults LoadSteam() {
            WebClient web = new WebClient();
            using (Stream stream = web.OpenRead($"{ INI.ReadIniData("CIM", "ServerIP") }?WSDL"))
            {
                CodeNamespace nmspace = new CodeNamespace();
                CodeCompileUnit unit = new CodeCompileUnit();
                ServiceDescription description = ServiceDescription.Read(stream);
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter()
                {
                    ProtocolName = "Soap",
                    Style = ServiceDescriptionImportStyle.Client,
                    CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync
                };
                importer.AddServiceDescription(description, null, null);
                unit.Namespaces.Add(nmspace);
                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters parameter = new CompilerParameters()
                {
                    GenerateInMemory = true
                };
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
                stream.Close();
                return result;
            }
            
        }

        public bool Load()
        {
            try
            {
                ServerIP = INI.ReadIniData("CIM", "ServerIP");
                ClassName = INI.ReadIniData("CIM", "ClassName");
                MethodName = INI.ReadIniData("CIM", "MethodName");
                Port = INI.ReadIniData("CIM", "Port");
                ProjectName = INI.ReadIniData("CIM", "ProjectName");
                DeviceName = INI.ReadIniData("CIM", "DeviceName");
                DeviceCode= INI.ReadIniData("CIM", "DeviceCode");


                double.TryParse(INI.ReadIniData("CIM", "BendingMaxSize"), out BendingMaxSize);
                double.TryParse(INI.ReadIniData("CIM", "BendingMinSize"), out BendingMinSize);

                bool.TryParse(INI.ReadIniData("CIM", "AutoStart"), out AutoStart);
                bool.TryParse(INI.ReadIniData("CIM", "BubbleFrm"), out BubbleFrm);
                bool.TryParse(INI.ReadIniData("CIM", "CallUp"), out CallUp);
                int.TryParse(INI.ReadIniData("CIM", "FileSaveDays"), out FileSaveDays);


                //加载webservice流



                return true;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:LoadConfig_" + ex.ToString(), true);
                return false;
            }
        }

        public bool Save()
        {
            try
            {
             

                INI.WriteIniData("CIM", "ServerIP", ServerIP);
                INI.WriteIniData("CIM", "ClassName", ClassName);
                INI.WriteIniData("CIM", "MethodName", MethodName);
                INI.WriteIniData("CIM", "Port", Port);
                INI.WriteIniData("CIM", "ProjectName", ProjectName);
                INI.WriteIniData("CIM", "DeviceName", DeviceName);
                INI.WriteIniData("CIM", "DeviceCode", DeviceCode);



                INI.WriteIniData("CIM", "BendingMaxSize", BendingMaxSize.ToString());
                INI.WriteIniData("CIM", "BendingMinSize", BendingMinSize.ToString());
                

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

    public delegate void MesDataChangeDel();
    public class MesData
    {
        public static MesDataChangeDel OnMesDataChange;
        public static readonly string path = Application.StartupPath + "\\Data";
        private DataSet _ds;
        static MesData _Instance;
        private MesData()
        {

        }
        public static MesData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MesData();
                }
                return _Instance;
            }
        }


        /// <summary>
        /// 添加一整条数据
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="ValidateLotResult"></param>
        /// <param name="TrackOutResult"></param>
        /// <param name="AOIRes"></param>
        /// <param name="info"></param>

        public void Add(string strInfo)
        {
            try
            {
                if (_ds == null || _ds.Tables.Count == 0 || _ds.Tables[0].TableName != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    Load();
                }
                string[] strArray = strInfo.Split(',');
                DataRow dr = _ds.Tables[0].NewRow();
                

                dr["Index"] = _ds.Tables[0].Rows.Count + 1;
                dr["扫码时间"] = strArray[0];
                dr["下料时间"] = strArray[1];
                dr["撕膜检测结果"] = strArray[2];
                dr["弯折BZ轴参数"] = strArray[3];
                dr["弯折BR轴参数"] = strArray[4];
                dr["保压温度"] = strArray[5];
                dr["保压压力"] = strArray[6];
                dr["保压时间"] = strArray[7];
                dr["AOI检测结果"] = strArray[8];
                dr["检测精度A"] = strArray[9];
                dr["检测精度B"] = strArray[10];
                dr["制程二维码"] = strArray[11];
                dr["刷卡ID"] = strArray[12];
                dr["端口"] = strArray[13];
                dr["设备代码"] = strArray[14];
            
                dr["设备类型"] = strArray[15];

                _ds.Tables[0].Rows.Add(dr);

                SaveAnsy();

                OnDataChange();
            }
            catch (Exception ex)
            {

                MachineContext.Print("Error:AddData_" + ex.ToString(), true);
            }
        }
        
        public void RefreshData() {
     
            OnDataChange();
        }

        private void OnDataChange()
        {
            if (OnMesDataChange != null)
            {
                OnMesDataChange();
            }
        }



        /// <summary>
        /// 加载数据 如果数据为空 则新建表单
        /// </summary>
        /// <returns></returns>
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
                    dc = new DataColumn("扫码时间", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("下料时间", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("撕膜检测结果", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("弯折BZ轴参数", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("弯折BR轴参数", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("保压温度", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("保压压力", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("保压时间", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("AOI检测结果", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("检测精度A", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("检测精度B", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("制程二维码", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("刷卡ID", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("端口", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("设备代码", typeof(string));
                    dt.Columns.Add(dc);
               
                    dc = new DataColumn("设备类型", typeof(string));
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


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceManager
{

    public delegate void ShowMessge(string msg, bool alarm);
    public delegate void ShowTipsDel(TipType type, string msg);
    public delegate void ShowMesInfoDel(string type, string val);
    public enum TipType
    {
        Error,
        Info,
        Success,
        Warning

    }


    public static class MachineContext
    {
        //------------[事件]------------
        public static ShowMessge OnShowMessge;
        public static ShowTipsDel OnShowTips;
        public static ShowMesInfoDel OnShowMesInfo;




        //----------[对象实例]---------
        public static Config _Config;
        public static MesData _MESData;
        public static clsMXCom1.CMXComm1 _QPLC;
        public static MesClient _MesClient;

        public static void Init()
        {

            _Config = Config.Instance;
            _Config.Load();

            _MESData = MesData.Instance;
            _MESData.Load();

           

            _QPLC = new clsMXCom1.CMXComm1();
            bool res = _QPLC.Open(2, "");
            Print(res ? "PLC连接成功" : "PLC连接失败");


            _MesClient = MesClient.Instance;
            _MesClient.Init();

        }

        /// <summary>
        /// 气泡弹窗提示
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public static void ShowTips(TipType type, string msg)
        {
            if (OnShowTips != null)
            {
                OnShowTips(type, msg);
            }
        }
        /// <summary>
        /// 将信息显示在主界面控件上
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        public static void ShowMesInfo(string type, string val)
        {
            if (OnShowMesInfo != null)
            {
                OnShowMesInfo(type, val);
            }
        }
        public static void Print(string info, bool isalarm = false)
        {
            if (OnShowMessge != null)
            {
                OnShowMessge(info, isalarm);
            }

        }
        public static void PritAlarm(string info)
        {
            if (OnShowMessge != null)
            {
                OnShowMessge(info, true);
            }

        }

    }

    public static class DevProcessLog
    {
        public readonly static string path = Application.StartupPath + "\\logs";
        public static void Save(string msg, bool isalarm = false)
        {
            try
            {
                string dirpath = Path.Combine(path, "log");
                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }
                string time = DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff");

                string fullname;
                if (isalarm)
                {
                    fullname = dirpath + @"\" + DateTime.Now.ToString("yyyyMMdd") + "Alarm.txt";
                }
                else
                {
                    fullname = dirpath + @"\" + DateTime.Now.ToString("yyyyMMdd") + "Info.txt";
                }
                File.AppendAllText(fullname, time + ":" + msg + "\r\n");
            }
            catch (Exception ex)
            {

            }
        }


        public static void SaveMCMQ(string sn, string info)
        {
            string dirpath = Path.Combine(path, "MCMQlog", DateTime.Now.ToString("yyyyMMdd"));
            string fullname = Path.Combine(dirpath, sn + ".txt");
            try
            {
                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }
                string time = DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff");
                File.AppendAllText(fullname, time + ":" + info + "\r\n");
            }
            catch (Exception ex)
            {
            }
        }

        public static void SaveNormalTest(string info)
        {
            string dirpath = Path.Combine(path, "MCMQlog", DateTime.Now.ToString("yyyyMMdd"));
            string fullname = Path.Combine(dirpath, "NormalTest.txt");
            try
            {
                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }
                string time = DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff");
                File.AppendAllText(fullname, time + ":" + info + "\r\n");
            }
            catch (Exception ex)
            {
            }
        }



    }
}

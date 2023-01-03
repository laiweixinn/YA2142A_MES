using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using McmqApi;
using System.Data;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Forms;
using DeviceManager;

namespace DeviceMCMQ
{

    public class MCMQ
    {
        private string function = "", user_id = "", sys_para = "", p_area = "", area = "", line = "",
                       operation = "", eqp_list = "", material_list = "", material_type = "";//回传给MES的数据
        private string material_lot_no = "";
        private string sn = "", icsn = "", lotresult = "", loterr_msg = "";
        private string outresult = "", outerr_msg = "", outgrade = "";//显示的数据
        public static bool IsMESConnected = false;
        private string aoiresult = "";
        private bool sendmaterial = true;//
        public static clsMXCom1.CMXComm1 QPLC;
        /// <summary>
        /// 被主机MES叫起  需要将窗体最大化显示出来
        /// </summary>
        public static bool IsNormalTestCall;
        static MCMQ _Instance;
        DeviceDatas.CIMConfig _config;

        private Thread tdCIMProcess;
        private Thread tdListen;
        public int step = 1;

        string snScanFinshAddr = "D16000";//产品扫码完成
        string icScanFinshAddr = "D16002";//IC扫码完成
        string aoiResultAddr = "D16004";//视觉检测结果
        string isEighthAddr = "D16008";//8抽1     
        string isTrackoutRetryAddr = "D16010";//是否重新过账

        string validateResultAddr = "D16050";
        string trackoutResultAddr = "D16052";
        string plcheartbeatAddr = "D16054";//心跳
        string mesStatusAddr = "D16056";//mes连接状态

        string SNAddr = "D16100";//扫码SN
        string icSNAddr = "D16200";//ic sn

        private MCMQ()
        {

        }

        public static MCMQ Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MCMQ();
                }
                return _Instance;
            }
        }

        private McmqApi.cMcmqWApi mcmqwapi;


        public void Init()
        {
            mcmqwapi = new McmqApi.cMcmqWApi();
            _config = DeviceDatas.CIMConfig.Instance;
            QPLC = new clsMXCom1.CMXComm1();
            ThreadPool.QueueUserWorkItem(delegate
            {
                bool res = QPLC.Open(2, "");
                MachineContext.Print("PLC连接" + (res ? "成功" : "失败"), !res);
            });


            tdCIMProcess = new Thread(CIMProcess);
            tdCIMProcess.IsBackground = true;
            tdCIMProcess.Start();

            tdListen = new Thread(Listen);
            tdListen.IsBackground = true;
            tdListen.Start();
        }

        /// <summary>
        /// 解析XML指定节点的值
        /// </summary>
        /// <param name="strxml"></param>
        /// <param name="tag"></param>
        /// <param name="val"></param>
        /// <param name="mainelement"></param>
        /// <returns></returns>
        public static bool GetXElementValue(string strxml, string tag, out string val, string mainelement = "si300_interface")
        {
            try
            {
                DataSet ds = new DataSet();
                StringReader read = new StringReader(strxml);
                XmlTextReader cml = new XmlTextReader(read);
                ds.ReadXml(cml);
                DataTable dt = ds.Tables[mainelement];
                string tmp = dt.Rows[0][tag].ToString();
                val = tmp;
                return true;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:GetXElementValue-" + ex.ToString(), true);
                val = "";
                return false;
            }
        }

        public static bool GetXElementValue(string strxml, string tag, out string val, params string[] mainelements)
        {
            try
            {
                DataSet ds = new DataSet();
                StringReader read = new StringReader(strxml);
                XmlTextReader cml = new XmlTextReader(read);
                ds.ReadXml(cml);
                foreach (string element in mainelements)
                {
                }
                DataTable dt = ds.Tables[0];
                string tmp = dt.Rows[0][tag].ToString();
                val = tmp;
                return true;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:GetXElementValue-" + ex.ToString(), true);

                val = null;
                return false;
            }
        }

        public static string GetCutString(string all, string child)
        {
            string str1 = null;
            string str2 = "<" + child + ">";
            string str3 = "</" + child + ">";
            if (all.Contains(str2) && all.Contains(str3))
            {
                int i1 = all.IndexOf(str2);
                int i2 = all.IndexOf(str3);
                if ((i2 - i1 - child.Length - 2) > 0)
                {
                    str1 = all.Substring(i1 + child.Length + 2, i2 - i1 - child.Length - 2);
                }
            }
            return str1;
        }

        private bool GetMESNormalTestValues(string xml)
        {
            try
            {
                GetXElementValue(xml.Trim(), "user_id", out user_id);
                //GetXElementValue(xml.Trim(), "sys_para", out sys_para);
                GetXElementValue(xml.Trim(), "p_area", out p_area);
                GetXElementValue(xml.Trim(), "area", out area);
                GetXElementValue(xml.Trim(), "line", out line);
                GetXElementValue(xml.Trim(), "operation", out operation);
                GetXElementValue(xml.Trim(), "eqp_list", out eqp_list);
                GetXElementValue(xml.Trim(), "material_list", out material_list);
                GetXElementValue(xml.Trim(), "function", out function);
                GetXElementValue(xml.Trim(), "user_id", out user_id);
                GetXElementValue(xml.Trim(), "material_type", out material_type);
                IsNormalTestCall = true;
                return true;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:GetMESNormalTestValues-" + ex.ToString(), true);
                return false;
            }
        }

        static object obj = new object();//线程互锁  不能同时进行CIM监控与操作

        private void Listen()
        {
            Thread.Sleep(3000);
            try
            {

                LoadNormalTestFile();

                while (true)
                {

                    Thread.Sleep(500);
                    if (QPLC.IsConnected)
                    {
                        QPLC.Write(plcheartbeatAddr, 2, new short[] { 1, 0 });
                        short status = IsMESConnected ? (short)1 : (short)2;
                        QPLC.Write(mesStatusAddr, 2, new short[] { status, 0 });
                    }
                    else
                    {
                        bool res = QPLC.Open(2, "");
                        Thread.Sleep(1000);
                        MachineContext.Print("PLC重接" + (res ? "成功" : "失败"), !res);
                    }

                    lock (obj)
                    {
                        ListenMESNormal_Test();
                        MachineContext.ShowMesInfo("sn", sn);
                        MachineContext.ShowMesInfo("function", function);
                        MachineContext.ShowMesInfo("operation", operation);
                        MachineContext.ShowMesInfo("area", area);
                        MachineContext.ShowMesInfo("line", line);
                        MachineContext.ShowMesInfo("user_id", user_id);
                        MachineContext.ShowMesInfo("material_type", material_type);

                        MachineContext.ShowMesInfo("validateresult", lotresult);
                        MachineContext.ShowMesInfo("error_msg", loterr_msg);
                        MachineContext.ShowMesInfo("trackresult", outresult);
                        MachineContext.ShowMesInfo("outerr_msg", outerr_msg);

                        // MachineContext.ShowMesInfo("grade", outgrade);
                        MachineContext.ShowMesInfo("aoiresult", aoiresult);
                    }
                }

            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:Listen-" + ex.ToString(), true);

            }
        }

        string normalTestFilePath = Application.StartupPath + "\\NormalTest.txt";
        private void LoadNormalTestFile()
        {

            try
            {
                if (File.Exists(normalTestFilePath))
                {
                    string xml = File.ReadAllText(normalTestFilePath);
                    GetMESNormalTestValues(xml);
                }
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:LoadFile_" + ex.ToString(),true);

            }
        }


        private void CIMProcess()
        {
            Thread.Sleep(3000);
            short[] cmd = new short[2];
            string resultmsg;
            bool res;
            while (true)
            {
                try
                {

                    Thread.Sleep(50);
                    if (!QPLC.IsConnected)
                    {
                        continue;
                    }

                    lock (obj)
                    {

                        switch (step)
                        {
                            case 1:
                                QPLC.Read(snScanFinshAddr, 2, out cmd);//扫码完成
                                if (cmd[0] == 1)
                                {
                                    sendmaterial = false;
                                    MachineContext.ShowMesInfo("MCMQresult", "3");
                                    short[] sntmp = new short[100];
                                    QPLC.Read(SNAddr, sntmp.Length, out sntmp);
                                    QPLC.Write(snScanFinshAddr, 2, new short[] { 0, 0 });
                                    QPLC.Read(icScanFinshAddr, 2, out cmd);//IC扫码完成
                                    if (cmd[0] == 1)
                                    {
                                        sendmaterial = true;
                                        short[] icsntmp = new short[100];
                                        QPLC.Read(icSNAddr, icsntmp.Length, out icsntmp);
                                        string stricsn;
                                        ArrayToString(icsntmp, out stricsn);
                                        this.icsn = stricsn.Replace('\0', ' ').Trim();
                                        QPLC.Write(icScanFinshAddr, 2, new short[] { 0, 0 });
                                        if (this.icsn.Length < 2)
                                        {
                                            MachineContext.Print("Error:无效的ICSN-" + icsn);
                                        }
                                    }
                                    lotresult = "";
                                    loterr_msg = "";
                                    aoiresult = "";
                                    outresult = "";
                                    outgrade = "";
                                    string strsn;
                                    ArrayToString(sntmp, out strsn);
                                    this.sn = strsn.Replace('\0', ' ').Trim();
                                    if (this.sn.Length < 2)
                                    {
                                        MachineContext.Print("Error:无效的SN-" + sn);
                                    }

                                    step = 2;
                                    if (_config.TestMode)
                                    {
                                        step = 0;
                                    }
                                }
                                break;

                            case 2://校验SN                                             
                                for (int i = 0; i < 3; i++)
                                {
                                    res = Send(ValidateLot(this.sn), out resultmsg);
                                    if (!res)
                                    {
                                        if (resultmsg == "invaliddata")//网络正常连接 MES无回复的失败 进行NG抛料
                                        {
                                            QPLC.Write(validateResultAddr, 2, new short[] { 2, 0 });//SN校验结果
                                            MachineContext.ShowTips(TipType.Error, this.sn + ":校验失败!\r\n" + "Error:" + loterr_msg);
                                            MachineContext.Print(this.sn + ":校验失败!\r\n" + "Error:" + loterr_msg, true);
                                            MachineContext._CIMPocessData.AddData(sn, "FAIL", "", "", "等待超时");
                                            MachineContext.ShowMesInfo("MCMQresult", "0");
                                            step = 1;
                                            if (_config.TestMode)
                                            {
                                                step = 0;
                                            }
                                            break;
                                        }
                                        else//网络异常导致的失败 需要进行重试
                                        {
                                            Thread.Sleep(500);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (lotresult == "valid")
                                        {
                                            QPLC.Write(validateResultAddr, 2, new short[] { 1, 0 });
                                            MachineContext.ShowTips(TipType.Success, this.sn + ":校验成功!");
                                            MachineContext.Print(this.sn + ":校验成功!");
                                            step = 3;
                                            if (_config.TestMode)
                                            {
                                                step = 0;
                                            }
                                            break;
                                        }
                                        else //if (lotresult == "invalid")
                                        {
                                            QPLC.Write(validateResultAddr, 2, new short[] { lotresult == "invalid" ? (short)2 : (short)3, 0 });//SN校验完成
                                            MachineContext.ShowTips(TipType.Error, this.sn + ":校验失败!\r\n" + "Error:" + loterr_msg);
                                            MachineContext.Print(this.sn + ":校验失败!\r\n" + "Error:" + loterr_msg, true);
                                            MachineContext._CIMPocessData.AddData(sn, "FAIL", "", "", loterr_msg);
                                            MachineContext.ShowMesInfo("MCMQresult", "0");
                                            step = 1;
                                            if (_config.TestMode)
                                            {
                                                step = 0;
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (!IsMESConnected)//未成功发送 可能存在网络异常 进行NG抛料
                                {
                                    MachineContext._CIMPocessData.AddData(sn, "FAIL", "", "", "等待超时");
                                    QPLC.Write(validateResultAddr, 2, new short[] { 2, 0 });//SN校验结果
                                    MachineContext.ShowTips(TipType.Error, this.sn + ":校验失败!\r\n" + "Error:等待超时");
                                    MachineContext.Print(this.sn + ":校验失败!\r\n" + "Error:等待超时", true);
                                    MachineContext.ShowMesInfo("MCMQresult", "0");
                                    step = 1;
                                    if (_config.TestMode)
                                    {
                                        step = 0;
                                    }
                                }
                                break;

                            case 3://是否8抽1
                                QPLC.Read(isEighthAddr, 2, out cmd);
                                if (cmd[0] != 0)
                                {
                                    if (cmd[0] == 1)//是
                                    {
                                        QPLC.Write(isEighthAddr, 2, new short[] { 0, 0 });
                                        MachineContext._CIMPocessData.AddData(sn, "PASS", "", "", "电测抽检");
                                        MachineContext.Print(this.sn + ":电测抽检!");
                                        MachineContext.ShowTips(TipType.Info, this.sn + ":电测抽检!\r\n");
                                        MachineContext.ShowMesInfo("MCMQresult", "2");
                                        step = 1;
                                    }
                                    else //不是
                                    {
                                        QPLC.Write(isEighthAddr, 2, new short[] { 0, 0 });
                                        step++;
                                    }
                                }
                                break;

                            case 4://过账
                                QPLC.Read(aoiResultAddr, 2, out cmd);//AOI完成
                                if (cmd[0] != 0)
                                {
                                    aoiresult = cmd[0] == 1 ? "good" : "defect";
                                    res = Send(TrackOut(sn), out resultmsg);
                                    if (res)
                                    {
                                        if (outresult != "fail")
                                        {
                                            MachineContext.ShowTips(TipType.Success, sn + ":过账成功!");
                                            MachineContext.Print(sn + ":过账成功!");
                                            MachineContext._CIMPocessData.AddData(sn, "PASS", "PASS", aoiresult, "");
                                            QPLC.Write(trackoutResultAddr, 2, new short[] { 1, 0 });
                                            QPLC.Write(aoiResultAddr, 2, new short[] { 0, 0 });
                                            MachineContext.ShowMesInfo("MCMQresult", "1");
                                            step = 1;
                                            if (_config.TestMode)
                                            {
                                                step = 0;
                                            }
                                        }
                                        else
                                        {
                                            MachineContext.ShowTips(TipType.Error, sn + ":过账失败!\r\n" + "Error:" + outerr_msg);
                                            MachineContext.Print(sn + ":过账失败!\r\n" + "Error:" + outerr_msg, true);
                                            QPLC.Write(trackoutResultAddr, 2, new short[] { 2, 0 });
                                            step = 5;
                                            MachineContext.ShowMesInfo("MCMQresult", "0");
                                        }
                                    }
                                    else
                                    {
                                        MachineContext.ShowTips(TipType.Error, sn + ":过账失败!");
                                        MachineContext.Print(sn + ":过账失败!");
                                        MachineContext._CIMPocessData.AddData(sn, "PASS", "FAIL", aoiresult, outerr_msg);
                                        QPLC.Write(trackoutResultAddr, 2, new short[] { 2, 0 });
                                        QPLC.Write(aoiResultAddr, 2, new short[] { 0, 0 });
                                        MachineContext.ShowMesInfo("MCMQresult", "2");
                                        step = 1;
                                        if (_config.TestMode)
                                        {
                                            step = 0;
                                        }
                                    }

                                }
                                break;

                            case 5://是否重新过账
                                QPLC.Read(isTrackoutRetryAddr, 2, out cmd);//AOI完成
                                if (cmd[0] == 1)//重新过账
                                {
                                    QPLC.Write(isTrackoutRetryAddr, 2, new short[] { 0, 0 });
                                    step = 4;
                                }
                                else if (cmd[0] == 2)//否
                                {
                                    MachineContext._CIMPocessData.AddData(sn, "PASS", "FAIL", aoiresult, outerr_msg);
                                    QPLC.Write(isTrackoutRetryAddr, 2, new short[] { 0, 0 });
                                    QPLC.Write(aoiResultAddr, 2, new short[] { 0, 0 });
                                    MachineContext.ShowMesInfo("MCMQresult", "0");
                                    step = 1;
                                    if (_config.TestMode)
                                    {
                                        step = 0;
                                    }
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {

                    MachineContext.Print("Error:CIMProcess-" + ex.ToString(), true);

                }
            }

        }

        private string ValidateLot(string sn)
        {
            string materialval = "";
            if (sendmaterial)
            {
                materialval = $"<materials>" +
                             $"<material>" +
                              $"<material_type>{material_type}</material_type>" +
                              $"<material_lot_no>{icsn}</material_lot_no>" +
                             $"</material>" +
                            $"</materials>";
            }

            string mcmqValue = "<?xml version=\"1.0\" encoding=\"utf - 8\" ?>" +
                             "<si300_interface>" +
                            $"<message_id>{"validate_lot"}</message_id>" +
                            $"<ver>{"2.7.9"}</ver><function>{function}</function>" +
                            $"<user_id>{user_id}</user_id><sys_para>{sys_para}</sys_para>" +
                            $"<p_area>{p_area}</p_area><area>{area}</area>" +
                            $"<line>{line}</line><operation>{operation}</operation>" +
                            $"<port_id>{""}</port_id><mlp_id>{""}</mlp_id>" +
                            $"<jig_id>{""}</jig_id><lot_no>{sn}</lot_no>" +
                            $"<model_no>{""}</model_no><eqp_list>{eqp_list}</eqp_list>" +
                              materialval +
                            "</si300_interface>";

            return mcmqValue;
        }

        private string TrackOut(string sn)
        {
            string materialval = "";
            if (sendmaterial)
            {
                materialval = $"<materials>" +
                             $"<material>" +
                              $"<material_type>{material_type}</material_type>" +
                              $"<material_lot_no>{icsn}</material_lot_no>" +
                             $"</material>" +
                            $"</materials>";
            }
            string mcmqValue = "<?xml version=\"1.0\" encoding=\"utf - 8\" ?>" +
                           "<si300_interface>" +
                          $"<message_id>{"track_out"}</message_id>" +
                          $"<function>{function}</function><result>{aoiresult}</result>" +
                          $"<lot_no>{sn}</lot_no><mlp_id>{""}</mlp_id>" +
                          $"<jig_id>{""}</jig_id><port_id>{""}</port_id>" +
                          $"<user_id>{user_id}</user_id><sys_para>{sys_para}</sys_para>" +
                          $"<p_area>{p_area}</p_area><area>{area}</area>" +
                          $"<line>{line}</line><operation>{operation}</operation>" +
                          $"<eqp_list>{eqp_list}</eqp_list>" +
                          $"<material_list>{material_list}</material_list>" +
                            materialval +
                          "</si300_interface>";

            return mcmqValue;

        }

        public bool ListenMESNormal_Test()
        {

            string rtn = mcmqwapi.connectMCMQ(_config.ServerIP, _config.Port);
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ创建链接异常_{rtn}", true);
                DevProcessLog.SaveNormalTest($"MCMQ创建链接异常_{rtn}");
                IsMESConnected = false;
                return false;
            }


            rtn = mcmqwapi.openQueue(_config.QueueDesc, _config.MaxSize, _config.MaxCount, _config.AlarmCount, _config.AlarmSize, _config.QueueType, false, _config.QueueDesc);//打开接收队列
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ打开接收链接_{_config.QueueDesc}异常_{rtn}", true);
                DevProcessLog.SaveNormalTest($"MCMQ打开接收链接_{_config.QueueDesc}异常_{rtn}");
                return false;
            }
            IsMESConnected = true;
            rtn = mcmqwapi.getQueue(mcmqwapi.nQueueHandle, _config.Timeout);//接受消息
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ获取接收链接数据异常_{rtn}", true);
                DevProcessLog.SaveNormalTest($"MCMQ获取接收链接数据异常_{rtn}");
                return false;
            }
            if (mcmqwapi.getBinMessage == null || mcmqwapi.getBinMessage.Length < 10)
            {
                return false;
            }
            string resultMessage = System.Text.Encoding.Default.GetString(mcmqwapi.getBinMessage);//获取返回消息内容并转换        
            MachineContext.Print(string.Format("MCMQ接收数据{0}", resultMessage));
            DevProcessLog.SaveNormalTest(string.Format("MCMQ接收数据{0}", resultMessage));
            bool res =  GetMESNormalTestValues(resultMessage);
            try
            {
                if (res)
                {
                    File.WriteAllText(resultMessage, normalTestFilePath, Encoding.Default);
                }
            }
            catch (Exception ex)
            {

                MachineContext.Print("Error:WriteText_" + ex.ToString(),true);
            }
            

            return true;
        }

        public bool Send(string xml, out string resultmsg)
        {
            MachineContext.Print($"MCMQ发送数据_{xml}");
            DevProcessLog.SaveMCMQ(sn, "发送数据_" + xml);
            resultmsg = "";
            byte[] msg = System.Text.Encoding.Default.GetBytes(xml);
            string rtn = mcmqwapi.connectMCMQ(_config.ServerIP, _config.Port);//创建连接
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ创建链接异常_{rtn}", true);
                DevProcessLog.SaveMCMQ(sn, $"MCMQ创建链接异常_{rtn}");
                resultmsg = rtn;
                IsMESConnected = false;
                return false;
            }
            IsMESConnected = true;
            rtn = mcmqwapi.openQueue(_config.QueueDesc, _config.MaxSize, _config.MaxCount, _config.AlarmCount, _config.AlarmSize, _config.QueueType, false, _config.QueueDesc);//打开接收队列
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ打开接收链接1{_config.QueueDesc}异常_{rtn}", true);
                DevProcessLog.SaveMCMQ(sn, $"MCMQ打开接收链接1{_config.QueueDesc}异常_{rtn}");
                resultmsg = rtn;
                return false;
            }
            //rtn = mcmqwapi.cleanQueue(mcmqwapi.nQueueHandle);//清除接收队列
            //rtn = mcmqwapi.closeQueue(mcmqwapi.nQueueHandle);//关闭接收队列

            rtn = mcmqwapi.openQueue(_config.QueueName, _config.MaxSize, _config.MaxCount, _config.AlarmCount, _config.AlarmSize, _config.QueueType, false, _config.QueueName);//
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ打开接收链接2{_config.QueueName}异常_{rtn}", true);
                DevProcessLog.SaveMCMQ(sn, $"MCMQ打开接收链接2{_config.QueueName}异常_{rtn}");
                resultmsg = rtn;
                return false;
            }
            rtn = mcmqwapi.putQueue(_config.QueueName, msg, _config.QueueDesc, _config.CorrelationID);//发送数据
            rtn = mcmqwapi.cleanQueue(mcmqwapi.nQueueHandle);//清除接收队列

            //   rtn = mcmqwapi.closeQueue(mcmqwapi.nQueueHandle);//关闭发送队列

            rtn = mcmqwapi.openQueue(_config.QueueDesc, _config.MaxSize, _config.MaxCount, _config.AlarmCount, _config.AlarmSize, _config.QueueType, false, _config.QueueDesc);//打开接收队列
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ打开接收链接3{_config.QueueDesc}异常_{rtn}", true);
                DevProcessLog.SaveMCMQ(sn, $"MCMQ打开接收链接3{_config.QueueDesc}异常_{rtn}");
                resultmsg = rtn;
                return false;
            }

            rtn = mcmqwapi.getQueue(mcmqwapi.nQueueHandle, _config.Timeout);//接受消息
            if (rtn != "0000")
            {
                MachineContext.Print($"MCMQ获取接收链接数据异常_{rtn}", true);
                DevProcessLog.SaveMCMQ(sn, $"MCMQ获取接收链接数据异常_{rtn}");
                resultmsg = rtn;
                return false;
            }

            string recivexml = System.Text.Encoding.Default.GetString(mcmqwapi.getBinMessage);//获取返回消息内容并转换   
            rtn = mcmqwapi.closeQueue(mcmqwapi.nQueueHandle);//关闭接收队列
            rtn = mcmqwapi.disconnect();//关闭连接     
            MachineContext.Print(string.Format("MCMQ接收数据{0}", recivexml));
            DevProcessLog.SaveMCMQ(sn, "接收数据_" + recivexml);
            if (recivexml.Length < 10)
            {
                MachineContext.Print($"MCMQ处理接收链接数据异常_{recivexml}", true);
                DevProcessLog.SaveMCMQ(sn, "MCMQ处理接收链接数据异常");
                resultmsg = "invaliddata";
                return false;
            }

            if (recivexml.Contains("validate_lot_result"))
            {
                GetXElementValue(recivexml.Trim(), "result", out lotresult);
                GetXElementValue(recivexml.Trim(), "error_message", out loterr_msg);

            }
            else if (recivexml.Contains("track_out_result"))
            {
                GetXElementValue(recivexml.Trim(), "result", out outresult);
                GetXElementValue(recivexml.Trim(), "error_message", out outerr_msg);
            }
            else if (recivexml.Contains("normal_test"))
            {
                GetMESNormalTestValues(recivexml.Trim());
            }
            return true;
        }
        public static void ArrayToString(short[] arrValue, out string svalue)
        {
            svalue = "";
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < arrValue.Length; i++)
            {
                string snew1 = Convert.ToString(arrValue[i], 16).PadLeft(4, '0');
                string sh1 = "", sl1 = "";
                sl1 = snew1.Substring(0, 2);
                sh1 = snew1.Substring(2, 2);
                byte bh1 = Convert.ToByte(sh1, 16);
                byte bl2 = Convert.ToByte(sl1, 16);
                char csh1 = Convert.ToChar(bh1);
                char csl2 = Convert.ToChar(bl2);
                sb1.Append(csh1.ToString());
                sb1.Append(csl2.ToString());
            }
            svalue = sb1.ToString();
        }
        public static void StringToArray(string sinput, int iNumberOfData, out short[] arrDeviceValue)
        {
            string debugstr = sinput.Trim().Replace(" ", "\0");
            byte[] btext = Encoding.ASCII.GetBytes(sinput.Trim().Replace(" ", "\0"));
            arrDeviceValue = new short[iNumberOfData];
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < btext.Length; i++)
            {
                string s1 = Convert.ToString(btext[i], 16).PadLeft(2, '0');
                sb1.Append(s1);
            }
            string snew1 = sb1.ToString();
            int length = snew1.Length;
            string tmp1;
            for (int i = 0; i < iNumberOfData; i++)
            {
                int startpos = i * 4;
                if (startpos <= length - 4)
                {
                    tmp1 = snew1.Substring(startpos, 4).PadLeft(4, '0');
                }
                else if (startpos < length)
                {
                    tmp1 = snew1.Substring(startpos).PadLeft(2, '0').PadRight(4, '0');
                }
                else
                {
                    tmp1 = "0000";
                }
                string high = tmp1.Substring(2, 2);
                string low = tmp1.Substring(0, 2);
                arrDeviceValue[i] = Convert.ToInt16(high + low, 16);
            }
        }
        public int PLCWordToInt(short[] plcdata)
        {
            int ivalue = 0;
            ivalue = Convert.ToInt32(plcdata[1].ToString("X4") + plcdata[0].ToString("X4"), 16);
            return ivalue;
        }

    }
}

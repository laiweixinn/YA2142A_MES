using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Forms;
using DeviceManager;

namespace DeviceManager
{

    public class MesClient
    {
        private string function = "", user_id = "", sys_para = "", p_area = "", area = "", line = "",
                       operation = "", eqp_list = "", material_list = "", material_type = "";//回传给MES的数据
        private string sn = "", icsn = "", lotresult = "", loterr_msg = "";
        private string outresult = "", outerr_msg = "";//显示的数据
        public static bool IsMESConnected = false;
        private string aoiresult = "";
        private bool sendmaterial = true;//

        /// <summary>
        /// 被主机MES叫起  需要将窗体最大化显示出来
        /// </summary>
        public static bool IsNormalTestCall;
        static MesClient _Instance;


        private Thread tdCIMProcess;
        private Thread codeTimeProcess;




        string Port;
        string ProjectName;
        string DeviceName;
        string DeviceCode;
        double bendingMaxSize;
        double bendingMinSize;
        string strProductTime;
        string strTearResult;
        string strBendingBZ;
        string strBendingBR;
        string strDwellTemperature;
        string strDwellPressure;
        string strDwellTime;
        string strAOIResult;
        string strAOIA;
        string strAOIB;
        string strQRCode;
        string strUserId;
        string strPacketMes;//Mes发送格式
        string strPacketAdd;//表格添加格式


        int firstAddr = 15000;
        int[] blockData = new int[600];
        short[] qrcodeStatus = new short[51];
        short[] sblockData = new short[600];

        int scanCodeFinishFlag;
        int scanCodeFlag;
        int tearFilmFinishFlag = 0;
        int tearFilmAOIFinishFlag = 0;
        int bendFinishFlag = 0;
        int dwellFinishFlag = 0;
        int dwellAFinishFlag = 0;
        int dwellBFinishFlag = 0;

        string intotime = "";//扫码时间
        int tearFilmRes = 1;
        int axis1ZPockingtPos = 0;
        int axis2ZPockingtPos = 0;
        int axis3ZPockingtPos = 0;
        int axis1RPockingtPos = 0;
        int axis2RPockingtPos = 0;
        int axis3RPockingtPos = 0;
        int dwellPressure = 0;
        int dwellTime = 0;
        int dwellAAOI = 0;
        int dwellBAOI = 0;
        int detectionA = 0;
        int detectionB = 0;

        int tearFilmStation = 0;
        int bendStation = 0;
        int dwellStation = 0;
        private MesClient()
        {

        }

        public static MesClient Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MesClient();
                }
                return _Instance;
            }
        }




        public void Init()
        {

            tdCIMProcess = new Thread(MesSendProcess);
            tdCIMProcess.IsBackground = true;
            tdCIMProcess.Start();
            codeTimeProcess = new Thread(qrCodeProcess);
            codeTimeProcess.IsBackground = true;
            codeTimeProcess.Start();

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

                    Thread.Sleep(200);


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
                MachineContext.Print("Error:LoadFile_" + ex.ToString(), true);

            }
        }


        Dictionary<String, String> qrCodeDic = new Dictionary<String, String>();
        private void qrCodeProcess() {
            while (true) {
                Thread.Sleep(200);
                short[] qrcodeTime = new short[50];//增加扫码时间而读取的二维码
                MachineContext._QPLC.Read(15430, qrcodeStatus.Length, out qrcodeStatus);
                //判断扫码时间                      
                if (qrcodeStatus[0] == 1)
                {
                    string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    Array.Copy(qrcodeStatus, 1, qrcodeTime, 0, 50);
                    string qrcodeStr = "";
                    ArrayToString(qrcodeTime, out qrcodeStr);
                    qrcodeStr = qrcodeStr.Trim('\u0000', '\u0001', '\r');
                    //制程二维码
                    if (qrCodeDic.ContainsKey(qrcodeStr))
                    {
                        qrCodeDic.Remove(qrcodeStr);
                    }
                    qrCodeDic.Add(qrcodeStr, time);
                    MachineContext._QPLC.Write("15430", 1, new short[] { 0 });
                }
            }
        }
      
        private void MesSendProcess()
        {
            Thread.Sleep(1000);
            bool res;
            Port = MachineContext._Config.Port+",";
            ProjectName = MachineContext._Config.ProjectName + ",";
            DeviceName = MachineContext._Config.DeviceName + ",";
            DeviceCode = MachineContext._Config.DeviceCode + ",";
            bendingMaxSize = MachineContext._Config.BendingMaxSize;
            bendingMinSize = MachineContext._Config.BendingMinSize;
            strProductTime="";
            strTearResult = "";
            strBendingBZ = "";
            strBendingBR = "";
            strDwellTemperature = "";
            strDwellPressure = "";
            strDwellTime = "";
            strAOIResult = "";
            strAOIA = "";
            strAOIB = "";
            strQRCode = "";
            strUserId = "";
            strPacketMes = "";
            strPacketAdd = "";
            
            while (true)
            {
                try
                {

                    Thread.Sleep(800);
                    if (!MachineContext._QPLC.IsConnected)
                    {
                        res = MachineContext._QPLC.Open(1, "");
                        Thread.Sleep(1000);
                        MachineContext.Print("PLC重接" + (res ? "成功" : "失败"), !res);
                        continue;
                    }
                    int materialFlag;
                    intotime = "";
                    lock (obj)
                    {
                        MachineContext._QPLC.Read(firstAddr, blockData.Length, out blockData);
                        MachineContext._QPLC.Read(firstAddr, sblockData.Length, out sblockData);
                        short[] qrCodes = new short[50];//下料时统一读取的二维码
                        
                        short[] userIDs = new short[10];


               


                        materialFlag = blockData[0];//有无料标志，1有料/0无料
                        MachineContext.ShowMesInfo("materialFlag", (materialFlag==1)?"有料":"清料");
                        if (0 == materialFlag)
                        {

                            MachineContext.ShowMesInfo("result", "WAIT");
                            continue;
                        }
                        
                    
                        tearFilmStation = blockData[30];
                        bendStation = blockData[31];
                        dwellStation = blockData[32];
                        if (tearFilmStation == 0 | bendStation == 0 || dwellStation == 0)
                        {
                            MachineContext.ShowMesInfo("result", "WAIT");
                            //   continue;
                        }
                        MachineContext.ShowMesInfo("tearFilmStation", tearFilmStation.ToString());
                        MachineContext.ShowMesInfo("bendStation", bendStation.ToString());
                        MachineContext.ShowMesInfo("dwellStation", dwellStation.ToString());

                        scanCodeFinishFlag = blockData[10];//扫码完成状态 1完成/0未完成
                        scanCodeFlag = blockData[21];//是否成功扫出码 0OK/1NG
                        if (scanCodeFinishFlag == 1 && scanCodeFlag == 0)
                        {
                            Array.Copy(sblockData, 50, qrCodes, 0, 50);
                            string qrcodeStr = "";
                            ArrayToString(qrCodes, out qrcodeStr);
                            qrcodeStr = qrcodeStr.Trim('\u0000', '\u0001', '\r');
                            MachineContext.ShowMesInfo("sn", qrcodeStr);
                            //制程二维码

                            if (qrCodeDic.TryGetValue(qrcodeStr, out intotime))
                            {
                                intotime += ",";
                                qrCodeDic.Remove(qrcodeStr);
                            }
                            else {
                                intotime= DateTime.Now.ToString("hhmmss") + ",";
                            }
                            strQRCode = qrcodeStr+",";
                        }
                        else
                        {
                                  //未扫码成功用时间代替
                            strQRCode = DateTime.Now.ToString("hhmmss") + ",";
                            intotime = DateTime.Now.ToString("hhmmss") + ",";
                        }
                        Array.Copy(sblockData, 410, userIDs, 0, 10);
                        string userIdStr = "";
                        ArrayToString(userIDs, out userIdStr);
                        userIdStr = userIdStr.Trim('\u0000', '\u0001', '\r');


                                  //刷卡ID
                        strUserId = userIdStr + ",";
                        tearFilmRes = blockData[22];
                        string result1 = tearFilmRes == 1 ? "NG" : "OK";
                        MachineContext.ShowMesInfo("tearFilmRes", result1);

                        //生产时间
                        strProductTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ",";

                        //撕膜检测结果
                        strTearResult = "撕膜平台" + tearFilmStation +"检测"+ result1 + ",";
                        int position1 = 0;
                        int position2 = 0;
                        switch (bendStation)
                        {
                            case 1:
                                axis1ZPockingtPos = ComBineData(blockData, 124); 
                                axis1RPockingtPos = ComBineData(blockData, 144);
                                position1 = axis1ZPockingtPos;
                                position2 = axis1RPockingtPos;
                                break;
                            case 2:
                                axis2ZPockingtPos = ComBineData(blockData, 224);
                                axis2RPockingtPos = ComBineData(blockData, 244);
                                position1 = axis2ZPockingtPos;
                                position2 = axis2RPockingtPos;
                                break;
                            case 3:
                                axis3ZPockingtPos = ComBineData(blockData, 324);
                                axis3RPockingtPos = ComBineData(blockData, 344);
                                position1 = axis3ZPockingtPos;
                                position2 = axis3RPockingtPos;
                                break;
                            default:
                                position1 = 0;
                                position2 = 0;
                                break;
                        }

                        strBendingBZ = "折弯" + bendStation +"平台" + "BZ轴贴合位:" +(double) position1/10000 + ",";//弯折BZ轴参数
                        strBendingBR = "折弯" + bendStation + "平台" + "BR轴贴合位:" + (double) position2/10000 + ",";//弯折BR轴参数

                        strDwellTemperature = "保压" + dwellStation + "平台温度:120" + ",";//保压温度，暂时为假数据

                        dwellPressure = blockData[400];
                                    //保压压力
                        strDwellPressure = "保压" + dwellStation + "平台压力:" + dwellPressure + ",";
                        dwellTime = blockData[402];
                                              //保压时间
                        strDwellTime = "保压" + dwellStation + "平台 保压时长:" + dwellTime + ",";

                        dwellAAOI = blockData[24];
                        dwellBAOI = blockData[25];
                        if (dwellAAOI == 0 && dwellBAOI == 0)
                        {
                                      //AOI检测结果
                            strAOIResult = "保压" + dwellStation + "平台AOI检测OK" + ",";
                            MachineContext.ShowMesInfo("bendingAOI", "OK");
                        }
                        else
                        {
                            //AOI检测结果
                            strAOIResult = "保压" + dwellStation + "平台AOI检测NG" + ",";
                            MachineContext.ShowMesInfo("bendingAOI", "NG");
                        }
                        switch (dwellStation)
                        {
                            case 1:
                                detectionA = blockData[520];
                                detectionB = blockData[522];
                                break;
                            case 2:
                                detectionA = blockData[524];
                                detectionB = blockData[526];
                                break;
                            case 3:
                                detectionA = blockData[528];
                                detectionB = blockData[530];                              
                                break;
                            case 4:
                                detectionA = blockData[532];
                                detectionB = blockData[534];
                                break;

                        }
                        //弯折精度                        
                        strAOIA = "保压" + dwellStation + "平台A面规格" + bendingMinSize + "-" + bendingMaxSize + "/长度:" + (double)detectionA / 10000 + ",";
                        strAOIB = "保压" + dwellStation + "平台B面规格" + bendingMinSize + "-" + bendingMaxSize + "/长度:" + (double)detectionB / 10000 + ",";
                      


                        //strPacketAdd = strProductTime + strTearResult + strBendingBZ + strBendingBR + strDwellTemperature + strDwellPressure + strDwellTime + strAOIResult + strAOIA + strAOIB + strQRCode + strUserId + Port + ProjectName + DeviceName;
                        strPacketAdd = intotime + strProductTime + strTearResult + strBendingBZ + strBendingBR + strDwellTemperature + strDwellPressure + strDwellTime + strAOIResult + strAOIA + strAOIB + strQRCode + strUserId + Port + DeviceCode + DeviceName;
                        //strPacketMes = Port + DeviceName + strQRCode + strUserId + strTearResult + strBendingBZ + strBendingBR + strDwellTemperature + strDwellPressure + strDwellTime + strAOIResult + strAOIA + strAOIB + strProductTime;
                        MachineContext._MESData.Add(strPacketAdd);
                        strPacketMes = Port + DeviceCode + strQRCode + DeviceName + intotime + strProductTime + strTearResult + strBendingBZ + strBendingBR + strDwellTemperature + strDwellPressure + strDwellTime + strAOIResult + strAOIA + strAOIB + strUserId;
                        strPacketMes += ", , , , , , , , , , , , , , , , , , , ,";
                        string sendRes = "";
                        bool isSendOK = Send(strPacketMes, out sendRes);
                        MachineContext._QPLC.Write("15000", 1, new short[] { 0 });
                        if (!isSendOK)
                        {
                            MachineContext.ShowTips(TipType.Error, this.sn + ":发送失败!\r\n" + "Error:" + loterr_msg);
                            MachineContext.Print(this.sn + ":发送失败!\r\n" + "Error:" + loterr_msg, true);
                            MachineContext.ShowMesInfo("result", "FAIL");
                        }
                        else
                        {
                            MachineContext.ShowTips(TipType.Success, this.sn + ":发送成功!");
                            MachineContext.Print(this.sn + ":发送成功!");
                            MachineContext.ShowMesInfo("result", "PASS");
                        }
                        MachineContext.ShowMesInfo("trackresult", sendRes);
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

            return true;
        }

        public bool Send(string str, out string resultmsg)
        {
            MachineContext.Print($"MCMQ发送数据_{str}");


            object resultBackMsg = WebServiceHelper.Invoke(MachineContext._Config.ServerIP, MachineContext._Config.ClassName, MachineContext._Config.MethodName, new object[] { str });//上传webservice

            if (resultBackMsg.ToString().Substring(0, 2).Equals("OK"))
            {
                resultmsg = "发送成功";
                return true;
            }
            else
            {
                resultmsg = "发送失败";
                return false;
            }


        }

        public static int ComBineData(int[] arrayVal, int index)
        {
            try
            {
                return ((int)arrayVal[index+1] << 16) + arrayVal[index];
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public static int ComBineData(int lowData, int hightData)
        {
            try
            {
                return ((int)hightData << 16) + lowData;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }



        public static int ComBineData(short[] arrayVal, int index)
        {
            try
            {
                return ((int)arrayVal[index+1] << 16) + arrayVal[index];
            }
            catch (Exception ex)
            {

                return 0;
            }
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

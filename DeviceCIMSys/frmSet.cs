using DeviceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HZH_Controls.Forms;
using Microsoft.Win32;

namespace DeviceCIMSys
{
    public partial class frmSet : Form
    {
        public frmSet()
        {
            InitializeComponent();
        }




        static frmSet _Instance;

        public static frmSet Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new frmSet();
                }
                return _Instance;
            }
        }

        private bool GetTextValue()
        {
            try
            {
                if (Convert.ToDouble(txt_bendingMaxSzie.Text) < Convert.ToDouble(txt_bendingMinSize.Text))
                {
                    MessageBox.Show("弯折精度范围数值输入有误！");
                    return false;
                }
                MachineContext._Config.ServerIP = txt_mesIp.Text;              
                MachineContext._Config.ClassName = txt_mesClassName.Text;
                MachineContext._Config.MethodName = txt_mesMethodName.Text;
                MachineContext._Config.Port = txt_mesPort.Text;
                MachineContext._Config.ProjectName = txt_mesProjectName.Text;
                MachineContext._Config.DeviceName = txt_mesDeviceName.Text;
                MachineContext._Config.DeviceCode = txt_mesDeviceCode.Text;

                MachineContext._Config.BendingMaxSize =Convert.ToDouble(txt_bendingMaxSzie.Text);
                MachineContext._Config.BendingMinSize =Convert.ToDouble(txt_bendingMinSize.Text);



                MachineContext._Config.AutoStart = chk_AutoStart.Checked;
                MachineContext._Config.BubbleFrm = chk_BubbleFrm.Checked;
                MachineContext._Config.CallUp = chk_CallUp.Checked;
                MachineContext._Config.FileSaveDays = (int)txt_FileSaveDays.Num;

                MachineContext._Config.TestMode = chk_TestMode.Checked;
                return true;
            }
            catch (Exception ex)
            {
                MachineContext.ShowTips(TipType.Error, "参数保存失败:" + ex.Message);
                return false;
            }
        }

        private void SetTextValue()
        {
            try
            {
                txt_mesIp.Text = MachineContext._Config.ServerIP;
                txt_mesClassName.Text = MachineContext._Config.ClassName;
                txt_mesMethodName.Text = MachineContext._Config.MethodName;
                txt_mesPort.Text = MachineContext._Config.Port;
                txt_mesProjectName.Text = MachineContext._Config.ProjectName;
                txt_mesDeviceName.Text = MachineContext._Config.DeviceName;
                txt_mesDeviceCode.Text = MachineContext._Config.DeviceCode;



                txt_bendingMaxSzie.Text = MachineContext._Config.BendingMaxSize.ToString();
                txt_bendingMinSize.Text = MachineContext._Config.BendingMinSize.ToString();

                chk_AutoStart.Checked = MachineContext._Config.AutoStart;
                chk_BubbleFrm.Checked = MachineContext._Config.BubbleFrm;
                chk_CallUp.Checked = MachineContext._Config.CallUp;
                txt_FileSaveDays.Num = MachineContext._Config.FileSaveDays;

            }
            catch (Exception ex)
            {
            }
        }

        private void frmSet_Load(object sender, EventArgs e)
        {
            MachineContext._Config.Load();
            SetTextValue();
        }




        private void read_Click(object sender, EventArgs e)
        {

            try
            {


                string txt = this.comboBox1.Text;
                if (string.IsNullOrEmpty(txt))
                {
                    DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Warning, "不正确的读取条件!");
                    return;
                }
                string address = addr.InputText;
                short[] plcval = new short[2];
                switch (txt) {
                    case "单字":                             
                        MachineContext._QPLC.Read(address, 2, out plcval);
                        richTextBox1.Clear();
                        richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss") + ":" + plcval[0] + "\r\n");
                        break;
                    case "双字":

                        MachineContext._QPLC.Read(address, plcval.Length, out plcval);
                        richTextBox1.Clear();
                        richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss") + ":" + MesClient.ComBineData(plcval, 0) + "\r\n");
                        break;
                }
            }
            catch (Exception ex)
            {


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

        private void write_Click(object sender, EventArgs e)
        {


            try
            {
                short val = short.Parse(value.InputText);
                string address = addr.InputText;
               MachineContext._QPLC.Write(address, 2, new short[] { val, 0 });
            }
            catch (Exception ex)
            {


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {           
                string address = addr.InputText;
                short[] plcval = new short[100];
                MachineContext._QPLC.Read(address, 100, out plcval);
                string strval;
                MesClient.ArrayToString(plcval, out strval);
                richTextBox1.Clear();
                richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss") + ":" + strval + "\r\n");


            }
            catch (Exception ex)
            {


            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            bool res = GetTextValue();
            SetSoftWareAutoStart();
            if (res)
            {
                MachineContext._Config.Save();
                MachineContext.ShowTips(TipType.Success, "参数保存成功!");
            }
            else
            {
                MachineContext.ShowTips(TipType.Error, "参数保存失败!");
            }
          
        }

        private void SetSoftWareAutoStart()
        {
            try
            {
                if (MachineContext._Config.AutoStart) //设置开机自启动  
                {

                    string path = Application.ExecutablePath;
                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                    rk2.SetValue("YA2142-MES", path);  //注意,Jc为自启动软件的软件名
                    rk2.Close();
                    rk.Close();
                }
                else //取消开机自启动  
                {

                    string path = Application.ExecutablePath;
                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                    rk2.DeleteValue("YA2142-MES", false);
                    rk2.Close();
                    rk.Close();
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

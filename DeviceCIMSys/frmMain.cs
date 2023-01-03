
using DeviceManager;
using HZH_Controls.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace DeviceCIMSys
{
    public partial class frmMain : Form
    {
        private frm_Auto _AutoForm;
        private frmSet _SetForm;
        private frmQuery _QueryForm;

        public frmMain()
        {
            InitializeComponent();
            MachineContext.OnShowTips += ShowTips;
            this.CenterToScreen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _AutoForm = frm_Auto.Instance;
            _SetForm = frmSet.Instance;
            _QueryForm = frmQuery.Instance;
            AddForm(_AutoForm, pel_main);

            MachineContext.Init();

            MachineContext.Print("参数加载完成");
        }



        private void ShowTips(DeviceManager.TipType type, string msg)
        {
            if (!MachineContext._Config.BubbleFrm)
            {
                return;
            }
            this.Invoke(new MethodInvoker(() =>
            {
                switch (type)
                {
                    case DeviceManager.TipType.Error:
                        HZH_Controls.Forms.FrmTips.ShowTipsError(this, msg);
                        break;
                    case DeviceManager.TipType.Info:
                        HZH_Controls.Forms.FrmTips.ShowTipsInfo(this, msg);
                        break;
                    case DeviceManager.TipType.Success:
                        HZH_Controls.Forms.FrmTips.ShowTipsSuccess(this, msg);
                        break;
                    case DeviceManager.TipType.Warning:
                        HZH_Controls.Forms.FrmTips.ShowTipsWarning(this, msg);
                        break;
                    default:
                        break;
                }
            }));
        }




        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        private void frm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFlag = true;                 //点击左键按下时标注为true；
            }
        }
        private void frm_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的位置
                Location = mouseSet;
            }
        }
        private void frm_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false; //释放鼠标后标注为false；
            }
        }



        private void toolp_Main_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = ((ToolStripItem)e.ClickedItem).Name;
            switch (name)
            {
                case "tsb_Main":
                    AddForm(_AutoForm, pel_main);

                    break;

                case "tsb_Set":
                    frmLogIn login = new frmLogIn();
                    DialogResult res = login.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        AddForm(_SetForm, pel_main);
                    }

                    break;

                case "tsb_Total":
                    AddForm(_QueryForm, pel_main);

                    break;


            }
        }


        private void AddForm(Form form, Control panel)
        {

            form.TopLevel = false;
            if (form.Width < panel.Width)
            {
                form.Width = panel.Width;
            }
            if (form.Height < panel.Height)
            {
                form.Height = panel.Height;
            }
            panel.Controls.Clear();
            panel.Controls.Add(form);
            form.Location = new Point(0, 0);
            form.Show();

        }




        DateTime timeBase = DateTime.Now;
        string cimlogpath = MesData.path;
        string runlogpath = DevProcessLog.path;
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_CurrTime.Text = "系统时间:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            TimeSpan ts = (DateTime.Now - timeBase);
            lbl_RunTime.Text = "运行时长:" + $"{ts.Days}天{ts.Hours}时{ts.Minutes}分{ts.Seconds}秒";

            lab_PLCStatus.Text = MachineContext._QPLC.IsConnected ? "连接成功" : "连接失败";
            if (MachineContext._QPLC.IsConnected)
            {
                lab_PLCStatus.BackColor = Color.LimeGreen;
            }
            else
            {

                lab_PLCStatus.BackColor = lab_PLCStatus.BackColor == Color.Red ? Color.WhiteSmoke : Color.Red;
            }


            lab_MESStatus.Text = MesClient.IsMESConnected ? "连接成功" : "连接失败";
            if (MesClient.IsMESConnected)
            {
                lab_MESStatus.BackColor = Color.LimeGreen;
            }
            else
            {
                lab_MESStatus.BackColor = lab_MESStatus.BackColor == Color.Red ? Color.WhiteSmoke : Color.Red;
            }


            if (MachineContext._Config.CallUp)
            {

                this.WindowState = FormWindowState.Normal;
                this.TopLevel = true;

            }


        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            MachineContext._QPLC.Dispose();
        }

        private void pic_frmMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;


        }

        private void pic_frmClose_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("是否退出!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //if (MachineContext._Config.CallUp)
            //{
            //    if (DeviceMCMQ.MCMQ.IsNormalTestCall)
            //    {
            //        this.WindowState = FormWindowState.Normal;
            //        this.TopLevel = true;
            //        DeviceMCMQ.MCMQ.IsNormalTestCall = false;
            //    }
            //    else
            //    {
            //        this.WindowState = FormWindowState.Minimized;
            //    }
            //}

            try
            {

                if (MachineContext._Config.CallUp)
                {


                }





                if (Directory.Exists(cimlogpath))
                {
                    string[] filesname = Directory.GetFiles(cimlogpath);
                    for (int i = 0; i < filesname.Length; i++)
                    {

                        FileInfo info = new FileInfo(filesname[i]);
                        if ((DateTime.Now - info.CreationTime).TotalDays > MachineContext._Config.FileSaveDays)
                        {
                            File.Delete(filesname[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


    }
}

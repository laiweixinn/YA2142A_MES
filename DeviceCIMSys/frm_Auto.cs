using DeviceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;
using RK.CNC.Controls;
using System.IO;

namespace DeviceCIMSys
{
    public partial class frm_Auto : Form
    {
        public frm_Auto()
        {
            InitializeComponent();

            MachineContext.OnShowMessge += WriteLine;
            MachineContext.OnShowMesInfo += ShowMesInfo;
            MesData.OnMesDataChange += RefreshGridView;
        }
        static frm_Auto _Instance;

        public static frm_Auto Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new frm_Auto();
                }
                return _Instance;
            }
        }

        private void frm_Auto_Load(object sender, EventArgs e)
        {

            InitGridView();
            ThreadPool.QueueUserWorkItem(delegate
            {
                Thread.Sleep(3000);
                RefreshGridView();
            });
            chkp_Result.IsChecked = TipsStatus.WAIT;






        }


        private void WriteLine(string msg, bool iserror)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    int i = txt_info.TextLength;
                    if (i > 102400)
                    {
                        txt_info.Clear();
                    }
                    i = txt_info.TextLength;
                    msg = string.Format("{0}:【{1}】\r\n", DateTime.Now.ToString("yyyy:MM:dd-HH:mm:ss fff"), msg);
                    txt_info.AppendText(msg);
                    txt_info.Select(i, txt_info.TextLength - 1);
                    txt_info.SelectionColor = (iserror ? Color.Red : Color.Black);
                    txt_info.SelectionStart = txt_info.TextLength;
                    txt_info.SelectionLength = 0;
                    txt_info.ScrollToCaret();

                    DevProcessLog.Save(msg, iserror);

                }
            }));
        }

        private void ShowMesInfo(string type, string val)
        {

            this.Invoke(new MethodInvoker(() =>
            {

                lbl_ClassName.Text = MachineContext._Config.ClassName;
                lbl_MethodName.Text = MachineContext._Config.MethodName;
                lbl_bendingMaxSzie.Text = MachineContext._Config.BendingMaxSize.ToString();
                lbl_bendingMinSize.Text = MachineContext._Config.BendingMinSize.ToString();
                switch (type)
                {
                    case "sn":
                        sn.Text = val;
                        break;                       
                    case "materialFlag":
                        materialFlag.Text = val;
                        break;
                    case "tearFilmRes":
                        tearFilmRes.Text = val;
                        break;
                    case "bendingAOI":
                        bendingAOI.Text = val;
                        break;

                    case "tearFilmStation":
                        tearFilmStation.Text = val;
                        break;
                    case "bendStation":
                        bendStation.Text = val;
                        break;
                    case "dwellStation":
                        dwellStation.Text = val;
                        break;
                    case "result":
                        chkp_Result.IsChecked = (TipsStatus)Enum.Parse(typeof(TipsStatus), val);
                        break;
                    //case "trackresult":
                    //    bendingMaxSzie.Text = val;
                    //    break;                        
                    default:
                        break;
                }

            }));
        }

        /// <summary>
        /// 刷新界面表单
        /// </summary>
        private void RefreshGridView()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                DataTable tbSource = MachineContext._MESData.GetData();
                if (tbSource == null)
                {
                    return;
                }

                dgv_CIMPutOrder.DataSource = tbSource;
                dgv_CIMPutOrder.Columns[0].HeaderText = "Index";
                dgv_CIMPutOrder.Columns[1].HeaderText = "扫码时间";
                dgv_CIMPutOrder.Columns[2].HeaderText = "下料时间";
                dgv_CIMPutOrder.Columns[3].HeaderText = "撕膜检测结果";
                dgv_CIMPutOrder.Columns[4].HeaderText = "弯折BZ轴参数";
                dgv_CIMPutOrder.Columns[5].HeaderText = "弯折BR轴参数";
                dgv_CIMPutOrder.Columns[6].HeaderText = "保压温度";
                dgv_CIMPutOrder.Columns[7].HeaderText = "保压压力";
                dgv_CIMPutOrder.Columns[8].HeaderText = "保压时间";
                dgv_CIMPutOrder.Columns[9].HeaderText = "AOI检测结果";
                dgv_CIMPutOrder.Columns[10].HeaderText = "检测精度A";
                dgv_CIMPutOrder.Columns[11].HeaderText = "检测精度B";
                dgv_CIMPutOrder.Columns[12].HeaderText = "制程二维码";
                dgv_CIMPutOrder.Columns[13].HeaderText = "刷卡ID";
                dgv_CIMPutOrder.Columns[14].HeaderText = "端口";
                dgv_CIMPutOrder.Columns[15].HeaderText = "设备代码";
                //dgv_CIMPutOrder.Columns[14].HeaderText = "项目代码";
                dgv_CIMPutOrder.Columns[16].HeaderText = "设备类型";

                dgv_CIMPutOrder.Columns[0].Width = 60;
                dgv_CIMPutOrder.Columns[1].Width = 300;
                dgv_CIMPutOrder.Columns[2].Width = 300;
                dgv_CIMPutOrder.Columns[3].Width = 300;
                dgv_CIMPutOrder.Columns[4].Width = 300;
                dgv_CIMPutOrder.Columns[5].Width = 300;
                dgv_CIMPutOrder.Columns[6].Width = 300;
                dgv_CIMPutOrder.Columns[7].Width = 300;
                dgv_CIMPutOrder.Columns[8].Width = 300;
                dgv_CIMPutOrder.Columns[9].Width = 300;
                dgv_CIMPutOrder.Columns[10].Width = 300;
                dgv_CIMPutOrder.Columns[11].Width = 300;
                dgv_CIMPutOrder.Columns[12].Width = 800;
                dgv_CIMPutOrder.Columns[13].Width = 200;
                dgv_CIMPutOrder.Columns[14].Width = 60;
                dgv_CIMPutOrder.Columns[15].Width = 100;
                dgv_CIMPutOrder.Columns[16].Width = 100;


                int idx = MachineContext._MESData.GetData().Rows.Count;
                if (idx != 0)
                {
                    dgv_CIMPutOrder.FirstDisplayedScrollingRowIndex = idx - 1;
                    // dgv_CIMPutOrder.Rows[idx].DefaultCellStyle.BackColor = Color.LightCyan;
                }
                dgv_CIMPutOrder.Refresh();
                Application.DoEvents();
            }));
        }


        private void InitGridView()
        {

            dgv_CIMPutOrder.AllowUserToAddRows = false;//不显示出dataGridView1的最后一行空白
            dgv_CIMPutOrder.BackgroundColor = Color.White;
            dgv_CIMPutOrder.GridColor = Color.Black;//设置网格颜色                                          
            dgv_CIMPutOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//整行      
            //字体样式
            dgv_CIMPutOrder.Font = new Font("微软雅黑", 10, FontStyle.Regular);
            //331禁止添加和删除行
            dgv_CIMPutOrder.AllowUserToDeleteRows = true;
            dgv_CIMPutOrder.AllowUserToAddRows = false;
            //居中
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgv_CIMPutOrder.ColumnHeadersDefaultCellStyle = headerStyle;
            dgv_CIMPutOrder.RowsDefaultCellStyle = headerStyle;








        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Application.DoEvents();
                //string sendMsg1 = MachineContext._Config.Port + "," + MachineContext._Config.ProjectName + MachineContext._Config.DeviceName + "," + "1234567890123456789012345678901234567890123456789012345678901234567890123456789" +
                //          "3742751953" + "," + "A01OK" + "," + "B011BZ8.11" + "," + "B011BR7.11" + "," + "C01H125" + "," + "C01P200" + "," + "C01T10" + "," + "C01OK" + ","
                //          + "C01L2.44-2.64/2.32" + "," + "C01R2.44-2.64/2.33" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sendMsg1= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + "撕膜1OK" + ","  +"折弯1BZ压合位85.6" + "," + "折弯1BR压合位80.4" + "," + "保压1温度120" + "," + "保压1压力140" + "," + "保压1时间10" + "," + "保压1AOIOK" + "," + "保压1A面2.54" + "," + "保压1B面2.56" + "," + "1234567890123456789012345678901234567890123456789012345678901234567890123456789"  + "1234567890" + MachineContext._Config.Port + MachineContext._Config.DeviceCode + MachineContext._Config.DeviceName;
                string sendMsg2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + "撕膜1OK" + "," + "折弯1BZ压合位85.6" + "," + "折弯1BR压合位80.4" + "," + "保压1温度120" + "," + "保压1压力140" + "," + "保压1时间10" + "," + "保压1AOIOK" + "," + "保压1A面2.54" + "," + "保压1B面2.56" + "," + "1234567890123456789012345678901234567890123456789012345678901234567890123456780" + "1234567890" + MachineContext._Config.Port + MachineContext._Config.DeviceCode + MachineContext._Config.DeviceName;
                MachineContext._MESData.Add(sendMsg1);
                MachineContext._MESData.Add(sendMsg2);
                Application.DoEvents();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmTest _frmtest = frmTest.Instance;
            _frmtest.ShowDialog();
        }
        Dictionary<string, DataSet> dicCIMDatas;
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
       
            RefreshGridView();

        }

    private void button5_Click(object sender, EventArgs e)
        {
            string sendMsg1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + "撕膜1OK" + "," + "折弯1BZ压合位85.6" + "," + "折弯1BR压合位80.4" + "," + "保压1温度120" + "," + "保压1压力140" + "," + "保压1时间10" + "," + "保压1AOIOK" + "," + "保压1A面2.54" + "," + "保压1B面2.56" + "," + "123" + "," + "1234567890" + "," + MachineContext._Config.Port + "," + MachineContext._Config.DeviceCode + "," + MachineContext._Config.DeviceName;
            MachineContext._MESData.Add(sendMsg1);
        }
    }
}

using DeviceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceCIMSys
{
    public partial class frmQuery : Form
    {
        Dictionary<string, DataSet> dicCIMDatas;
        public frmQuery()
        {
            InitializeComponent();
        }

        static frmQuery _Instance;

        public static frmQuery Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new frmQuery();
                }
                return _Instance;
            }
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            dicCIMDatas = new Dictionary<string, DataSet>();
            LoadFiles();
            InitGridView();
            RefreshGridView(DateTime.Now);
        }
        private void InitGridView()
        {

            dgv_CIMPutOrder.AllowUserToAddRows = true;//不显示出dataGridView1的最后一行空白
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

            //dgv_CIMPutOrder.MultiSelect = true;

            //dgv_CIMPutOrder.Columns.Add("", "序号");
            //dgv_CIMPutOrder.Columns.Add("", "SN");
            //dgv_CIMPutOrder.Columns.Add("", "时间");
            //dgv_CIMPutOrder.Columns.Add("", "状态");
            //dgv_CIMPutOrder.Columns.Add("", "信息");
            //dgv_CIMPutOrder.Columns[0].Width = 100;
            //dgv_CIMPutOrder.Columns[1].Width = 200;
            //dgv_CIMPutOrder.Columns[2].Width = 500;
            //dgv_CIMPutOrder.Columns[3].Width = 100;
            // dgv_CIMPutOrder.Columns[4].Width = 400;

            //设置对齐方式
            //dgv_CIMPutOrder.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dgv_CIMPutOrder.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dgv_CIMPutOrder.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;






        }
        private void ucRadioButton1_CheckedChangeEvent(object sender, EventArgs e)
        {

        }

        private void btn_Before_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);

        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);

        }


        private void LoadFiles()
        {
            try
            {
                dicCIMDatas = new Dictionary<string, DataSet>();
                string path =MesData.path;
                if (!Directory.Exists(path))
                {
                    DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Info, "数据不存在");
                    Directory.CreateDirectory(path);
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    string[] time = Path.GetFileName(file).Split('.');
                    DataSet ds = new DataSet();
                    ds.ReadXml(file);
                    dicCIMDatas.Add(time[0], ds);
                }
            }
            catch (Exception ex)
            {


            }
        }



        private DataSet GetDayData(DateTime tim)
        {
            try
            {
                string now = tim.ToString("yyyy-MM-dd");

                if (dicCIMDatas.ContainsKey(now))
                {
                    return dicCIMDatas[now];
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;

            }



        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadFiles();
            RefreshGridView(dateTimePicker1.Value);


        }

        private void RefreshGridView(DateTime tim)
        {

            DataSet ds = GetDayData(tim);

            if (ds != null && ds.Tables.Count > 0)
            {
                dgv_CIMPutOrder.DataSource = ds.Tables[0];
            }
            else
            {
                DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Warning, "无当前数据!");
                dgv_CIMPutOrder.DataSource = null;
                return;
            }
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
            dgv_CIMPutOrder.Refresh();
        }

        private void RefreshFomat()
        {
            try
            {

            }
            catch (Exception ex)
            {


            }


        }



        private DataTable FindData(string key, string value, DateTime tm)
        {
            DataTable dt = new DataTable();
            foreach (KeyValuePair<string, DataSet> val in dicCIMDatas)
            {
                if (val.Value.Tables.Count > 0)
                {
                    dt = val.Value.Tables[0].Clone();
                    break;
                }
            }
            DataRow[] drs = new DataRow[0];

            foreach (KeyValuePair<string, DataSet> val in dicCIMDatas)//在所有XML加载进来的DataSet里查询符合条件的DataRow
            {
                if (key != "制程二维码" && val.Value.Tables[0].TableName == tm.ToString("yyyy-MM-dd"))
                {

                    drs = val.Value.Tables[0].Select($"{key}='{value}'");
                    break;
                }
                else if (key == "制程二维码")
                {
                    drs = val.Value.Tables[0].Select($"{key}='{value}'");
                    if (drs.Length > 0)
                    {
                        break;
                    }
                }
            }

            foreach (DataRow dr in drs)
            {
                DataRow drnow = dt.NewRow();
                drnow.ItemArray = dr.ItemArray;
                drnow["Index"] = dt.Rows.Count + 1;
                dt.Rows.Add(drnow);
            }

            return dt;
        }






        private void btn_Query_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbc_Query.SelectedIndex == 1)
                {
                    txt_log.Clear();
                    string sn = txt_SN.InputText;
                    if (string.IsNullOrEmpty(sn))
                    {
                        DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Warning, "无效SN!");
                        return;
                    }
                    FileInfo file = FindLogFileBySN(sn);
                    if (file == null)
                    {
                        txt_log.AppendText("无相关数据！");
                    }
                    else
                    {

                        string[] lines = File.ReadAllLines(file.FullName, Encoding.UTF8);
                        foreach (string line in lines)
                        {
                            WriteLine(line);
                        }
                    }
                    return;
                }

                DataTable dt = null;
                if (radio_QuerybyCondition.Checked)
                {
                    string txt = cmb_type.Text;
                    if (string.IsNullOrEmpty(txt))
                    {
                        DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Warning, "不正确的查找条件!");
                        return;
                    }
                    string key = "";
                    string val = "";
                    switch (txt)
                    {
                      
                        case "过账成功":
                            key = "TrackOut";
                            val = "PASS";
                            break;
                        case "过账失败":
                            key = "TrackOut";
                            val = "FAIL";
                            break;                    
                    }

                    dt = FindData(key, val, dateTimePicker1.Value);
                }
                else
                {
                    string sn = txt_SN.InputText;
                    if (string.IsNullOrEmpty(sn))
                    {
                        DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Warning, "无效SN!");
                        return;
                    }
                    dt = FindData("制程二维码", txt_SN.InputText, dateTimePicker1.Value);
                }
                dgv_CIMPutOrder.DataSource = null;
                if (dt == null || dt.Rows.Count == 0)
                {
                    DeviceManager.MachineContext.ShowTips(DeviceManager.TipType.Warning, "无相关数据!");
                    return;
                }

                dgv_CIMPutOrder.DataSource = dt;
                //dgv_CIMPutOrder.Columns[0].HeaderText = "Index";
                //dgv_CIMPutOrder.Columns[1].HeaderText = "生产时间";
                //dgv_CIMPutOrder.Columns[2].HeaderText = "撕膜检测结果";
                //dgv_CIMPutOrder.Columns[3].HeaderText = "弯折BZ轴参数";
                //dgv_CIMPutOrder.Columns[4].HeaderText = "弯折BR轴参数";
                //dgv_CIMPutOrder.Columns[5].HeaderText = "保压温度";
                //dgv_CIMPutOrder.Columns[6].HeaderText = "保压压力";
                //dgv_CIMPutOrder.Columns[7].HeaderText = "保压时间";
                //dgv_CIMPutOrder.Columns[8].HeaderText = "AOI检测结果";
                //dgv_CIMPutOrder.Columns[9].HeaderText = "检测精度A";
                //dgv_CIMPutOrder.Columns[10].HeaderText = "检测精度B";
                //dgv_CIMPutOrder.Columns[11].HeaderText = "制程二维码";
                //dgv_CIMPutOrder.Columns[12].HeaderText = "刷卡ID";
                //dgv_CIMPutOrder.Columns[13].HeaderText = "端口";
                //dgv_CIMPutOrder.Columns[14].HeaderText = "项目代码";
                //dgv_CIMPutOrder.Columns[15].HeaderText = "设备代码";

                //dgv_CIMPutOrder.Columns[0].Width = 60;
                //dgv_CIMPutOrder.Columns[1].Width = 300;
                //dgv_CIMPutOrder.Columns[2].Width = 300;
                //dgv_CIMPutOrder.Columns[3].Width = 300;
                //dgv_CIMPutOrder.Columns[4].Width = 300;
                //dgv_CIMPutOrder.Columns[5].Width = 300;
                //dgv_CIMPutOrder.Columns[6].Width = 300;
                //dgv_CIMPutOrder.Columns[7].Width = 300;
                //dgv_CIMPutOrder.Columns[8].Width = 300;
                //dgv_CIMPutOrder.Columns[9].Width = 300;
                //dgv_CIMPutOrder.Columns[10].Width = 300;
                //dgv_CIMPutOrder.Columns[11].Width = 800;
                //dgv_CIMPutOrder.Columns[12].Width = 200;
                //dgv_CIMPutOrder.Columns[13].Width = 60;
                //dgv_CIMPutOrder.Columns[14].Width = 100;
                //dgv_CIMPutOrder.Columns[15].Width = 100;

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
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:btn_Query_Click_" + ex.ToString(), true);

            }
        }

        private FileInfo FindLogFileBySN(string sn)
        {
            try
            {
                string path = Application.StartupPath + "\\logs";
                string dirpath = Path.Combine(path, "MCMQlog");

                foreach (string info in Directory.GetDirectories(dirpath))
                {
                    foreach (string file in Directory.GetFiles(info))
                    {
                        string name = Path.GetFileName(file);
                        if (name.Split('.')[10] == sn)
                        {
                            return new FileInfo(file);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:FindLogFileBySN_" + ex.ToString(), true);
                return null;
            }
        }




        private void WriteLine(string msg, bool iserror = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    int i = txt_log.TextLength;
                    if (i > 102400)
                    {
                        txt_log.Clear();
                    }
                    i = txt_log.TextLength;
                    txt_log.AppendText(msg + "\r\n");
                    txt_log.Select(i, txt_log.TextLength - 1);
                    txt_log.SelectionColor = (iserror ? Color.Red : Color.Black);
                    txt_log.SelectionStart = txt_log.TextLength;
                    txt_log.SelectionLength = 0;
                    txt_log.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                MachineContext.Print("Error:WriteLine_" + ex.ToString(), true);

            }
        }



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshGridView(dateTimePicker1.Value);
        }

        private void tbc_Query_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbc_Query.SelectedIndex == 0)
            {
                radio_QuerybyCondition.Enabled = true;
                cmb_type.Enabled = true;
            }
            else if (tbc_Query.SelectedIndex == 1)
            {
                radio_QuerybySN.Checked = true;
                radio_QuerybyCondition.Enabled = false;
                cmb_type.Enabled = false;

            }
        }

        private void frmQuery_Activated(object sender, EventArgs e)
        {
            LoadFiles();
            RefreshGridView(dateTimePicker1.Value);
        }
    }
}

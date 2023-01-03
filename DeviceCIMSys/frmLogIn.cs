using DeviceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceCIMSys
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            INIHelps INI = new INIHelps(Config.path);
            string psw = INI.ReadIniData("CIM", "admin");
            if (txt_psw.InputText == psw)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MachineContext.ShowTips(TipType.Warning, "密码错误!");

            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.TopLevel = true;
        }
    }
}

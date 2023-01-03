using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DeviceCIMSys
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool flg = false;
            Mutex mutex = new Mutex(true, "YA1230C-CIM", out flg);
            if (!flg)
            {
                MessageBox.Show("已经启动了一个相同的程序，不可再次启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(1);
                return;
            }
                   
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}

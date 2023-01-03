using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RK.CNC.Controls
{
    [Flags]
    public enum TipsStatus:int
    {
        FAIL =0,
        PASS,
        NULL,
        WAIT
    }


    public partial class CheckPro : UserControl
    {
        public CheckPro()
        {
            InitializeComponent();
        }
        private TipsStatus _IsChecked;

        public event EventHandler CheckedChanged;

        protected void OnCheckedChanged()
        {
            if (CheckedChanged != null)
            {
                CheckedChanged(this, null);
            }
        }

        public TipsStatus IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                RefreshBox();
            }
        }

        private void RefreshBox()
        {
            switch (_IsChecked)
            {
                case TipsStatus.WAIT:
                    {
                        lbl_IsOK.BackColor = Color.Silver;
                        TrueTip = "WAIT ";
                    }
                    break;
                case TipsStatus.NULL:
                    {
                        lbl_IsOK.BackColor = Color.Yellow;
                        TrueTip = "NULL ";
                    }
                    break;
                case TipsStatus.PASS:
                    {
                        lbl_IsOK.BackColor = Color.LimeGreen;
                        TrueTip = "PASS";
                    }
                    break;

                case TipsStatus.FAIL:
                    {
                        lbl_IsOK.BackColor = Color.Red;
                        TrueTip = "FAIL";
                    }
                    break;
                default:
                    break;
            }

        }

        [Browsable(true), Category("自定义属性"), Description("标签True")]
        public string TrueTip
        {
            get
            {
                return lbl_IsOK.Text;
            }
            set
            {
                lbl_IsOK.Text = value;
            }
        }

        [Browsable(true), Category("自定义属性"), Description("标签False")]
        public string FalseTip
        {
            get
            {
                return lbl_IsOK.Text;
            }
            set
            {
                lbl_IsOK.Text = value;
            }
        }
    }
}

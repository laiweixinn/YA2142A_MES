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
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        static frmTest _Instance;

        public static frmTest Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new frmTest();
                }
                return _Instance;
            }

        }

        private void btn_ValidateLot_Click(object sender, EventArgs e)
        {
           
        }

        private void btn_Trackout_Click(object sender, EventArgs e)
        {
         
        }

        private void btn_Write_Click(object sender, EventArgs e)
        {
            string sn = txb_SN.Text;
            string icsn = txb_ICSN.Text;
            bool icsnsend = chk_ICSN.Checked;

            short[] sntmp = new short[100];
       

        
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {


                short[] val = new short[2];
                //DeviceMCMQ.MCMQ.QPLC.Read("D16000", 2, out val);
                //SN.Text = val[0].ToString();

              
            }
            catch (Exception ex)
            {


            }

        }

        private void hh_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
          
        }

        private void button7_Click(object sender, EventArgs e)
        {
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
          
        }

        private void button11_Click(object sender, EventArgs e)
        {
          
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
        }

        private void button12_Click(object sender, EventArgs e)
        {
          
        }

        private void button13_Click(object sender, EventArgs e)
        {
          
        }

        private void button14_Click(object sender, EventArgs e)
        {
         
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
        }

        private void button18_Click(object sender, EventArgs e)
        {
          
        }

        private void button17_Click(object sender, EventArgs e)
        {
          
        }

        private void button19_Click(object sender, EventArgs e)
        {
         
        }

        private void button20_Click(object sender, EventArgs e)
        {
         
        }
    }
}

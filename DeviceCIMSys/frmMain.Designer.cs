namespace DeviceCIMSys
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolp_Main = new System.Windows.Forms.ToolStrip();
            this.tsb_Main = new System.Windows.Forms.ToolStripButton();
            this.tsb_Set = new System.Windows.Forms.ToolStripButton();
            this.tsb_Total = new System.Windows.Forms.ToolStripButton();
            this.label_User = new System.Windows.Forms.Label();
            this.pel_main = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_RunTime = new System.Windows.Forms.Label();
            this.lbl_CurrTime = new System.Windows.Forms.Label();
            this.lab_PLCStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_MESStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pic_frmClose = new System.Windows.Forms.PictureBox();
            this.pic_frmMin = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.toolp_Main.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_frmClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_frmMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolp_Main
            // 
            this.toolp_Main.AutoSize = false;
            this.toolp_Main.Dock = System.Windows.Forms.DockStyle.None;
            this.toolp_Main.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolp_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Main,
            this.tsb_Set,
            this.tsb_Total});
            this.toolp_Main.Location = new System.Drawing.Point(870, 2);
            this.toolp_Main.Name = "toolp_Main";
            this.toolp_Main.Size = new System.Drawing.Size(239, 66);
            this.toolp_Main.TabIndex = 9;
            this.toolp_Main.Text = "toolStrip1";
            this.toolp_Main.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolp_Main_ItemClicked);
            // 
            // tsb_Main
            // 
            this.tsb_Main.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Main.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_Main.Image = global::DeviceCIMSys.Properties.Resources.Home_62;
            this.tsb_Main.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_Main.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Main.Name = "tsb_Main";
            this.tsb_Main.Size = new System.Drawing.Size(64, 63);
            this.tsb_Main.Text = "主界面";
            this.tsb_Main.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // tsb_Set
            // 
            this.tsb_Set.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Set.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Set.Image")));
            this.tsb_Set.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_Set.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Set.Name = "tsb_Set";
            this.tsb_Set.Size = new System.Drawing.Size(64, 63);
            this.tsb_Set.Text = "设置";
            // 
            // tsb_Total
            // 
            this.tsb_Total.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Total.Image = global::DeviceCIMSys.Properties.Resources.Query_60;
            this.tsb_Total.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_Total.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Total.Name = "tsb_Total";
            this.tsb_Total.Size = new System.Drawing.Size(64, 63);
            this.tsb_Total.Text = "数据查询";
            // 
            // label_User
            // 
            this.label_User.AutoSize = true;
            this.label_User.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_User.Location = new System.Drawing.Point(6, 15);
            this.label_User.Name = "label_User";
            this.label_User.Size = new System.Drawing.Size(79, 40);
            this.label_User.TabIndex = 1;
            this.label_User.Text = "PLC\r\n连接状态：";
            this.label_User.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.label_User.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.label_User.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // pel_main
            // 
            this.pel_main.BackColor = System.Drawing.Color.White;
            this.pel_main.Location = new System.Drawing.Point(5, 69);
            this.pel_main.Name = "pel_main";
            this.pel_main.Size = new System.Drawing.Size(1234, 644);
            this.pel_main.TabIndex = 261;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(1127, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Version:V2.7.21";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // lbl_RunTime
            // 
            this.lbl_RunTime.AutoSize = true;
            this.lbl_RunTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_RunTime.Location = new System.Drawing.Point(6, 9);
            this.lbl_RunTime.Name = "lbl_RunTime";
            this.lbl_RunTime.Size = new System.Drawing.Size(172, 20);
            this.lbl_RunTime.TabIndex = 2;
            this.lbl_RunTime.Text = "运行时长:1天2时50分22秒";
            this.lbl_RunTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.lbl_RunTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.lbl_RunTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // lbl_CurrTime
            // 
            this.lbl_CurrTime.AutoSize = true;
            this.lbl_CurrTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_CurrTime.Location = new System.Drawing.Point(6, 37);
            this.lbl_CurrTime.Name = "lbl_CurrTime";
            this.lbl_CurrTime.Size = new System.Drawing.Size(190, 20);
            this.lbl_CurrTime.TabIndex = 3;
            this.lbl_CurrTime.Text = "当前时间:2022/7/9 23:00:00 ";
            this.lbl_CurrTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.lbl_CurrTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.lbl_CurrTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // lab_PLCStatus
            // 
            this.lab_PLCStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lab_PLCStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_PLCStatus.Location = new System.Drawing.Point(82, 8);
            this.lab_PLCStatus.Name = "lab_PLCStatus";
            this.lab_PLCStatus.Size = new System.Drawing.Size(106, 55);
            this.lab_PLCStatus.TabIndex = 263;
            this.lab_PLCStatus.Text = "连接成功";
            this.lab_PLCStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lab_PLCStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.lab_PLCStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.lab_PLCStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_CurrTime);
            this.groupBox2.Controls.Add(this.lbl_RunTime);
            this.groupBox2.Location = new System.Drawing.Point(195, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 66);
            this.groupBox2.TabIndex = 265;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lab_PLCStatus);
            this.groupBox3.Controls.Add(this.label_User);
            this.groupBox3.Location = new System.Drawing.Point(642, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(197, 67);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lab_MESStatus);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(430, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 69);
            this.groupBox1.TabIndex = 264;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // lab_MESStatus
            // 
            this.lab_MESStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lab_MESStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_MESStatus.Location = new System.Drawing.Point(77, 10);
            this.lab_MESStatus.Name = "lab_MESStatus";
            this.lab_MESStatus.Size = new System.Drawing.Size(106, 55);
            this.lab_MESStatus.TabIndex = 263;
            this.lab_MESStatus.Text = "连接成功";
            this.lab_MESStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lab_MESStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.lab_MESStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.lab_MESStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 40);
            this.label3.TabIndex = 1;
            this.label3.Text = "MES\r\n连接状态:";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.label3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.label3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // pic_frmClose
            // 
            this.pic_frmClose.Image = global::DeviceCIMSys.Properties.Resources._00j58PaICI79_1024;
            this.pic_frmClose.Location = new System.Drawing.Point(1201, 1);
            this.pic_frmClose.Name = "pic_frmClose";
            this.pic_frmClose.Size = new System.Drawing.Size(31, 34);
            this.pic_frmClose.TabIndex = 267;
            this.pic_frmClose.TabStop = false;
            this.pic_frmClose.Click += new System.EventHandler(this.pic_frmClose_Click);
            // 
            // pic_frmMin
            // 
            this.pic_frmMin.Image = global::DeviceCIMSys.Properties.Resources.min;
            this.pic_frmMin.Location = new System.Drawing.Point(1129, 4);
            this.pic_frmMin.Name = "pic_frmMin";
            this.pic_frmMin.Size = new System.Drawing.Size(34, 34);
            this.pic_frmMin.TabIndex = 266;
            this.pic_frmMin.TabStop = false;
            this.pic_frmMin.Click += new System.EventHandler(this.pic_frmMin_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(168, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 230;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1241, 717);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pic_frmClose);
            this.Controls.Add(this.pic_frmMin);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pel_main);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.toolp_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frm_MouseUp);
            this.toolp_Main.ResumeLayout(false);
            this.toolp_Main.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_frmClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_frmMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolp_Main;
        private System.Windows.Forms.ToolStripButton tsb_Main;
        private System.Windows.Forms.ToolStripButton tsb_Set;
        private System.Windows.Forms.ToolStripButton tsb_Total;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_User;
        private System.Windows.Forms.Panel pel_main;
        private System.Windows.Forms.Label lbl_CurrTime;
        private System.Windows.Forms.Label lbl_RunTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lab_PLCStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pic_frmMin;
        private System.Windows.Forms.PictureBox pic_frmClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lab_MESStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer2;
    }
}


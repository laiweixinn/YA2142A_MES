namespace DeviceCIMSys
{
    partial class frmQuery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btn_Before = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.btn_Query = new System.Windows.Forms.Button();
            this.cmb_type = new System.Windows.Forms.ComboBox();
            this.radio_QuerybyCondition = new HZH_Controls.Controls.UCRadioButton();
            this.radio_QuerybySN = new HZH_Controls.Controls.UCRadioButton();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.txt_SN = new HZH_Controls.Controls.UCTextBoxEx();
            this.dgv_CIMPutOrder = new System.Windows.Forms.DataGridView();
            this.tbc_Query = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txt_log = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CIMPutOrder)).BeginInit();
            this.tbc_Query.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Location = new System.Drawing.Point(295, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(193, 29);
            this.dateTimePicker1.TabIndex = 5;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // btn_Before
            // 
            this.btn_Before.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Before.Location = new System.Drawing.Point(193, 12);
            this.btn_Before.Name = "btn_Before";
            this.btn_Before.Size = new System.Drawing.Size(96, 29);
            this.btn_Before.TabIndex = 6;
            this.btn_Before.Text = "前一天";
            this.btn_Before.UseVisualStyleBackColor = true;
            this.btn_Before.Click += new System.EventHandler(this.btn_Before_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Next.Location = new System.Drawing.Point(494, 12);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(96, 29);
            this.btn_Next.TabIndex = 7;
            this.btn_Next.Text = "后一天";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // btn_Query
            // 
            this.btn_Query.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Query.Location = new System.Drawing.Point(1041, 238);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(108, 46);
            this.btn_Query.TabIndex = 8;
            this.btn_Query.Text = "查询";
            this.btn_Query.UseVisualStyleBackColor = true;
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // cmb_type
            // 
            this.cmb_type.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_type.FormattingEnabled = true;
            this.cmb_type.Items.AddRange(new object[] {
            "撕膜1平台",
            "撕膜2平台",
            "撕膜3平台",
            "折弯1平台",
            "折弯2平台",
            "折弯3平台",
            "保压1平台",
            "保压2平台",
            "保压3平台",
            "保压4平台"});
            this.cmb_type.Location = new System.Drawing.Point(1019, 152);
            this.cmb_type.Name = "cmb_type";
            this.cmb_type.Size = new System.Drawing.Size(164, 29);
            this.cmb_type.TabIndex = 9;
            this.cmb_type.Visible = false;
            // 
            // radio_QuerybyCondition
            // 
            this.radio_QuerybyCondition.Checked = true;
            this.radio_QuerybyCondition.GroupName = null;
            this.radio_QuerybyCondition.Location = new System.Drawing.Point(1030, 101);
            this.radio_QuerybyCondition.Name = "radio_QuerybyCondition";
            this.radio_QuerybyCondition.Size = new System.Drawing.Size(136, 30);
            this.radio_QuerybyCondition.TabIndex = 10;
            this.radio_QuerybyCondition.TextValue = "按条件查找";
            this.radio_QuerybyCondition.Visible = false;
            this.radio_QuerybyCondition.CheckedChangeEvent += new System.EventHandler(this.ucRadioButton1_CheckedChangeEvent);
            // 
            // radio_QuerybySN
            // 
            this.radio_QuerybySN.Checked = false;
            this.radio_QuerybySN.GroupName = null;
            this.radio_QuerybySN.Location = new System.Drawing.Point(813, 18);
            this.radio_QuerybySN.Name = "radio_QuerybySN";
            this.radio_QuerybySN.Size = new System.Drawing.Size(108, 30);
            this.radio_QuerybySN.TabIndex = 11;
            this.radio_QuerybySN.TextValue = "按SN查找";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Refresh.Location = new System.Drawing.Point(638, 12);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(96, 29);
            this.btn_Refresh.TabIndex = 14;
            this.btn_Refresh.Text = "刷新";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // txt_SN
            // 
            this.txt_SN.BackColor = System.Drawing.Color.Transparent;
            this.txt_SN.ConerRadius = 5;
            this.txt_SN.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_SN.DecLength = 2;
            this.txt_SN.FillColor = System.Drawing.Color.Empty;
            this.txt_SN.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.txt_SN.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SN.InputText = "";
            this.txt_SN.InputType = HZH_Controls.TextInputType.NotControl;
            this.txt_SN.IsFocusColor = true;
            this.txt_SN.IsRadius = true;
            this.txt_SN.IsShowClearBtn = true;
            this.txt_SN.IsShowKeyboard = true;
            this.txt_SN.IsShowRect = false;
            this.txt_SN.IsShowSearchBtn = false;
            this.txt_SN.KeyBoardType = HZH_Controls.Controls.KeyBoardType.UCKeyBorderAll_EN;
            this.txt_SN.Location = new System.Drawing.Point(911, 12);
            this.txt_SN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_SN.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txt_SN.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.txt_SN.Name = "txt_SN";
            this.txt_SN.Padding = new System.Windows.Forms.Padding(5);
            this.txt_SN.PromptColor = System.Drawing.Color.Gray;
            this.txt_SN.PromptFont = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SN.PromptText = "";
            this.txt_SN.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txt_SN.RectWidth = 1;
            this.txt_SN.RegexPattern = "";
            this.txt_SN.Size = new System.Drawing.Size(306, 42);
            this.txt_SN.TabIndex = 15;
            // 
            // dgv_CIMPutOrder
            // 
            this.dgv_CIMPutOrder.BackgroundColor = System.Drawing.Color.White;
            this.dgv_CIMPutOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_CIMPutOrder.GridColor = System.Drawing.Color.Black;
            this.dgv_CIMPutOrder.Location = new System.Drawing.Point(3, 6);
            this.dgv_CIMPutOrder.Name = "dgv_CIMPutOrder";
            this.dgv_CIMPutOrder.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_CIMPutOrder.RowTemplate.Height = 23;
            this.dgv_CIMPutOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_CIMPutOrder.Size = new System.Drawing.Size(981, 554);
            this.dgv_CIMPutOrder.TabIndex = 16;
            // 
            // tbc_Query
            // 
            this.tbc_Query.Controls.Add(this.tabPage1);
            this.tbc_Query.Controls.Add(this.tabPage2);
            this.tbc_Query.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbc_Query.Location = new System.Drawing.Point(6, 47);
            this.tbc_Query.Name = "tbc_Query";
            this.tbc_Query.SelectedIndex = 0;
            this.tbc_Query.Size = new System.Drawing.Size(998, 600);
            this.tbc_Query.TabIndex = 17;
            this.tbc_Query.SelectedIndexChanged += new System.EventHandler(this.tbc_Query_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgv_CIMPutOrder);
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(990, 566);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txt_log);
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(990, 566);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "日志查询";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txt_log
            // 
            this.txt_log.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_log.Location = new System.Drawing.Point(6, 6);
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(978, 554);
            this.txt_log.TabIndex = 0;
            this.txt_log.Text = "";
            // 
            // frmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1230, 659);
            this.Controls.Add(this.tbc_Query);
            this.Controls.Add(this.txt_SN);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.radio_QuerybySN);
            this.Controls.Add(this.radio_QuerybyCondition);
            this.Controls.Add(this.cmb_type);
            this.Controls.Add(this.btn_Query);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.btn_Before);
            this.Controls.Add(this.dateTimePicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmQuery";
            this.Text = "frmQuery";
            this.Activated += new System.EventHandler(this.frmQuery_Activated);
            this.Load += new System.EventHandler(this.frmQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CIMPutOrder)).EndInit();
            this.tbc_Query.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btn_Before;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.Button btn_Query;
        private System.Windows.Forms.ComboBox cmb_type;
        private HZH_Controls.Controls.UCRadioButton radio_QuerybyCondition;
        private HZH_Controls.Controls.UCRadioButton radio_QuerybySN;
        private System.Windows.Forms.Button btn_Refresh;
        private HZH_Controls.Controls.UCTextBoxEx txt_SN;
        private System.Windows.Forms.DataGridView dgv_CIMPutOrder;
        private System.Windows.Forms.TabControl tbc_Query;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox txt_log;
    }
}
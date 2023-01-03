namespace RK.CNC.Controls
{
    partial class CheckPro
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_IsOK = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_IsOK
            // 
            this.lbl_IsOK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_IsOK.BackColor = System.Drawing.Color.Yellow;
            this.lbl_IsOK.Font = new System.Drawing.Font("宋体", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_IsOK.Location = new System.Drawing.Point(0, 0);
            this.lbl_IsOK.Name = "lbl_IsOK";
            this.lbl_IsOK.Size = new System.Drawing.Size(207, 108);
            this.lbl_IsOK.TabIndex = 0;
            this.lbl_IsOK.Text = "NULL";
            this.lbl_IsOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckPro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_IsOK);
            this.Name = "CheckPro";
            this.Size = new System.Drawing.Size(207, 108);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_IsOK;
    }
}

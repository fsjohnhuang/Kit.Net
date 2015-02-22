namespace Win
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cfg = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnView = new System.Windows.Forms.Button();
            this.cst = new C1.Win.C1SuperTooltip.C1SuperTooltip(this.components);
            this.selectLists1 = new lpp.WinFormController.SelectLists();
            ((System.ComponentModel.ISupportInitialize)(this.cfg)).BeginInit();
            this.SuspendLayout();
            // 
            // cfg
            // 
            this.cfg.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cfg.ExtendLastCol = true;
            this.cfg.Location = new System.Drawing.Point(12, 12);
            this.cfg.Name = "cfg";
            this.cfg.Rows.DefaultSize = 20;
            this.cfg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            this.cfg.Size = new System.Drawing.Size(540, 150);
            this.cfg.TabIndex = 0;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(477, 180);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "查  看";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // cst
            // 
            this.cst.AutoPopDelay = 2000;
            this.cst.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cst.IsBalloon = true;
            this.cst.Opacity = 0.7D;
            // 
            // selectLists1
            // 
            this.selectLists1.Location = new System.Drawing.Point(-4, 209);
            this.selectLists1.Name = "selectLists1";
            this.selectLists1.Size = new System.Drawing.Size(570, 247);
            this.selectLists1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 504);
            this.Controls.Add(this.selectLists1);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.cfg);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.cfg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cfg;
        private System.Windows.Forms.Button btnView;
        private C1.Win.C1SuperTooltip.C1SuperTooltip cst;
        private lpp.WinFormController.SelectLists selectLists1;
    }
}


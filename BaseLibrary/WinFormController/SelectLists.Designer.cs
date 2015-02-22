namespace lpp.WinFormController
{
    partial class SelectLists
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
            this.components = new System.ComponentModel.Container();
            this.cfgSrc = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.cfgDest = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnSrc2Dest = new System.Windows.Forms.Button();
            this.btnDest2Src = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnRollback = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cst = new C1.Win.C1SuperTooltip.C1SuperTooltip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cfgSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfgDest)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cfgSrc
            // 
            this.cfgSrc.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.Light3D;
            this.cfgSrc.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cfgSrc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cfgSrc.ExtendLastCol = true;
            this.cfgSrc.Location = new System.Drawing.Point(3, 3);
            this.cfgSrc.Name = "cfgSrc";
            this.cfgSrc.Rows.DefaultSize = 20;
            this.cfgSrc.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            this.cfgSrc.Size = new System.Drawing.Size(262, 367);
            this.cfgSrc.TabIndex = 0;
            // 
            // cfgDest
            // 
            this.cfgDest.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cfgDest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cfgDest.ExtendLastCol = true;
            this.cfgDest.Location = new System.Drawing.Point(371, 3);
            this.cfgDest.Name = "cfgDest";
            this.cfgDest.Rows.DefaultSize = 20;
            this.cfgDest.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            this.cfgDest.Size = new System.Drawing.Size(262, 367);
            this.cfgDest.TabIndex = 1;
            // 
            // btnSrc2Dest
            // 
            this.btnSrc2Dest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSrc2Dest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSrc2Dest.Location = new System.Drawing.Point(25, 95);
            this.btnSrc2Dest.Name = "btnSrc2Dest";
            this.btnSrc2Dest.Size = new System.Drawing.Size(43, 30);
            this.btnSrc2Dest.TabIndex = 2;
            this.btnSrc2Dest.UseVisualStyleBackColor = true;
            // 
            // btnDest2Src
            // 
            this.btnDest2Src.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDest2Src.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDest2Src.Location = new System.Drawing.Point(25, 255);
            this.btnDest2Src.Name = "btnDest2Src";
            this.btnDest2Src.Size = new System.Drawing.Size(43, 30);
            this.btnDest2Src.TabIndex = 3;
            this.btnDest2Src.UseVisualStyleBackColor = true;
            // 
            // btnExchange
            // 
            this.btnExchange.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExchange.Location = new System.Drawing.Point(25, 176);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(43, 30);
            this.btnExchange.TabIndex = 4;
            this.btnExchange.UseVisualStyleBackColor = true;
            // 
            // btnRollback
            // 
            this.btnRollback.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRollback.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRollback.Location = new System.Drawing.Point(25, 24);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(43, 30);
            this.btnRollback.TabIndex = 5;
            this.btnRollback.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.cfgSrc, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cfgDest, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(636, 373);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(271, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 367);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnRollback, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnSrc2Dest, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnDest2Src, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.btnExchange, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(94, 367);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // cst
            // 
            this.cst.Font = new System.Drawing.Font("Tahoma", 8F);
            // 
            // SelectLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SelectLists";
            this.Size = new System.Drawing.Size(636, 373);
            ((System.ComponentModel.ISupportInitialize)(this.cfgSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfgDest)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cfgSrc;
        private C1.Win.C1FlexGrid.C1FlexGrid cfgDest;
        private System.Windows.Forms.Button btnSrc2Dest;
        private System.Windows.Forms.Button btnDest2Src;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button btnRollback;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private C1.Win.C1SuperTooltip.C1SuperTooltip cst;

    }
}

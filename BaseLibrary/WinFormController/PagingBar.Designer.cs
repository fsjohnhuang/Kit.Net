namespace lpp.WinFormController
{
    partial class PagingBar
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
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnGoTo = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.lblIndex = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(18, 20);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(41, 12);
            this.lblPageInfo.TabIndex = 0;
            this.lblPageInfo.Text = "label1";
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFirst.Location = new System.Drawing.Point(323, 15);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(56, 23);
            this.btnFirst.TabIndex = 1;
            this.btnFirst.Text = "首  页";
            this.btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPrev.Location = new System.Drawing.Point(390, 15);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(56, 23);
            this.btnPrev.TabIndex = 2;
            this.btnPrev.Text = "上一页";
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnNext.Location = new System.Drawing.Point(460, 15);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(56, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "下一页";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnGoTo
            // 
            this.btnGoTo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnGoTo.Location = new System.Drawing.Point(728, 15);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(56, 23);
            this.btnGoTo.TabIndex = 4;
            this.btnGoTo.Text = "跳  转";
            this.btnGoTo.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            this.btnLast.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLast.Location = new System.Drawing.Point(530, 15);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(56, 23);
            this.btnLast.TabIndex = 5;
            this.btnLast.Text = "末  页";
            this.btnLast.UseVisualStyleBackColor = true;
            // 
            // lblIndex
            // 
            this.lblIndex.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblIndex.AutoSize = true;
            this.lblIndex.Location = new System.Drawing.Point(592, 20);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(131, 12);
            this.lblIndex.TabIndex = 6;
            this.lblIndex.Text = "第                 页";
            // 
            // txtPage
            // 
            this.txtPage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtPage.Location = new System.Drawing.Point(610, 16);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(95, 21);
            this.txtPage.TabIndex = 7;
            this.txtPage.Text = "1";
            // 
            // PagingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.lblIndex);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.lblPageInfo);
            this.Name = "PagingBar";
            this.Size = new System.Drawing.Size(804, 52);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnGoTo;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Label lblIndex;
        private System.Windows.Forms.TextBox txtPage;
    }
}

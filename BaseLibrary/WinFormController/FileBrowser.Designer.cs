namespace lpp.WinFormController
{
    partial class FileBrowser
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tvFile = new System.Windows.Forms.TreeView();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCat = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.tss = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDir = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(330, 372);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(247, 372);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保 存(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // tvFile
            // 
            this.tvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFile.Location = new System.Drawing.Point(0, 2);
            this.tvFile.Name = "tvFile";
            this.tvFile.Size = new System.Drawing.Size(416, 361);
            this.tvFile.TabIndex = 3;
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCat,
            this.tsmiEdit,
            this.tsmiCreate,
            this.tsmiRename,
            this.tsmiDelete,
            this.tsmiDownload});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(107, 136);
            // 
            // tsmiCat
            // 
            this.tsmiCat.Name = "tsmiCat";
            this.tsmiCat.Size = new System.Drawing.Size(106, 22);
            this.tsmiCat.Text = "查看";
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(106, 22);
            this.tsmiEdit.Text = "编辑";
            // 
            // tsmiCreate
            // 
            this.tsmiCreate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTxt,
            this.tss,
            this.tsmiDir});
            this.tsmiCreate.Name = "tsmiCreate";
            this.tsmiCreate.Size = new System.Drawing.Size(106, 22);
            this.tsmiCreate.Text = "新建";
            // 
            // tsmiTxt
            // 
            this.tsmiTxt.Name = "tsmiTxt";
            this.tsmiTxt.Size = new System.Drawing.Size(118, 22);
            this.tsmiTxt.Text = "文本文件";
            // 
            // tss
            // 
            this.tss.Name = "tss";
            this.tss.Size = new System.Drawing.Size(115, 6);
            // 
            // tsmiDir
            // 
            this.tsmiDir.Name = "tsmiDir";
            this.tsmiDir.Size = new System.Drawing.Size(118, 22);
            this.tsmiDir.Text = "文件夹";
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            this.tsmiRename.Size = new System.Drawing.Size(106, 22);
            this.tsmiRename.Text = "重命名";
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(106, 22);
            this.tsmiDelete.Text = "删除";
            // 
            // tsmiDownload
            // 
            this.tsmiDownload.Name = "tsmiDownload";
            this.tsmiDownload.Size = new System.Drawing.Size(106, 22);
            this.tsmiDownload.Text = "下载";
            // 
            // FileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 404);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tvFile);
            this.Name = "FileBrowser";
            this.Text = "FileBrowser";
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TreeView tvFile;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem tsmiCat;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreate;
        private System.Windows.Forms.ToolStripMenuItem tsmiRename;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiTxt;
        private System.Windows.Forms.ToolStripMenuItem tsmiDir;
        private System.Windows.Forms.ToolStripSeparator tss;
        private System.Windows.Forms.ToolStripMenuItem tsmiDownload;
        private System.Windows.Forms.SaveFileDialog sfd;
    }
}
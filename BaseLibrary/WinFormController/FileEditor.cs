using lpp.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lpp.WinFormController
{
    public partial class FileEditor : Form
    {
        #region Delegate
        public delegate bool SaveDelegate(string content);
        #endregion

        #region Action
        private SaveDelegate Save;
        #endregion

        #region Action
        #endregion

        #region Initialize
        public FileEditor(string title, string content, SaveDelegate save = null)
        {
            InitializeComponent();

            try
            {
                this.Text = title;
                txtContent.Text = content;
                btnSave.Visible = save != null;
                Save = save;
                InitEvent();
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        private void InitEvent()
        {
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            txtContent.KeyDown += FileEditor_KeyDown;
        }
        #endregion

        #region Event Handler
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save(txtContent.Text))
                {
                    this.Close();
                }
                else
                {
                    MsgBox.ShowError("出错", "保存失败！");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void FileEditor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }
        #endregion
    }
}

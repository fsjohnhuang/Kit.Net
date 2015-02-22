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
    public partial class RenameDialog : Form
    {
        #region Delegate
        public delegate bool SaveDelegate(string newName);
        #endregion

        #region Action
        public SaveDelegate Save;
        #endregion

        #region Initialize
        public RenameDialog(string name, SaveDelegate save)
        {
            Save = save;
            InitializeComponent();

            try
            {
                txtName.Text = name;
                btnSave.Click += btnSave_Click;
                btnCancel.Click += btnCancel_Click;
                txtName.KeyDown += RenameDialog_KeyDown;
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        #endregion

        #region Event Handler

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save(txtName.Text))
                {
                    this.Close();
                }
                else
                {
                    MsgBox.ShowError("出错", "重命名失败！");
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

        void RenameDialog_KeyDown(object sender, KeyEventArgs e)
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

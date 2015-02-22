using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace lpp.WinFormController
{
    public sealed class MsgBox
    {
        public static string TIPS = "提示";
        public static string WARNS = "警告";
        public static string ERROR = "错误";
        public static string CONFIRM = "询问";

        public delegate void SuccessCallback();
        public delegate void FailureCallback();

        public static void ShowYNQuestion(string title, string text, SuccessCallback yesHandler = null, FailureCallback noHandler = null)
        {
            DialogResult result = MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                if (yesHandler != null)
                {
                    yesHandler();
                }
            }
            else if (noHandler != null)
            {
                noHandler();
            }
        }

        public static void ShowInfo(string title, string text, SuccessCallback after = null)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (after != null)
            {
                after();
            }
        }

        public static void ShowError(string title, string text, SuccessCallback after = null)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (after != null)
            {
                after();
            }
        }

        public static void ShowTips(string text, bool isShow = false, SuccessCallback after = null)
        {
            if (isShow)
            {
                ShowInfo(TIPS, text, after);
            }
        }
    }
}

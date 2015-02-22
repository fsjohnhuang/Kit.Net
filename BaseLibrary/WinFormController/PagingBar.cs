using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace lpp.WinFormController
{
    public delegate DataTable QueryByPaging(int start, int limit, out int total);

    public partial class PagingBar : UserControl
    {
        private static string PagingInfo = "总共{0}条记录，当前第{1}页，共{2}页，每页{3}记录";
        private int curIndex;
        private int total;

        public DataGridView Dgv { get; set; }
        public int PageSize { get; set; }
        public QueryByPaging QueryByPaging { get; set; }

        private string oldTxtofTxtPage = "1";

        #region Initialization
        public PagingBar()
        {
            InitializeComponent();
            lblPageInfo.Text = string.Empty;
        }

        public void Init(DataGridView dgv, int pageSize, QueryByPaging queryByPaging)
        {
            Dgv = dgv;
            PageSize = pageSize;
            QueryByPaging = queryByPaging;
            Init();
        }

        public void Init()
        {
            // Event Binding
            txtPage.KeyPress += txtPage_KeyPress;
            txtPage.KeyUp += txtPage_KeyUp;
            txtPage.LostFocus += txtPage_LostFocus;
            btnGoTo.Click += btnGoTo_Click;
            btnFirst.Click += btnFirst_Click;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
            btnLast.Click += btnLast_Click;
        }

        #endregion

        #region Event Handler
        void txtPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPage.Text) && (Convert.ToInt32(txtPage.Text) <= 0 || Convert.ToInt32(txtPage.Text) > this.total / PageSize + (this.total % PageSize == 0 ? 0 : 1)))
            {
                txtPage.Text = oldTxtofTxtPage;
            }
        }

        void txtPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "[0-9]") && ((int)e.KeyChar) != 8)
            {
                e.Handled = true;
            }
            else if (!string.IsNullOrEmpty(txtPage.Text) && Convert.ToInt32(txtPage.Text) > 0 && Convert.ToInt32(txtPage.Text) <= this.total / PageSize + (this.total % PageSize == 0 ? 0 : 1))
            {
                oldTxtofTxtPage = txtPage.Text;
            }
        }

        void txtPage_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPage.Text))
            {
                txtPage.Text = oldTxtofTxtPage;
            }
        }

        void btnGoTo_Click(object sender, EventArgs e)
        {
            GoTo();
        }

        void btnFirst_Click(object sender, EventArgs e)
        {
            First();
        }

        void btnPrev_Click(object sender, EventArgs e)
        {
            Prev();
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        void btnLast_Click(object sender, EventArgs e)
        {
            Last();
        }

        #endregion

        #region Public Interface
        public void First()
        {
            int total;
            curIndex = 0;
            DataTable ds = QueryByPaging(0, PageSize, out total);
            RefreshInfo(ds, PageSize, total, curIndex);
        }

        public void Prev()
        {
            int total;
            --curIndex;
            DataTable ds = QueryByPaging(curIndex * PageSize, PageSize, out total);
            RefreshInfo(ds, PageSize, total, curIndex);
        }

        public void Next()
        {
            int total;
            ++curIndex;
            DataTable ds = QueryByPaging(curIndex * PageSize, PageSize, out total);
            RefreshInfo(ds, PageSize, total, curIndex);
        }

        public void Last()
        {
            curIndex = this.total / PageSize + (this.total % PageSize == 0 ? 0 : 1) - 1;
            int total;
            DataTable ds = QueryByPaging(curIndex * PageSize, PageSize, out total);
            RefreshInfo(ds, PageSize, total, curIndex);
        }

        public void GoTo()
        {
            int total;
            if (string.IsNullOrEmpty(txtPage.Text))
            {
                txtPage.Text = oldTxtofTxtPage;
            }
            curIndex = Convert.ToInt32(txtPage.Text) - 1;
            DataTable ds = QueryByPaging(curIndex * PageSize, PageSize, out total);
            RefreshInfo(ds, PageSize, total, curIndex);
        }
        #endregion

        #region Private Tools
        private void RefreshInfo(DataTable ds, int pageSize, int total, int curIndex)
        {
            this.total = total;
            int totalPage = total / pageSize + (total % pageSize == 0 ? 0 : 1);
            lblPageInfo.Text = string.Format(PagingInfo, total, curIndex + 1, totalPage, pageSize);
            txtPage.Text = (curIndex + 1).ToString();

            if (totalPage <= 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnGoTo.Enabled = false;
            }
            else if (curIndex == 0)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
            else if (curIndex + 1 == totalPage)
            {
                btnFirst.Enabled = true;
                btnPrev.Enabled = true;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }

            Dgv.DataSource = ds;
        }
        #endregion
    }
}

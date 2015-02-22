using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Win
{
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();

            InitData();
            InitCfg();
            InitEvent();
        }

        private void InitData()
        {
            dt.TableName = "测试数据表";
            dt.Columns.Add("IP");
            dt.Columns.Add("类型");
            dt.Rows.Add(new object[] { "10.248.10.11", "应急" });
            dt.Rows.Add(new object[] { "10.248.10.94", "生产" });
            dt.Rows.Add(new object[] { "192.168.10.100", "测试" });
        }

        private void InitCfg()
        {
            cfg.DataSource = dt;

            cfg.Cols["IP"].Visible = false;
            cfg.Cols["类型"].ComboList = "生产|应急|测试";
        }

        private void InitEvent()
        {
            btnView.Click += btnView_Click;
        }

        void btnView_Click(object sender, EventArgs e)
        {
            string str = "";
            cst.SetToolTip(sender as Control, "stt");
        }
    }
}

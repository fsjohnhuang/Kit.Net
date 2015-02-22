using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using C1.Win.C1FlexGrid;
using lpp.LogHelper;
using System.Reflection;
using lpp.CollectionHelper;

namespace lpp.WinFormController
{
    public partial class SelectLists : UserControl
    {
        #region 委托
        public delegate DataTable TransitData(DataRowView[] srcRows, DataTable oldDestDT);
        public delegate void ExchangeData(DataRowView[] srcRows, DataRowView[] destRows, out DataTable srcDT, out DataTable destDT);
        public delegate void ConfigUI(C1FlexGrid cfg);
        #endregion

        #region 成员对象
        private DataTable srcOrigDT;
        private DataTable destOrigDT;

        private TransitData src2DestTransition;
        private TransitData dest2SrcTransition;
        private ExchangeData dataExchange;
        private ConfigUI configSrcUI;
        private ConfigUI configDestUI;

        public TransitData Src2DestTransition { set { src2DestTransition = value; } }
        public TransitData Dest2SrcTransition { set { dest2SrcTransition = value; } }
        public ExchangeData DataExchange { set { dataExchange = value; } }
        #endregion

        #region Initialize
        public SelectLists()
        {
            InitializeComponent();

            try
            {
                InitUI();
                InitEvent();
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        private void InitUI()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            btnRollback.Image = Image.FromStream(executingAssembly.GetManifestResourceStream("lpp.WinFormController.imgs.arrow_rotate_anticlockwise.png"));
            btnSrc2Dest.Image = Image.FromStream(executingAssembly.GetManifestResourceStream("lpp.WinFormController.imgs.arrow_right.png"));
            btnDest2Src.Image = Image.FromStream(executingAssembly.GetManifestResourceStream("lpp.WinFormController.imgs.arrow_left.png"));
            btnExchange.Image = Image.FromStream(executingAssembly.GetManifestResourceStream("lpp.WinFormController.imgs.arrow_refresh.png"));

            cst.SetToolTip(btnRollback, "恢复原来状态");
            cst.SetToolTip(btnSrc2Dest, "将数据从左边移到右边");
            cst.SetToolTip(btnDest2Src, "将数据从右边移到左边");
            cst.SetToolTip(btnExchange, "互换数据");
        }

        private void InitEvent()
        {
            btnRollback.Click += btnRollback_Click;
            btnSrc2Dest.Click += btnSrc2Dest_Click;
            btnDest2Src.Click += btnDest2Src_Click;
            btnExchange.Click += btnExchange_Click;
        }
        #endregion

        #region Event Handler
        void btnRollback_Click(object sender, EventArgs e)
        {
            try
            {
                BindData(srcOrigDT.Copy(), destOrigDT.Copy());
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void btnSrc2Dest_Click(object sender, EventArgs e)
        {
            try
            {
                if (cfgSrc.Selection.TopRow == -1)
                {
                    MsgBox.ShowInfo("提示", "请选择有效记录!");
                    return;
                }

                DataTable dt = src2DestTransition(GetSelectedDataRowViews(cfgSrc), cfgDest.DataSource as DataTable);
                RemoveSelectedRows(cfgSrc);
                cfgDest.DataSource = dt;
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void btnDest2Src_Click(object sender, EventArgs e)
        {
            try
            {
                if (cfgDest.Selection.TopRow == -1)
                {
                    MsgBox.ShowInfo("提示", "请选择有效记录!");
                    return;
                }

                DataTable dt = dest2SrcTransition(GetSelectedDataRowViews(cfgDest), cfgSrc.DataSource as DataTable);
                RemoveSelectedRows(cfgDest);
                cfgSrc.DataSource = dt;
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void btnExchange_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable srcDT, destDT;
                dataExchange(GetAllDataRowViews(cfgSrc), GetAllDataRowViews(cfgDest)
                    , out srcDT, out destDT);
                BindData(srcDT, destDT);
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 绑定数据到cfgSrc
        /// </summary>
        /// <param name="dt"></param>
        public void BindData2Src(DataTable dt, ConfigUI configUI = null)
        {
            if (srcOrigDT == null)
            {
                srcOrigDT = dt.Copy();
            }
            cfgSrc.DataSource = dt;

            if (null == configSrcUI && null != configUI)
            {
                configSrcUI = configUI;
            }
            if (null != configSrcUI)
            {
                configSrcUI(cfgSrc);
            }
        }

        /// <summary>
        /// 绑定数据到cfgDest
        /// </summary>
        /// <param name="dt"></param>
        public void BindData2Dest(DataTable dt, ConfigUI configUI = null)
        {
            if (destOrigDT == null)
            {
                destOrigDT = dt.Copy();
            }
            cfgDest.DataSource = dt;

            if (null == configDestUI && null != configUI)
            {
                configDestUI = configUI;
            }
            if (null != configDestUI)
            {
                configDestUI(cfgDest);
            }
        }

        /// <summary>
        /// 绑定数据到cfgSrc和cfgDest
        /// </summary>
        /// <param name="srcDt"></param>
        /// <param name="destDt"></param>
        public void BindData(DataTable srcDt, DataTable destDt
            , ConfigUI configSrcUI = null, ConfigUI configDestUI = null)
        {
            BindData2Src(srcDt, configSrcUI);
            BindData2Dest(destDt, configDestUI);
        }

        /// <summary>
        /// 根据列名获取cfgSrc的列对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Column GetSrcColumnByName(string name)
        {
            return cfgSrc.Cols[name];
        }

        /// <summary>
        /// 根据列名获取cfgDest的列对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Column GetDestColumnByName(string name)
        {
            return cfgDest.Cols[name];
        }

        /// <summary>
        /// 获取cfgSrc数据表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSrcDT()
        {
            return (cfgSrc.DataSource as DataTable).Copy();
        }

        /// <summary>
        /// 获取cfgDest数据表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDestDT()
        {
            return (cfgDest.DataSource as DataTable).Copy();
        }
        #endregion

        #region Public Uitl
        /// <summary>
        /// 将某一个列设置为combobox列
        /// </summary>
        /// <param name="col">列对象</param>
        /// <param name="selections">可选项值</param>
        public void GenerateComboBox(Column col, string[] selections)
        {
            string comboListStr = ArrayHelper.Join<string>(selections, "|");
            col.ComboList = comboListStr;
        }
        #endregion

        #region Private Util
        /// <summary>
        /// 获取选择项的数据
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private DataRowView[] GetSelectedDataRowViews(C1FlexGrid cfg)
        {
            int startRowIndex = cfg.Selection.TopRow;
            int endRowIndex = cfg.Selection.BottomRow;
            List<DataRowView> drs = new List<DataRowView>();
            if (startRowIndex != -1 && endRowIndex != -1)
            {
                for (int i = startRowIndex, len = endRowIndex + 1; i < len; i++)
                {
                    drs.Add(cfg.Rows[i].DataSource as DataRowView);
                }
            }
            else if (null != cfg.DataSource)
            {
                DataView dv = (cfg.DataSource as DataTable).DefaultView;
                for (int i = 0, len = dv.Count; i < len; i++)
                {
                    drs.Add(dv[i]);
                }
            }

            return drs.ToArray();
        }

        private DataRowView[] GetAllDataRowViews(C1FlexGrid cfg)
        {
            List<DataRowView> drs = new List<DataRowView>();
            if (null != cfg.DataSource)
            {
                DataView dv = (cfg.DataSource as DataTable).DefaultView;
                for (int i = 0, len = dv.Count; i < len; i++)
                {
                    drs.Add(dv[i]);
                }
            }

            return drs.ToArray();
        }

        /// <summary>
        /// 移除选中的行数据
        /// </summary>
        /// <param name="cfg"></param>
        private void RemoveSelectedRows(C1FlexGrid cfg)
        {
            DataTable origDT = cfg.DataSource as DataTable;
            for (int i = cfg.Selection.TopRow - 1; i < cfg.Selection.BottomRow; i++)
            {
                origDT.Rows.RemoveAt(i);
            }

            cfg.DataSource = origDT;
        }
        #endregion
    }
}

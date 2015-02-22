using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace lpp.WinFormController
{
    /// <summary>
    /// Combobox帮助类
    /// </summary>
    public sealed class Cbx
    {
        /// <summary>
        /// 绑定字典到Combobox
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="cbx">Combobox组件</param>
        /// <param name="data">数据</param>
        /// <param name="reverse">默认为false，表示数据的K是DisplayMember,V是ValueMember；若reverse是true，则相反</param>
        public static void BindData<K, V>(ComboBox cbx, IDictionary<K, V> data, bool reverse = false)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = data;
            cbx.DataSource = bs;
            cbx.DisplayMember = (reverse ? "Value" : "Key");
            cbx.ValueMember = (reverse ? "Key" : "Value");
        }
    }
}

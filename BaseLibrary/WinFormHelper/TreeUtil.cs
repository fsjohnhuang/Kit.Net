using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using lpp.StringHelper;

namespace lpp.WinFormHelper
{
    public static class TreeUtil
    {
        /// <summary>
        /// 根据属性和属性值
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetTreeNodeIndex(TreeNodeCollection treeNodes
            , string propName
            , string[] keywords
            , MatchType matchType = MatchType.ALL)
        {
            const int NON_INDEX = -1;

            if (treeNodes == null || treeNodes.Count == 0 || string.IsNullOrEmpty(propName) || keywords == null || keywords.Length == 0) return NON_INDEX;

            Type treeNodeType = typeof(TreeNode);
            PropertyInfo pi = treeNodeType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (pi == null) return NON_INDEX;

            int index = NON_INDEX;
            for (int i = 0, len = treeNodes.Count; i < len && index == NON_INDEX; i++)
            {
                if (Str.IsMatch((string)pi.GetValue(treeNodes[i], null), keywords, matchType))
                    index = i;
            }

            return index;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lpp.ExtHelper
{
    public class EnumCls
    {
        /// <summary>
        /// Grid的Column类型
        /// </summary>
        /// <remarks>Extj4.1</remarks>
        public enum ColType
        {
            templatecolumn,
            gridcolumn,
            actioncolumn,
            numbercolumn,
            booleancolumn,
            datecolumn
        }

        /// <summary>
        /// Store的Field类型
        /// </summary>
        /// <remarks>Extjs4.1</remarks>
        public enum FieldType
        {
            auto,
            @int,
            @string
        }

        /// <summary>
        /// Grid的Feature类型
        /// </summary>
        public enum FeatureType
        {
            RowBody,
            Summary
        }

        /// <summary>
        /// Grid的Summary feature类型
        /// </summary>
        public enum SummaryType
        {
            count,
            sum,
            min,
            max,
            average
        }

        /// <summary>
        /// 内置呈现器
        /// </summary>
        /// <remarks>Extj4.1</remarks>
        public enum Renderer
        {
            usMoney
        }

        /// <summary>
        /// 获取内置呈现器
        /// </summary>
        /// <remarks>Extj4.1</remarks>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public string GetRenderer(Renderer renderer)
        {
            string result = string.Empty;

            Dictionary<string, string> renderers = new Dictionary<string, string>();
            renderers.Add(Renderer.usMoney.ToString(), "Ext.util.Format.usMoney");

            if (renderers.ContainsKey(renderer.ToString()))
            {
                result = renderers[renderer.ToString()];
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;

namespace lpp.CommonHelper
{
    public static class ExportHelper
    {
        /// <summary>
        /// 针对WEB控件导出EXCEL
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ExcelFileName"></param>
        public static void ExportToExcel(System.Web.UI.Control control, string ExcelFileName)
        {
            HtmlForm form = new HtmlForm();
            form.Controls.Add(control);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.Charset = "UTF8";

            context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(ExcelFileName, System.Text.Encoding.UTF8) + ".xls");

            context.Response.ContentEncoding = System.Text.Encoding.UTF8;//设置输出流为简体中文
            context.Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。 

            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            control.RenderControl(oHtmlTextWriter);
            string charset = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf8\">";
            context.Response.Write(charset + oStringWriter.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 针对WEB控件导出word
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ExcelFileName"></param>
        public static void ExportToWord(System.Web.UI.Control control, string WordFileName)
        {
            HtmlForm form = new HtmlForm();
            form.Controls.Add(control);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.Charset = "UTF8";

            context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(WordFileName, System.Text.Encoding.UTF8) + ".doc");

            context.Response.ContentEncoding = System.Text.Encoding.UTF8;//设置输出流为简体中文
            context.Response.ContentType = "application/ms-Word";//设置输出文件类型为excel文件。 

            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            control.RenderControl(oHtmlTextWriter);
            string charset = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf8\">";
            context.Response.Write(charset + oStringWriter.ToString());
            context.Response.End();
        }
    }
}

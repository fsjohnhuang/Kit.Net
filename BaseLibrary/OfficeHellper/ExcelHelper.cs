using System;
using System.Collections.Generic;
using System.Text;

using Aspose.Cells;
using System.Data;

namespace lpp.OfficeHellper
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <param name="excelFileName">文件名称</param>
        public static void Export(DataTable dt, string excelFileName)
        {
            Workbook wb = new Workbook();
            if (wb.Worksheets.Count == 0)
                wb.Worksheets.Add(dt.TableName);
            Worksheet ws = wb.Worksheets[0];
            ws.Name = dt.TableName;
            for (int i = 0, len = dt.Columns.Count; i < len; ++i)
            {
                ws.Cells[0, i].Value = dt.Columns[i].ColumnName;
            }

            for (int i = 0, iLen = dt.Rows.Count; i < iLen; ++i)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 0, jLen = dt.Columns.Count; j < jLen; ++j)
                {
                    ws.Cells[i + 1, j].Value = dr[j];
                }
            }

            wb.Save(excelFileName);
        }
    }
}

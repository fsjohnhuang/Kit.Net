using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBAttr
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ColumnAttr : Attribute
    {
        private bool m_IsPrimary = false; // 是否为主键
        private bool m_IsAutoGenerate = false; // 是否自增序列
        private object m_IgnoreValue = null; // 当属性值为该值时则不往数据库传送

        public bool IsPrimary { get { return m_IsPrimary; } set { m_IsPrimary = value; } }
        public bool IsAutoGenerate { get { return m_IsAutoGenerate; } set { m_IsAutoGenerate = value; } }
        public object IgnoreValue { get { return m_IgnoreValue; } set { m_IgnoreValue = value; } }


        public string TblAlias { get; set; }
        public string ColName { get; set; }
        public string RelatedTblAlias { get; set; }
        public string RelatedColName { get; set; }
        public JoinType JoinType { get; set; }

        public ColumnAttr(string colName = "", string tblAlias = "A", string relatedColName = "", string relatedTblAlias = "", JoinType joinType = JoinType.NONE)
        {
            ColName = colName;
            TblAlias = tblAlias;
            RelatedColName = relatedColName;
            RelatedTblAlias = relatedTblAlias;
            JoinType = joinType;
        }
    }

    public enum JoinType
    {
        NONE,
        INNER_JOIN,
        LEFT_OUTER_JOIN,
        RIGHT_OUTER_JOIN,
        CROSS_JOIN
    }
}

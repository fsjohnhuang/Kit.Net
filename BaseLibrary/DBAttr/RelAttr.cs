using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBAttr
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RelAttr : Attribute
    {
        public string TblAlias { get; set; }
        public string RelatedTblAlias { get; set; }
        public JoinType JoinType { get; set; }
        public string JoinConstraint { get; set; }
        public int Index { get; set; }

        public RelAttr(string tblAlias, string relatedTblAlias, JoinType joinType = JoinType.INNER_JOIN, string joinConstraint = "", int index = 0)
        {
            TblAlias = tblAlias;
            RelatedTblAlias = relatedTblAlias;
            JoinType = joinType;
            JoinConstraint = joinConstraint;
            Index = index;
        }
    }
}

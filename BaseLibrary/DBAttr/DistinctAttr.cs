using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBAttr
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DistinctAttr : Attribute
    {
        public DistinctAttr() { }
    }
}

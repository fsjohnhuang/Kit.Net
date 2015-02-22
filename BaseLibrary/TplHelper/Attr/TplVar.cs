using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.TplHelper.Attr
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class TplVar : Attribute
    {
        public string Name { get; set; }
        public string Format { get; set; }

        public TplVar(string name)
        {
            Name = name;
        }
    }
}

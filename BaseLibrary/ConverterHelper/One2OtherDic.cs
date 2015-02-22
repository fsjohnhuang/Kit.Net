using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.ConverterHelper
{
    public class One2OtherDic
    {
        public delegate object ConverterHandler(object state);

        public string SrcPropName { get; set; }
        public One2OtherDic.ConverterHandler Convert { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class FileService : MarshalByRefObject
    {
        public string GetFileName()
        {
            return "BINGO!";
        }
    }
}

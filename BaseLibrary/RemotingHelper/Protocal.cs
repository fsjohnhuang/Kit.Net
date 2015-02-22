using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.RemotingHelper
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public sealed class Protocal
    {
        public readonly static Protocal IPC = new Protocal(0, "ipc");
        public readonly static Protocal HTTP = new Protocal(1, "http");
        public readonly static Protocal TCP = new Protocal(2, "tcp");

        public int Value { get; private set; }
        public string Str { get; private set; }

        public Protocal(int value, string str)
        {
            Value = value;
            Str = str;
        }
    }
}

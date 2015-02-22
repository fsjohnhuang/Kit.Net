using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lpp.FlowHelper
{
    /// <summary>
    /// 流程帮助类
    /// </summary>
    public sealed class Flow
    {
        /// <summary>
        /// 管道模块函数
        /// </summary>
        /// <param name="state">管道流域参数</param>
        /// <param name="prevPipingItemReturnValue">前一个管道模块的返回值</param>
        /// <returns>管道模块的返回值</returns>
        public delegate PipingItemReturnValue PipingItem(IDictionary<string, object> state, IDictionary<string, object> prevPipingItemReturnValue = null);
        /// <summary>
        /// 跳出管道时的回调函数
        /// </summary>
        /// <param name="lastPipingItemReturnValue">最后一个管道模块的返回值</param>
        public delegate void PipingCallback(PipingItemReturnValue lastPipingItemReturnValue);

        /// <summary>
        /// 管道流，当管道流中的其中一个模块返回false时则跳出管道并执行回调函数
        /// </summary>
        /// <param name="pipingItems">管道</param>
        /// <param name="state">管道流域参数</param>
        /// <param name="pipingCallback">跳出管道时的回调函数</param>
        public static void Piping(PipingItem[] pipingItems, IDictionary<string, object> state = null, PipingCallback pipingCallback = null, Interrupter interrupter = null)
        {
            PipingItemReturnValue curPipingItemReturnValue = null;
            if (state == null)
            {
                state = new Dictionary<string, object>();
            }
            if (interrupter != null)
            {
                state["Interrupt"] = interrupter.Interrupt;
            }
            for (int i = 0, len = pipingItems.Length; i < len && (curPipingItemReturnValue == null || curPipingItemReturnValue.IsSuccess); i++)
            {
                curPipingItemReturnValue = pipingItems[i](state, (curPipingItemReturnValue == null ? null : curPipingItemReturnValue.Objs));
            }

            if (pipingCallback == null) return;

            pipingCallback(curPipingItemReturnValue);
        }
    }

    public class Interrupter
    {
        public class PASVService
        {
            public string TypeName { get; set; }
            public string Method { get; set; }
        }

        private IDictionary<string, PASVService> PASVServiceMap = new Dictionary<string, PASVService>();
        public IDictionary<string, bool> Symbol = new Dictionary<string, bool>();

        public delegate object InterruptDelegate(string serviceName, params object[] states);
        public InterruptDelegate Interrupt;

        public Interrupter()
        {
            Interrupt = (serviceName, states) => {
                if (!PASVServiceMap.ContainsKey(serviceName)) return null;

                object returnValue = null;
                Symbol[serviceName] = true;

                ThreadPool.QueueUserWorkItem((state) => {
                    PASVService pasvService = PASVServiceMap[serviceName];
                    if (null != pasvService)
                    {
                        Type type = Type.GetType(PASVServiceMap[serviceName].TypeName);
                        type.GetMethod(PASVServiceMap[serviceName].Method).Invoke(Activator.CreateInstance(type), states);
                    }
                });

                while (Symbol.ContainsKey(serviceName) && Symbol[serviceName])
                {
                    Thread.Sleep(500);
                }

                return returnValue;
            };
        }

        public void AddPASVService(string serviceName, PASVService pasvService)
        {
            PASVServiceMap[serviceName] = pasvService;
        }
    }
}

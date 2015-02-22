using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lpp.FlowHelper
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public sealed class Interceptor
    {
        public delegate PipingItemReturnValue Before();
        public delegate PipingItemReturnValue TargetFn(PipingItemReturnValue returnValue);
        public delegate PipingItemReturnValue After(PipingItemReturnValue returnValue);
        public delegate void Error(PipingItemReturnValue returnValue);
        public delegate void Must(PipingItemReturnValue returnValue);
        
        public delegate void FnWithInterceptor();

        /// <summary>
        /// 生成同步拦截器，即before、targetFn、after、error会按顺序执行并整个过程均阻塞当前线程。
        /// </summary>
        /// <param name="targetFn">原函数</param>
        /// <param name="before">前置拦截器</param>
        /// <param name="after">后置拦截器</param>
        /// <param name="error">异常处理</param>
        /// <param name="must">无论before、targeFn、after和error函数执行结果如何都必须执行的函数</param>
        /// <returns>加工后带拦截器的函数</returns>
        public static FnWithInterceptor CreateInterceptor(TargetFn targetFn, Before before = null, After after = null, Error error = null, Must must = null)
        {
            return () => { 
                PipingItemReturnValue returnValue = new PipingItemReturnValue();
                if (null != before)
                {
                    returnValue = before();
                }
                try
                {
                    if (null != targetFn)
                    {
                        returnValue = targetFn(returnValue);
                        if (null != after)
                        {
                            after(returnValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (null != error)
                    {
                        returnValue.Ex = ex;
                        error(returnValue);
                    }
                }
                finally
                {
                    if (null != must)
                    {
                        must(returnValue);
                    }
                }
            };
        }

        /// <summary>
        /// 生成异步拦截器，即before、targetFn、after、error会按顺序执行但仅before函数阻塞当前线程，后续函数均在另一个线程中执行。
        /// </summary>
        /// <param name="targetFn">原函数</param>
        /// <param name="before">前置拦截器</param>
        /// <param name="after">后置拦截器</param>
        /// <param name="error">异常处理</param>
        /// <param name="must">无论before、targeFn、after和error函数执行结果如何都必须执行的函数</param>
        /// <returns>加工后带拦截器的函数</returns>
        public static FnWithInterceptor CreateAsyncInterceptor(TargetFn targetFn, Before before = null, After after = null, Error error = null, Must must = null)
        {
            return () =>
            {
                PipingItemReturnValue returnValue = new PipingItemReturnValue();
                if (null != before)
                {
                    returnValue = before();
                }

                if (null != targetFn)
                {
                    ThreadPool.QueueUserWorkItem((state) =>
                    {
                        try
                        {
                            returnValue = targetFn(returnValue);
                            if (null != after)
                            {
                                after(returnValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (null != error)
                            {
                                returnValue.Ex = ex;
                                error(returnValue);
                            }
                        }
                        finally
                        {
                            if (null != must)
                            {
                                must(returnValue);
                            }
                        }
                    });
                }
                else if (null != must)
                {
                    must(returnValue);
                }
            };
        }
    }
}

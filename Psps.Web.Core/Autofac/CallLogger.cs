using Castle.DynamicProxy;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Web.Core.Autofac
{
    public class CallLogger : IInterceptor
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CallLogger()
        {
        }

        public void Intercept(IInvocation invocation)
        {
            Stopwatch sw = new Stopwatch();

            _logger.Debug(string.Format("Calling method {0}.{1} with parameters {2}... ",
                invocation.TargetType.FullName,
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())));

            sw.Start();
            invocation.Proceed();
            sw.Stop();

            _logger.Debug(string.Format("Done: result from {0} was {1} in {2}ms.", invocation.Method.Name, invocation.ReturnValue, sw.ElapsedMilliseconds));
        }
    }
}
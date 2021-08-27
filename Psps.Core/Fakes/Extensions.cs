using Psps.Core.Helper;
using System;
using System.Web;

namespace Psps.Core.Fakes
{
    public static class Extensions
    {
        /// <summary>
        /// Indicates whether this context is fake
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns>Result</returns>
        public static bool IsFakeContext(this HttpContextBase httpContext)
        {
            Ensure.Argument.NotNull(httpContext, "httpContext");

            return httpContext is FakeHttpContext;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Psps.Web.Core.Mvc
{
    public enum JsonResponseType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public class JsonResponseFactory
    {
        public static object ErrorResponse(string message)
        {
            return new { Success = false, Message = message };
        }

        public static object ErrorResponse(ModelStateDictionary modelState)
        {
            return new { Success = false, Errors = modelState.SerializeErrors(), Tag = "ValidationError" };
        }

        public static object SuccessResponse(string message)
        {
            return new { Success = true, Message = message };
        }

        public static object SuccessResponse(object data, string message = "")
        {
            return new { Success = true, Data = data, Message = message };
        }
    }
}
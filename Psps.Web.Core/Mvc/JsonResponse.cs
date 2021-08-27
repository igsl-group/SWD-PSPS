using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Core.Mvc
{
    [Flags]
    public enum JsonResponseTag
    {
        ValidationError
    }

    public class JsonResponse
    {
        /// <summary>
        /// Represent a json response
        /// </summary>
        /// <param name="sucess">Is it success</param>
        public JsonResponse(bool success)
        {
            this.Success = success;
        }

        /// <summary>
        /// Indicate whether the request is success or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message need to be sent to client
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Store the errors for model state, <see cref="ModelStateExtensions.SerializeErrors()"/> for more information.
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// Store the extra data for client
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Tag of the response
        /// </summary>
        public JsonResponseTag Tag { get; set; }

        /// <summary>
        ///
        /// </summary>
        //public Dictionary<string, object> CustomProperties { get; set; }
    }
}
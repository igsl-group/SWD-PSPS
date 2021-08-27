using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.OGCIO
{
    public class Result
    {
        /// <summary>
        /// Status 2XX - Success of some kind, 4XX - Error occurred in client’s part, 5XX - Error
        /// occurred in server’s part
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Raw content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Session ID that associates with the error
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Error details
        /// </summary>
        public List<string> ErrorList { get; set; }

        public bool HasError { get { return ErrorList != null; } }
    }
}
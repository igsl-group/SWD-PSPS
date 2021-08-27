using System.Collections.Generic;

namespace Novacode
{
    public class Headers
    {
        public Header odd;

        public Header even;

        public Header first;

        public List<Header> headers;

        internal Headers()
        {
            headers = new List<Header>();
        }

        public int Count()
        {
            return (odd != null ? 1 : 0) + (even != null ? 1 : 0) + (first != null ? 1 : 0) + headers.Count;
        }
    }
}
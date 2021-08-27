using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data
{
    public class QueryTransformer : NHibernate.Transform.IResultTransformer
    {
        public List<string> columnsNames = new List<string>();
        private object[] result = null;

        #region IResultTransformer Members

        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            result = tuple;
            for (int i = 0; i < tuple.Length; i++)
            {
                //Getting the column Names collection from the aliases object list
                if (!columnsNames.Contains(aliases[i]))
                    columnsNames.Add(aliases[i]);
            }
            return result;
        }

        #endregion IResultTransformer Members
    }
}
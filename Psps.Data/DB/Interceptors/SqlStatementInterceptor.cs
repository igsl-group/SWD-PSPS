using NHibernate;
using NHibernate.Cfg;
using System.Diagnostics;

namespace Psps.Data.DB.Interceptors
{
    public class SqlStatementInterceptor : InterceptorDecorator
    {
        public SqlStatementInterceptor(IInterceptor innerInterceptor)
            : base(innerInterceptor)
        {
        }

        public override global::NHibernate.SqlCommand.SqlString OnPrepareStatement(global::NHibernate.SqlCommand.SqlString sql)
        {
            Trace.WriteLine(sql.ToString());
            return sql;
        }
    }
}
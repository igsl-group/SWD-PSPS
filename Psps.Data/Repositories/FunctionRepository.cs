using NHibernate;
using NHibernate.Transform;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Data.Repositories
{
    public interface IFunctionRepository : IRepository<Function, string>
    {
        IList<FunctionDto> GetFunctionsByRoleId(string roleId);
    }

    public class FunctionRepository : BaseRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(ISession session)
            : base(session)
        {
        }

        public IList<FunctionDto> GetFunctionsByRoleId(string roleId)
        {
            string sql = "select f.FunctionId,f.[Module],f.Description,CAST(CASE WHEN f2.RoleId IS NULL THEN 0 ELSE 1 END AS BIT) AS IsEnabled ,f2.FunctionsInRolesId, f2.RoleId " +
                "from [Function] f LEFT JOIN FunctionsInRoles f2 ON f.FunctionId=f2.FunctionId and f2.RoleId=:roleId ORDER BY f.[Module], f.FunctionId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetString("roleId", roleId);

            var result = query
                 .SetResultTransformer(Transformers.AliasToBean(typeof(FunctionDto)))
                 .List<FunctionDto>();

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Psps.Models.Domain;
using NHibernate;
using Psps.Data.Infrastructure;

namespace Psps.Data.Repositories
{
    public interface IFunctionsInRoleRepository : IRepository<FunctionsInRoles, int>
    {
        int GetRecordCountByRoleId(string roleId);
        IList<FunctionsInRoles> GetByRoleId(string roleId);
        FunctionsInRoles GetByRoleIdAndFuncId(string roleId,string funcId);
        //void DeleteByRoleId(string roleId);
    }

    public class FunctionsInRoleRepository : BaseRepository<FunctionsInRoles, int>, IFunctionsInRoleRepository
    {
        public FunctionsInRoleRepository(ISession session)
            : base(session)
        {
        }

        public int GetRecordCountByRoleId(string roleId) 
        {
            return this.Table.Where(x => x.Role.RoleId == roleId).Count();
        }

        public IList<FunctionsInRoles> GetByRoleId(string roleId) 
        {
            return this.Table.Where(x => x.Role.RoleId == roleId).ToList();
        }

        public FunctionsInRoles GetByRoleIdAndFuncId(string roleId, string funcId) 
        {
            return this.Table.Where(x => x.Role.RoleId == roleId && x.Function.FunctionId == funcId).ToList().FirstOrDefault();
        }

        /*
        public void DeleteByRoleId(string roleId) 
        {
            var hql = "DELETE FROM FunctionsInRoles f WHERE f.Role.RoleId='"+roleId+"'";
            this.Session.Delete(hql);

        }
         * */
    }
}

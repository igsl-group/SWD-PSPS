using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System.Linq;
using System.Linq.Expressions;

namespace Psps.Data.Repositories
{
    public interface ISystemParameterRepository : IRepository<SystemParameter, int>
    {
        string getValByCode(string code);
    }

    public class SystemParameterRepository : BaseRepository<SystemParameter, int>, ISystemParameterRepository
    {
        public SystemParameterRepository(ISession session)
            : base(session)
        {
        }

        public string getValByCode(string code)
        {
            var query = from u in this.Table.Where(x => x.Code == code)
                        select new
                        {
                            value = u.Value.ToString()
                        };

            return query.FirstOrDefault().value;
        }
    }
}
using NHibernate;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface IComplaintFollowUpActionRepository : IRepository<ComplaintFollowUpAction, int>
    {
        string GenerateEnclosureNum(int complaintMasterId);
    }

    public class ComplaintFollowUpActionRepository : BaseRepository<ComplaintFollowUpAction, int>, IComplaintFollowUpActionRepository
    {
        public ComplaintFollowUpActionRepository(ISession session)
            : base(session)
        {
        }

        public string GenerateEnclosureNum(int complaintMasterId)
        {
            string sql = "SELECT MAX(EnclosureNum) As EnclosureNum FROM ComplaintFollowUpAction " + Environment.NewLine +
                         "WHERE ComplaintMasterId = :complaintMasterId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetParameter("complaintMasterId", complaintMasterId);

            var result = query.UniqueResult();

            return result != null ? result.ToString() : string.Empty;
        }
    }
}
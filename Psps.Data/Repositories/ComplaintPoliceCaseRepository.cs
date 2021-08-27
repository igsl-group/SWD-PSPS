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
    public interface IComplaintPoliceCaseRepository : IRepository<ComplaintPoliceCase, int>
    {
        string GenerateRefNum(int complaintMasterId);
    }

    public class ComplaintPoliceCaseRepository : BaseRepository<ComplaintPoliceCase, int>, IComplaintPoliceCaseRepository
    {
        public ComplaintPoliceCaseRepository(ISession session)
            : base(session)
        {
        }

        public string GenerateRefNum(int complaintMasterId)
        {
            string sql = "SELECT MAX(CaseInvestigateRefNum) As CaseInvestigateRefNum FROM ComplaintPoliceCase " + Environment.NewLine +
                         "WHERE ComplaintMasterId = :complaintMasterId";

            var query = this.Session.CreateSQLQuery(sql);
            query.SetParameter("complaintMasterId", complaintMasterId);

            var result = query.UniqueResult();

            return result != null ? result.ToString() : string.Empty;
        }
    }
}
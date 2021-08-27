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
    public interface IComplaintOtherDepartmentEnquiryRepository : IRepository<ComplaintOtherDepartmentEnquiry, int>
    {
        string GenerateRefNum();
    }

    public class ComplaintOtherDepartmentEnquiryRepository : BaseRepository<ComplaintOtherDepartmentEnquiry, int>, IComplaintOtherDepartmentEnquiryRepository
    {
        public ComplaintOtherDepartmentEnquiryRepository(ISession session)
            : base(session)
        {
        }

        public string GenerateRefNum()
        {
            string sql = "SELECT MAX(RefNum) As RefNum FROM ComplaintOtherDepartmentEnquiry";
            var result = this.Session.CreateSQLQuery(sql).UniqueResult();

            return result != null ? result.ToString() : string.Empty;
        }
    }
}
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
    public interface IComplaintTelRecordRepository : IRepository<ComplaintTelRecord, int> 
    {
        string GenerateTelComplaintRef(int year);
    }

    public class ComplaintTelRecordRepository : BaseRepository<ComplaintTelRecord, int>, IComplaintTelRecordRepository
    {
        public ComplaintTelRecordRepository(ISession session)
            : base(session)
        {
        }

        public string GenerateTelComplaintRef(int year) 
        {
            string telComplaintRef = "";
            var sql = "SELECT TelComplaintRef FROM ComplaintTelRecord WHERE ComplaintTelRecordId=(SELECT MAX(ComplaintTelRecordId) FROM ComplaintTelRecord WHERE TelComplaintRef LIKE 'PSP/" + year + "/%')";
            var result=this.Session.CreateSQLQuery(sql);
            
            if (result.UniqueResult()!=null)
            {
                string resultNum = result.UniqueResult().ToString();
                var num = resultNum.Substring(resultNum.LastIndexOf("/") + 1);
                telComplaintRef = "PSP/" + year + "/" + (Convert.ToInt32(num)+1);
            }
            else 
            {
                telComplaintRef = "PSP/" + year + "/1";
            }
            return telComplaintRef;
        }

    }
}